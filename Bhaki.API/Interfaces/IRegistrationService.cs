using Bhaki.API.Data.Models;
using Bhaki.API.Data.Dto;
using System.Collections.Generic;
using System;

namespace Bhaki.API.Interfaces
{
    public interface IRegistrationService
    {
        int Register(RegistrationRequest request);
        List<Registration> GetRegistration();
        List<Registration> GetRegistration(Guid branchId);
        List<Registration> GetRegistration(DateTime startDate, DateTime endDate, Guid branchId);
        List<Registration> GetRegistration(DateTime startDate, DateTime endDate);
    }
}
