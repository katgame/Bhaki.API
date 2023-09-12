using Bhaki.API.Data.Models;
using Bhaki.API.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bhaki.API.Data.Models
{
    public class Registration
    {
        public Guid Id { get; set; }

        public int RegistrationNumber { get; set; }
        public Guid BranchId { get; set; }
        public Student student { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime RegistrationDate { get; set; }
        public Guid CourseId { get; set; }
        public double OutstandingAmount { get; set; }
        public double PaidAmount { get; set; }
        public AccountStatus Status { get; set; }
        public string RecieptReference { get; set; } 
    }
}
