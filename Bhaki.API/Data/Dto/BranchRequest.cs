using Bhaki.API.Data.Models;
using System;

namespace Bhaki.API.Data.Dto
{
    public class CourseRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string CourseDuration { get; set; }
        public string AdditionalDescription { get; set; }
        public string Firearm { get; set; }
        public string Grade { get; set; }
        public double Price { get; set; }

    }
}
