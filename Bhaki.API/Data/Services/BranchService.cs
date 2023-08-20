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
using Microsoft.EntityFrameworkCore.Storage;

namespace Bhaki.API.Data.Services
{
    public class BranchService : IBranchService
    {
        private AppDbContext _dbContext;
        private ICourseService _courseService;
        public BranchService(AppDbContext context, ICourseService courseService)
        {
            _dbContext = context;
            _courseService = courseService;
        }


        public bool CreateBranch(BranchRequest request)
        {

            using (var dbContext = _dbContext)
            {
                using (var transaction = dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        var newBranch = new Branch { CreatedOn = DateTime.Now, Description = request.Description, Id = Guid.NewGuid(), Name = request.Name };
                        _dbContext.Branch.Add(newBranch);
                        _dbContext.SaveChanges();
                        transaction.Commit();
                        return true;
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        return false;
                    }
                }
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
                response.Add(new BranchReportResponse
                {
                    Branch = item,
                    TotalRegistrations = _dbContext.Registration.Count(x => x.BranchId == item.Id),
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
            using (var dbContext = _dbContext)
            {
                using (var transaction = dbContext.Database.BeginTransaction())
                {
                    try
                    {
                      
                        _dbContext.Branch.Update(branch);
                        _dbContext.SaveChanges();
                      
                        transaction.Commit();
                        return true;
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        return false;
                    }
                }
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
