using Bhaki.API.Data.Models;
using System;

namespace Bhaki.API.Data.Dto
{
    public class ReportRegistrationResponse
    {
        public string RegistrationNumber { get; set; }
        public UserInfo CreateBy { get; set; }
        public Branch Branch { get; set; }
        public DateTime RegistrationDate { get; set; }
        public string StudentName { get; set; }
        public Student Student { get; set; }
        public string Course { get; set; }
    }
}
