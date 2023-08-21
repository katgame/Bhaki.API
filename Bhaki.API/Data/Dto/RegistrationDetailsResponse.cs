using Bhaki.API.Data.Models;
using System;

namespace Bhaki.API.Data.Dto
{
    public class RegistrationDetailsResponse
    {
        public Registration Registration { get; set; }
        public UserInfo CreateBy { get; set; }
        public Branch Branch { get; set; }
        public Course Course { get; set; }
    }
}
