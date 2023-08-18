using Bhaki.API.Data.Models;

using System;

namespace Bhaki.API.Data.Dto
{
    public class BranchReportResponse
    {
        public Branch Branch { get; set; }
        public int? TotalRegistrations { get; set; }
        public DateTime? LastUpdate { get; set; }

    }
}
