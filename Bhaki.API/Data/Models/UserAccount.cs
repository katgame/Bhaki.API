using Bhaki.API.Data.Models;
using Bhaki.API.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bhaki.API.Data.Models
{
    public class UserAccount
    {
        public Guid Id { get; set; }    
        public Guid UserId { get; set; }
        public Guid CourseId { get; set; }
        public double OutstandingAmount { get; set; }
        public double PaidAmount { get; set; }
        public AccountStatus Status { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
