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
using Microsoft.Extensions.Logging;

namespace Bhaki.API.Controllers
{
    [Authorize(Roles = UserRoles.Admin)]
    [Route("api/[controller]")]
    [ApiController]
    public class BranchController : ControllerBase
    {
        private readonly IBranchService _branchService;
        private readonly ILogger<BranchController> _logger;
        public BranchController(IBranchService branchService, ILogger<BranchController> logger)
        {
            _logger = logger;
            _branchService = branchService;
        }

        [HttpPost("add-branch")]
        public IActionResult CreateBranch([FromBody] BranchRequest request)
        {
            try
            {
                _branchService.CreateBranch(request);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError("Branch could not be added", ex);
                throw;
            }
        
        }

        [HttpGet("get-branch-information/{branchId}")]
        public IActionResult GetBranchInformation(Guid branchId)
        {
            var response = _branchService.GetBranchInformation(branchId);
            return Ok(response);
        }

        [HttpGet("get-all-branches")]
        public IActionResult GetAllBranches()
        {
            var response = _branchService.GetAllBranches();
            return Ok(response);
        }

      
    }
}
