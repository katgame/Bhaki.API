using Dice.API.Data;
using Dice.API.Data.Models;
using Dice.API.Interfaces;
using System;

namespace Dice.API.Data.Services
{
    public class ProfileService : IProfileService
    {
        private AppDbContext _context;

        public ProfileService(AppDbContext context)
        {
            _context = context;
        }

        public bool CreateProfile(Guid userID)
        {
            try
            {
                var request = new AccountInfo { AccountBalance = 0.0, Id = Guid.NewGuid(), UserId = userID };
                _context.AccountInfo.Add(request);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
                throw;
            }
        }
    }
}
