using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Dice.API.Data.Services;
using Dice.API.Data.ViewModels;
using Dice.API.Data.ViewModels.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dice.API.Interfaces;
using Dice.API.Data.Dto;

namespace Dice.API.Controllers
{
    [Authorize(Roles = UserRoles.Admin + "," + UserRoles.Player)]
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly IProfileService _profileService;
        public ProfileController(IProfileService profileService)
        {
            _profileService = profileService;
        }

        [Authorize(Roles = UserRoles.Admin)]
        [HttpGet("get-dashboard")]
        public IActionResult CreateProfile(Guid UserId)
        {
            var response = _profileService.CreateProfile(UserId);
            return Ok(response);
        }
    }
}
