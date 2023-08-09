using Bhaki.API.Data.Dto;
using Bhaki.API.Data.Models;
using Bhaki.API.Data.ViewModels;
using Bhaki.API.Enums;
using Bhaki.API.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bhaki.API.Data.Services
{
    public class BranchService : IBranchService
    {
        private AppDbContext _dbContext;
        public BranchService(AppDbContext context)
        {
            _dbContext = context;
        }


        public bool CreateBranch(BranchRequest request)
        {
            try
            {
                var newBranch = new Branch { CreatedOn = DateTime.Now, Description = request.Description, Id = Guid.NewGuid(), Name = request.Name };
                _dbContext.Branch.Add(newBranch);
                _dbContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                 throw new Exception($"The Branch cant not be added");
            }
        
        }


        public Branch GetBranchInformation(Guid branchId)
        {
            return _dbContext.Branch.SingleOrDefault(x => x.Id == branchId);
        }

        public List<Branch> GetAllBranches()
        {
            return _dbContext.Branch.ToList();
        }
    }
}
