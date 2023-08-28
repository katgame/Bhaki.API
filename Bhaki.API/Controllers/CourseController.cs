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
using Bhaki.API.Data.Models;

namespace Bhaki.API.Controllers
{
    [Authorize(Roles = UserRoles.Admin + "," + UserRoles.Clerk)]
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly ICourseService _CourseService;
        private readonly ILogger<CourseController> _logger;
        public CourseController(ICourseService CourseService, ILogger<CourseController> logger)
        {
            _logger = logger;
            _CourseService = CourseService;
        }
        [Authorize(Roles = UserRoles.Admin)]
        [HttpPost("add-course")]
        public IActionResult CreateCourse([FromBody] Course request)
        {
            try
            {
                _CourseService.CreateCourse(request);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError("Course could not be added", ex);
                throw;
            }

        }
        [Authorize(Roles = UserRoles.Admin)]
        [HttpGet("get-course-information/{courseId}")]
        public IActionResult GetCourseInformation(Guid CourseId)
        {
            var response = _CourseService.GetCourseInformation(CourseId);
            return Ok(response);
        }

        [HttpGet("get-all-courses/{branchId}")]
        public IActionResult GetAllCourses(Guid branchId)
        {
            var response = _CourseService.GetAllCourses(branchId);
            return Ok(response);
        }
        [Authorize(Roles = UserRoles.Admin)]
        [HttpPut("update-course")]
        public IActionResult UpdateCourse(Course course)
        {
            var response = _CourseService.UpdateCourse(course);
            return Ok(response);
        }


    }
}
