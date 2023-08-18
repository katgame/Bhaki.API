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

        public List<BranchReportResponse> GetAllBranchesForReport()
        {
            var response = new List<BranchReportResponse>();
            var branches = _dbContext.Branch.ToList();
            foreach (var item in branches)
            {
                response.Add(new BranchReportResponse { Branch = item,
                    TotalRegistrations = _dbContext.Registration.Count(x => x.BranchId == item.Id) ,
                    LastUpdate = getLastRegistration(item.Id)
                });
            }
            return response;
        }

        private DateTime? getLastRegistration(Guid branchid)
        {
            var regs = _dbContext.Registration.Any(x => x.BranchId == branchid);
            if (regs)
            {
               return _dbContext.Registration!.Where(x => x.BranchId == branchid)!
                    .OrderByDescending(x => x.RegistrationDate)!
                    .FirstOrDefault()!.RegistrationDate;
            }
            else
            {
                return null;
            }
         
        }
        public List<Branch> GetAllBranches()
        {
   
            return _dbContext.Branch.ToList();
        }



        public bool UpdateBranch(Branch branch)
        {
            try
            {
                _dbContext.Branch.Update(branch);
                _dbContext.SaveChanges();
                return true;
            }
            catch 
            {
                return false;
            }
        }

        public bool SaveUserBranch(UserBranch Userbranch)
        {
            try
            {
                _dbContext.UserBranch.Update(Userbranch);
                _dbContext.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
