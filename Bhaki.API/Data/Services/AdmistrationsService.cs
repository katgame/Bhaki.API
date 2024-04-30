using Dice.API.Data.Models;
using Dice.API.Interfaces;
using System;
using System.Linq;

namespace Dice.API.Data.Services
{
    public class AdmistrationsService : IAdminstratorService
    {
        private AppDbContext _context;
        public AdmistrationsService(AppDbContext context)
        {
            _context = context;
        }

        public bool CreditFunds(Guid? userID, double creditAmount)
        {
            try
            {
                var newCredit = new AdminTransaction
                {
                    AccountBalance = 0.0,
                    Credit = creditAmount,
                    Debit = 0.0,
                    Id = Guid.NewGuid(),
                    TransactionDate = System.DateTime.Now,
                };
                _context.AdminTransaction.Add(newCredit);
                _context.SaveChanges();
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }


        public AccountInfo GetAccountInformation(Guid userID)
            => _context.AdminTransaction.SingleOrDefault(x => x.UserId == userID);

        public bool DebitFunds(Guid userID, double DebitAmount)
        {
            //this account should never be debited
            return false;
        }


    }
}
