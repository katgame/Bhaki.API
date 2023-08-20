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


        public bool CreateCourse(Course request)
        {
            try
            {
              
                request.Id = Guid.NewGuid();
                request.CreatedOn = DateTime.Now;
                _dbContext.Course.Add(request);
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

        public List<Course> GetAllCourses(Guid branchId)
        {
            return _dbContext.Course.Where(x => x.BranchId == branchId).OrderBy(y => y.Name).ToList();
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
