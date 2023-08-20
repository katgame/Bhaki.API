using Bhaki.API.Data.Dto;
using Bhaki.API.Data.Models;
using Bhaki.API.Data.ViewModels;
using Bhaki.API.Enums;
using Bhaki.API.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bhaki.API.Data.Services
{
    public class RegistrationService : IRegistrationService
    {
        private readonly IBranchService _branchService;
        private readonly ICourseService _courseService;
        private AppDbContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;
        public RegistrationService(AppDbContext context, IBranchService branchService, UserManager<ApplicationUser> userManager, ICourseService courseService)
        {
            _branchService = branchService;
            _dbContext = context;
            _userManager = userManager;
            _courseService = courseService;
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
                    Surname = request.Surname
                },
                OutstandingAmount = request.Balance,
                CourseId = request.CourseId,
                PaidAmount = request.AmountPaid,
                Status = AccountStatus.Outstanding,
                CreatedOn = DateTime.UtcNow,
                BranchId = request.BranchId,
                RegistrationDate = DateTime.UtcNow,
                CreatedBy = request.CreatedBy
            };

        }

        public List<ReportRegistrationResponse> GetRegistration()
        {
            var response = new List<ReportRegistrationResponse>();
            var allBranches = _branchService.GetAllBranches();
            var allCourses = _courseService.GetAllCourses();
            var data = _dbContext.Registration.Include(a=> a.student).OrderBy(c => c.RegistrationNumber).ToList();
            var allUsers = _userManager.Users;
            foreach (var item in data)
            {
                response.Add(new ReportRegistrationResponse
                {
                    RegistrationDate = item.RegistrationDate,
                    Branch = allBranches.SingleOrDefault(x => x.Id == item.BranchId),
                    CreateBy = new UserInfo
                    {
                        Id = item.CreatedBy.ToString(),
                        Name = allUsers.SingleOrDefault(x => x.Id == item.CreatedBy.ToString())!.UserName,
                        Email = allUsers.SingleOrDefault(x => x.Id == item.CreatedBy.ToString())!.Email,
                        Role = null,
                        BranchId = item.BranchId,
                    },
                    RegistrationNumber = item.RegistrationNumber.ToString(),
                    StudentName = item.student.Name,
                    Course = allCourses?.SingleOrDefault(x => x.Id == item.CourseId)!.Name,
                }); ;
            }
            return response;
        }

        public DashBoardResponse GeLastWeeklyStats()
        {
            var lastWeekStart = GetPreviousWeek();
            var currentWeekStart = GetCurrentWeek();
            var endOfCurrentWeek = currentWeekStart.AddDays(7).AddTicks(-1);

            var lastweekStats = new List<int>();
            var currentweekStats = new List<int>();
            var lastWeekTotal = 0.0;

            double currentWeekCount = _dbContext.Registration
                .Where(x => x.RegistrationDate >= currentWeekStart).Count(x => x.RegistrationDate <= endOfCurrentWeek);
            for (int i = 0; i < 7; i++)
            {
                if (i > 0)
                {
                    lastWeekStart = lastWeekStart.AddDays(1);
                    lastweekStats.Add(_dbContext.Registration
                        .Where(x => x.RegistrationDate >= new DateTime(lastWeekStart.Year, lastWeekStart.Month, lastWeekStart.Day, 0, 0, 0, 999)).Count(x => x.RegistrationDate <= new DateTime(lastWeekStart.Year, lastWeekStart.Month, lastWeekStart.Day, 23, 59, 59, 999)));
                    lastWeekTotal = lastWeekTotal + lastweekStats[i];
                }
                else
                {
                    lastweekStats.Add(_dbContext.Registration
                        .Where(x => x.RegistrationDate >= new DateTime(lastWeekStart.Year, lastWeekStart.Month, lastWeekStart.Day, 0, 0, 0, 999)).Count(x => x.RegistrationDate <= new DateTime(lastWeekStart.Year, lastWeekStart.Month, lastWeekStart.Day, 23, 59, 59, 999)));
                    lastWeekTotal = lastWeekTotal + lastweekStats[i];
                }
             
            }

            for (int i = 0; i < 7; i++)
            {
                if (i > 0)
                {
                    currentWeekStart = currentWeekStart.AddDays(1);
                    currentweekStats.Add(_dbContext.Registration
                        .Where(x => x.RegistrationDate >= new DateTime(currentWeekStart.Year, currentWeekStart.Month, currentWeekStart.Day, 0, 0, 0, 999)).Count(x => x.RegistrationDate <= new DateTime(currentWeekStart.Year, currentWeekStart.Month, currentWeekStart.Day, 23, 59, 59, 999)));
                   // lastWeekTotal = currentWeekStart + currentweekStats[i];
                }
                else
                {
                    currentweekStats.Add(_dbContext.Registration
                        .Where(x => x.RegistrationDate >= new DateTime(currentWeekStart.Year, currentWeekStart.Month, currentWeekStart.Day, 0, 0, 0, 999)).Count(x => x.RegistrationDate <= new DateTime(currentWeekStart.Year, currentWeekStart.Month, currentWeekStart.Day, 23, 59, 59, 999)));
                  //  lastWeekTotal = lastWeekTotal + currentweekStats[i];
                }

            }

            var Growth = Math.Round(((currentWeekCount - lastWeekTotal) / lastWeekTotal) * 100, 2) ;
            double price;
            if (Double.TryParse(Growth.ToString(), out price))
            {
               // Console.WriteLine(validate);
            }
            else
            {
                Growth = 0;
            }
            return new DashBoardResponse
            {
                Growth = Growth,
                LastWeekStats = lastweekStats,
                CurrentWeekStats = currentweekStats,
                LastWeekTotal = lastWeekTotal,
                increase = (currentWeekCount > lastWeekTotal)

            };
        }

        public DateTime GetPreviousWeek()
        {
            DayOfWeek weekStart = DayOfWeek.Monday; // or Sunday, or whenever
            DateTime startingDate = DateTime.Today;

            while (startingDate.DayOfWeek != weekStart)
                startingDate = startingDate.AddDays(-1);

            DateTime previousWeekStart = startingDate.AddDays(-7);
            DateTime previousWeekEnd = startingDate.AddDays(-1);
            return previousWeekStart;
        }

        public DateTime GetCurrentWeek()
        {
            DayOfWeek weekStart = DayOfWeek.Monday; // or Sunday, or whenever
            DateTime startingDate = DateTime.Today;

            while (startingDate.DayOfWeek != weekStart)
                startingDate = startingDate.AddDays(-1);

            return startingDate;
        }
        public List<ReportRegistrationResponse> GetRegistration(Guid branchId)
        {
            var response = new List<ReportRegistrationResponse>();
            var branch = _branchService.GetBranchInformation(branchId);
            var allCourses = _courseService.GetAllCourses();
            var data = _dbContext.Registration.Include(a => a.student).Where(x => x.BranchId == branchId).ToList();
            var allUsers = _userManager.Users;
            foreach (var item in data)
            {
                response.Add(new ReportRegistrationResponse
                {
                    RegistrationDate = item.RegistrationDate,
                    Branch = branch,
                    CreateBy = new UserInfo
                    {
                        Id = item.CreatedBy.ToString(),
                        Name = allUsers.SingleOrDefault(x => x.Id == item.CreatedBy.ToString()).UserName,
                        Email = allUsers.SingleOrDefault(x => x.Id == item.CreatedBy.ToString()).Email,
                        Role = null,
                        BranchId = item.BranchId,
                    },
                    RegistrationNumber = item.RegistrationNumber.ToString(),
                    StudentName = item.student.Name,
                    Course = allCourses.SingleOrDefault(x => x.Id == item.CourseId).Name,
                }); ;
            }
            return response;
           
        }

        public List<ReportRegistrationResponse> GetRegistration(DateTime startDate, DateTime endDate, Guid branchId)
        {
            var response = new List<ReportRegistrationResponse>();
            var branch = _branchService.GetBranchInformation(branchId);
            var allCourses = _courseService.GetAllCourses();
            var data = _dbContext.Registration.Include(a => a.student).Where(x => x.BranchId == branchId).Where(x => x.RegistrationDate >= startDate).Where(x => x.RegistrationDate <= endDate).ToList();
            var allUsers = _userManager.Users;
            foreach (var item in data)
            {
                response.Add(new ReportRegistrationResponse
                {
                    RegistrationDate = item.RegistrationDate,
                    Branch = branch,
                    CreateBy = new UserInfo
                    {
                        Id = item.CreatedBy.ToString(),
                        Name = allUsers.SingleOrDefault(x => x.Id == item.CreatedBy.ToString()).UserName,
                        Email = allUsers.SingleOrDefault(x => x.Id == item.CreatedBy.ToString()).Email,
                        Role = null,
                        BranchId = item.BranchId,
                    },
                    RegistrationNumber = item.RegistrationNumber.ToString(),
                    StudentName = item.student.Name,
                    Course = allCourses.SingleOrDefault(x => x.Id == item.CourseId).Name,
                }); ;
            }
            return response;
        }

        public List<ReportRegistrationResponse> GetRegistration(DateTime startDate, DateTime endDate)
        {
            var response = new List<ReportRegistrationResponse>();
            var allBranches = _branchService.GetAllBranches();
            var allCourses = _courseService.GetAllCourses();
            var data = _dbContext.Registration.Include(a => a.student).Where(x => x.RegistrationDate >= startDate).Where(x => x.RegistrationDate <= endDate).ToList();
            var allUsers = _userManager.Users;
            foreach (var item in data)
            {
                response.Add(new ReportRegistrationResponse
                {
                    RegistrationDate = item.RegistrationDate,
                    Branch = allBranches.SingleOrDefault(x => x.Id == item.BranchId),
                    CreateBy = new UserInfo
                    {
                        Id = item.CreatedBy.ToString(),
                        Name = allUsers.SingleOrDefault(x => x.Id == item.CreatedBy.ToString()).UserName,
                        Email = allUsers.SingleOrDefault(x => x.Id == item.CreatedBy.ToString()).Email,
                        Role = null,
                        BranchId = item.BranchId,
                    },
                    RegistrationNumber = item.RegistrationNumber.ToString(),
                    StudentName = item.student.Name,
                    Course = allCourses.SingleOrDefault(x => x.Id == item.CourseId).Name,
                }); ;
            }
            return response;
           // return _dbContext.Registration.Where(x => x.RegistrationDate >= startDate).Where(x => x.RegistrationDate <= endDate).ToList();
        }

        public DashBoardResponse GetDashboard()
        {
            return GeLastWeeklyStats();
        }
    }
}
