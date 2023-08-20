using Bhaki.API.Data.Dto;
using Bhaki.API.Data.Models;
using Bhaki.API.Data.ViewModels;
using Bhaki.API.Enums;
using Bhaki.API.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bhaki.API.Data.Services
{
    public class CourseService : ICourseService
    {
        private AppDbContext _dbContext;
        public CourseService(AppDbContext context)
        {
            _dbContext = context;
        }


        public bool CreateCourse(CourseRequest request)
        {
            try
            {
                var newCourse = new Course
                {
                    CreatedOn = DateTime.Now, Description = request.Description, Id = Guid.NewGuid(), Name = request.Name, AdditionalDescription = request.AdditionalDescription, CourseDuration = request.CourseDuration,
                    Firearm = request.Firearm, Grade = request.Grade, Price = request.Price
                };
                _dbContext.Course.Add(newCourse);
                _dbContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                 throw new Exception($"The Course cant not be added");
            }
        
        }


        public Course GetCourseInformation(Guid courseId)
        {
            return _dbContext.Course.SingleOrDefault(x => x.Id == courseId);
        }

        public List<Course> GetAllCourses()
        {
            return _dbContext.Course.ToList();
        }

       

        public bool UpdateCourse(Course course)
        {
            try
            {
                _dbContext.Course.Update(course);
                _dbContext.SaveChanges();
                return true;
            }
            catch 
            {
                return false;
            }
        }

      
    }
}
