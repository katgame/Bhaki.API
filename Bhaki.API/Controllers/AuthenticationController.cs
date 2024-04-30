using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Dice.API.Data;
using Dice.API.Data.Models;
using Dice.API.Data.ViewModels.Authentication;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Dice.API.Data.Dto;
using Dice.API.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Dice.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;
        private readonly IProfileService _profileService;
        private readonly IAccountService _accountService;
        public AuthenticationController(UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            AppDbContext context,
            IConfiguration configuration, 
            ILogger<AuthenticationController> logger,
            IProfileService profileService,
            IAccountService accountService)
        {
            _logger = logger;
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
            _configuration = configuration;
            _profileService = profileService;
            _accountService = accountService;
        }
        //[Authorize(Roles = UserRoles.Admin)]
        [HttpPost("register-user")]
        public async Task<IActionResult> Register([FromBody]RegisterVM payload)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest("Please, provide all required fields");
                }

                var userExists = await _userManager.FindByEmailAsync(payload.Email);

                if (userExists != null)
                {
                    return BadRequest($"User {payload.Email} already exists");
                }

                ApplicationUser newUser = new ApplicationUser()
                {
                    Email = payload.Email,
                    UserName = payload.UserName,
                    SecurityStamp = Guid.NewGuid().ToString(),
                    LockoutEnabled = false
                };


                var result = await _userManager.CreateAsync(newUser, payload.Password);

                if (!result.Succeeded)
                {
                    return BadRequest("User could not be created!");
                }
                await _userManager.SetLockoutEnabledAsync(newUser, false);

                var profileCreated = _profileService.CreateProfile(Guid.Parse(newUser.Id));

                switch (payload.Role)
                {
                    case "Admin":
                        await _userManager.AddToRoleAsync(newUser, UserRoles.Admin);
                        break;
                    case "Player":
                        await _userManager.AddToRoleAsync(newUser, UserRoles.Player);
                        break;
                  
                
                }

                return Ok();
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Authorize(Roles = UserRoles.Admin)]
        [HttpGet("all-user")]
        public async Task<IActionResult> GetUsers()
        {
            var users = _userManager.Users.ToList();
            var userRole =  _roleManager.Roles.ToList();
            var response = users;
            return Ok(response);
        }
        [Authorize(Roles = UserRoles.Admin)]
        [HttpGet("get-user-roles")]
        public async Task<IActionResult> GetUserRoles()
        {
            var roles =  _roleManager.Roles.Where(x => (x.Name == "Admin") || (x.Name == "Player")).ToList();
            return Ok(roles);
        }
        [Authorize(Roles = UserRoles.Admin)]
        [HttpPut("enable-user/{userId}")]
        public async Task<IActionResult> EnableUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            await _userManager.SetLockoutEnabledAsync(user, false);
            return Ok();
        }
        [Authorize(Roles = UserRoles.Admin)]
        [HttpDelete("delete-user/{userId}")]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                
                var tokens = _context.RefreshTokens.Where(x => x.UserId == userId).ToList();
                foreach (var token in tokens)
                {
                    _context.RefreshTokens.Remove(token);
                }
                await _userManager.SetLockoutEnabledAsync(user, true);
                return Ok();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
          
        }
        [HttpPost("login-user")]
        public async Task<IActionResult> Login([FromBody]LoginVM payload)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Please, provide all required fields");
            }

            var user = await _userManager.FindByEmailAsync(payload.Email);
            if(user != null && await _userManager.CheckPasswordAsync(user, payload.Password) && user.LockoutEnabled == false)
            {
                var tokenValue = await GenerateJwtToken(user);
                _logger.LogTrace("Token value: " , tokenValue);
                var userRole = await _userManager.GetRolesAsync(user);
                _logger.LogTrace("userRole value: ", userRole);
                var accountInfo = _accountService.GetAccountInformation(Guid.Parse(user.Id));
                var loginRespose = new LoginRespose
                {
                    token = tokenValue,
                    userDetails = new UserInfo
                    {
                        Id = user.Id,
                        Role = (List<string>)userRole,
                        Name = user.UserName,
                        Email = user.Email,
                    },
                    userAccount = accountInfo
                };
                return Ok(loginRespose);
            }

            return Unauthorized();
        }



        private async Task<AuthResultVM> GenerateJwtToken(ApplicationUser user)
        {
            try
            {
                var authClaims = new List<Claim>()
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };

                //Add User Roles
                var userRoles = await _userManager.GetRolesAsync(user);
                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }


                var authSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["JWT:Secret"]));

                var token = new JwtSecurityToken(
                    issuer: _configuration["JWT:Issuer"],
                    audience: _configuration["JWT:Audience"],
                    expires: DateTime.UtcNow.AddHours(1),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

                var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);

                var refreshToken = new RefreshToken()
                {
                    JwtId = token.Id,
                    IsRevoked = false,
                    UserId = user.Id,
                    DateAdded = DateTime.UtcNow,
                    DateExpire = DateTime.UtcNow.AddMonths(6),
                    Token = Guid.NewGuid().ToString() + "-" + Guid.NewGuid().ToString()
                };
                await _context.RefreshTokens.AddAsync(refreshToken);
                await _context.SaveChangesAsync();

                var response = new AuthResultVM()
                {
                    Token = jwtToken,
                    RefreshToken = refreshToken.Token,
                    ExpiresAt = token.ValidTo
                };

                return response;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw e;
            }
           
        }

    }
}
