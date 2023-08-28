using System.Collections.Generic;
using Bhaki.API.Data.Models;
using Microsoft.AspNetCore.Identity;

namespace Bhaki.API.Data.Dto
{
    public class UserRequest
    {
        public ApplicationUser User { get; set; }
        public Branch Branch { get; set; }
        public List<string> Roles { get; set; }
    }
}
