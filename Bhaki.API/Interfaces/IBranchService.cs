using Bhaki.API.Data.Dto;
using Bhaki.API.Data.Models;
using System;
using System.Collections.Generic;

namespace Bhaki.API.Interfaces
{
    public interface IBranchService
    {
        public bool CreateBranch(BranchRequest request);
        public Branch GetBranchInformation(Guid branchId);
        public List<Branch> GetAllBranches();
    }
}
