using Bhaki.API.Data.Models;
using Bhaki.API.Data.Dto;
using System.Collections.Generic;
using System;

namespace Bhaki.API.Interfaces
{
    public interface IRegistrationService
    {
        int Register(RegistrationRequest request);
        List<ReportRegistrationResponse> GetRegistration();
        List<ReportRegistrationResponse> GetRegistration(Guid branchId);
        List<ReportRegistrationResponse> GetRegistration(DateTime startDate, DateTime endDate, Guid branchId);
        List<ReportRegistrationResponse> GetRegistration(DateTime startDate, DateTime endDate);
        DashBoardResponse GetDashboard();
        RegistrationDetailsResponse GetRegistrationDetails(int id);
    }
}
