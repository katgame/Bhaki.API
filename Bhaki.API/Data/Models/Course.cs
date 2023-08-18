using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bhaki.API.Data.Models
{
    public class Course 
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string CourseDuration { get; set; }
        public DateTime CreatedOn { get; set; }
        public string AdditionalDescription { get; set; }
        public string Firearm { get; set; }
        public string Grade { get; set; }
        public double Price { get; set; }
      

    }
}
