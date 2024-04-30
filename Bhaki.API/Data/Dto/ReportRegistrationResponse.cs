using Dice.API.Data.Models;
using System;

namespace Dice.API.Data.Dto
{
    public class ReportprofileResponse
    {
        public string profileNumber { get; set; }
        public UserInfo CreateBy { get; set; }
        public DateTime profileDate { get; set; }
        public string StudentName { get; set; }
        public string Course { get; set; }
    }
}
