using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Bhaki.API.Data.Services;
using Bhaki.API.Data.ViewModels;
using Bhaki.API.Data.ViewModels.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bhaki.API.Interfaces;
using Bhaki.API.Data.Dto;

namespace Bhaki.API.Controllers
{
    [Authorize(Roles = UserRoles.Admin + "," + UserRoles.Clerk)]
    [Route("api/[controller]")]
    [ApiController]
    public class RegistrationController : ControllerBase
    {
        private readonly IRegistrationService _registrationService;
        public RegistrationController(IRegistrationService registrationService)
        {
            _registrationService = registrationService;
        }

        [HttpPost("add-registration")]
        public IActionResult CreateRegistration([FromBody] RegistrationRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Please, provide all required fields");
            }

            var response = _registrationService.Register(request);
            return Ok(response);
        }
        [Authorize(Roles = UserRoles.Admin)]
        [HttpGet("get-all-registrations")]
        public IActionResult GetRegistrations()
        {
            var response =  _registrationService.GetRegistration();
            return Ok(response);
        }

        //[Authorize(Roles = UserRoles.Admin)]
        //[HttpGet("get-all-registrations-by-id/{id}")]
        //public IActionResult GetRegistrationsById(Guid id)
        //{
        //    var response = _registrationService.GetRegistrationbyId(id);
        //    return Ok(response);
        //}

        [HttpGet("get-all-registrations-by-branch-id/{branchId}")]
        public IActionResult GetRegistrationsByBranchId(Guid branchId)
        {
            var response = _registrationService.GetRegistration(branchId);
            return Ok(response);
        }

        [HttpGet("get-all-registrations-by-date-branchId/{startdate}/{enddate}/{branchId}")]
        public IActionResult GetRegistrationsByDateAndBranchId(DateTime startdate, DateTime enddate, Guid branchId)
        {
            var response = _registrationService.GetRegistration(startdate,enddate,branchId);
            return Ok(response);
        }

        [HttpGet("get-all-registrations-by-date/{startdate}/{enddate}")]
        public IActionResult GetRegistrationsByDateRange(DateTime startdate, DateTime enddate)
        {
            var response = _registrationService.GetRegistration(startdate, enddate);
            return Ok(response);
        }

        [HttpGet("get-dashboard")]
        public IActionResult GetDashboard()
        {
            var response = _registrationService.GetDashboard();
            return Ok(response);
        }
        [HttpGet("get-registration-details/{registrationNumber}")]
        public IActionResult GetRegistrationDetails(int registrationNumber)
        {
            var response = _registrationService.GetRegistrationDetails(registrationNumber);
            return Ok(response);
        }
    }
}
