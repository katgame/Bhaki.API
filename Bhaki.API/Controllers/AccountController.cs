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
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }


        [Authorize(Roles = UserRoles.Admin)]
        [HttpPost("credit-funds")]
        public IActionResult CreditFunds(Guid userID, double price)
        {
            var response = _accountService.CreditFunds(userID,price);
            return Ok(response);
        }

        [Authorize(Roles = UserRoles.Admin)]
        [HttpPost("debit-funds")]
        public IActionResult DebitFunds(Guid userID, double price)
        {
            var response = _accountService.DebitFunds(userID, price);
            return Ok(response);
        }
    }
}
