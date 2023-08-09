using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bhaki.API.Data.Models
{
    public class CoursePrice
    {
        public Guid Id { get; set; }
        public Guid CourseId { get; set; }
        public double Price { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
