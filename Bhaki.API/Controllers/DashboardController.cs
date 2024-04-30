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
    [Authorize(Roles = UserRoles.Admin)]
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService _dashboardService;
        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }
        
        [Authorize(Roles = UserRoles.Admin)]
        [HttpGet("get-dashboard")]
        public IActionResult GetDashboard()
        {
            var response = _dashboardService.GetDashboard();
            return Ok(response);
        }
       
    }
}
