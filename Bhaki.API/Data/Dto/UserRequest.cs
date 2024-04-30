using System.Collections.Generic;
using Dice.API.Data.Models;
using Microsoft.AspNetCore.Identity;

namespace Dice.API.Data.Dto
{
    public class UserRequest
    {
        public ApplicationUser User { get; set; }
        public List<string> Roles { get; set; }
    }
}
