using System;
using System.ComponentModel.DataAnnotations;

namespace Bhaki.API.Data.Models
{
    public class UserBranch
    {
        [Key]
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid BranchId { get; set; }
    }
}
