using Bhaki.API.Data.Dto;
using Bhaki.API.Data.Models;
using System;
using System.Collections.Generic;

namespace Bhaki.API.Interfaces
{
    public interface ICourseService
    {
        public bool CreateCourse(Course request);
        public Course GetCourseInformation(Guid courseId);
        public List<Course> GetAllCourses(Guid branchId );
        public List<Course> GetAllCourses();
        public bool UpdateCourse(Course course);

    }
}
