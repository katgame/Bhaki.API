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
    public class RegistrationService : IRegistrationService
    {
        private AppDbContext _dbContext;
        public RegistrationService(AppDbContext context)
        {
            _dbContext = context;
        }


        public int Register(RegistrationRequest request)
        {
            var registrationRequest = createRegistationModel(request);
            var RegistrationNumber = GetRegistrationNumber();
            registrationRequest.RegistrationNumber = RegistrationNumber;
            registrationRequest.student.Address.Id = Guid.NewGuid();
            _dbContext.Registration.Add(registrationRequest);
            _dbContext.SaveChanges();
            return registrationRequest.RegistrationNumber;
        }

        internal int GetRegistrationNumber()
        {
            var registrationNumber = _dbContext.Registration.OrderBy(x => x.CreatedOn).LastOrDefault()?.RegistrationNumber;
            if (registrationNumber == null) { return 10000; }
            else
            {
                var currentNumber = (registrationNumber + 1);
                return (int)currentNumber;
            }
        }
        public void Delete(RegistrationRequest request) { }

        internal Registration createRegistationModel(RegistrationRequest request)
        {
            var userID = Guid.NewGuid();
            return new Registration
            {
                Id = Guid.NewGuid(),
                student = new Student
                {
                    Id = userID,
                    CellPhone = request.Cellphone,
                    Address = request.Address,
                    CreatedOn = DateTime.UtcNow,
                    EmailAddress = request.EmailAddress,
                    IdDocument = request.IdDocument,
                    IdNumber = request.IdNumber,
                    Name = request.Name,
                    PassportNumber = string.Empty,
                    Surname = request.Surname,
                    UserAccount = new UserAccount
                    {
                        Id = Guid.NewGuid(),
                        UserId = userID,
                        OutstandingAmount = request.Balance,
                        CourseId = Guid.NewGuid(),
                        CreatedOn = DateTime.UtcNow,
                        PaidAmount = request.AmountPaid,
                        Status = AccountStatus.Outstanding
                    }
                },
                CreatedOn = DateTime.UtcNow,
                RegistrationDate = DateTime.UtcNow
            };

        }

        public List<Registration> GetRegistration()
        {
            return _dbContext.Registration.ToList();
        }

        public List<Registration> GetRegistration(Guid branchId)
        {
            return _dbContext.Registration.Where(x => x.BranchId == branchId) .ToList();
        }

        public List<Registration> GetRegistration(DateTime startDate, DateTime endDate, Guid branchId)
        {
            return _dbContext.Registration.Where(x => x.BranchId == branchId).Where(x => x.RegistrationDate >= startDate).Where(x => x.RegistrationDate <= endDate).ToList();
        }

        public List<Registration> GetRegistration(DateTime startDate, DateTime endDate)
        {
            return _dbContext.Registration.Where(x => x.RegistrationDate >= startDate).Where(x => x.RegistrationDate <= endDate).ToList();
        }
    }
}
