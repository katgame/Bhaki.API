using Dice.API.Data.Models;
using Dice.API.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

//App TAX on withdrawl


namespace Dice.API.Data.Services
{
    
    public class AccountService : IAccountService
    {
        const int appTax = 10;
        private AppDbContext _context;
        private readonly IAdminstratorService _adminstratorService;
        public AccountService(AppDbContext context, IAdminstratorService adminstratorService)
        {
            _context = context;
            _adminstratorService = adminstratorService; 
        }

        public bool CreditFunds(Guid? userID, double creditAmount)
        {
            try
            {
                var accountDetails = _context.AccountInfo.Where(x => x.UserId == userID).ToList();
                var balance = accountDetails.Sum(x => x.Credit - x.Debit);
                var newCredit = new AccountInfo
                {
                    AccountBalance = balance,
                    Credit = creditAmount,
                    Debit = 0.0,
                    UserId = userID.Value,
                    Id = Guid.NewGuid(),
                    TransactionDate = System.DateTime.Now,
                };
                _context.AccountInfo.Add(newCredit);
                _context.SaveChanges();
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }


        public AccountInfo GetAccountInformation(Guid userID) 
            => _context.AccountInfo.SingleOrDefault(x => x.UserId == userID);

        internal double GetCommission(double balance) =>  balance / appTax;

        public bool DebitFunds(Guid userID, double DebitAmount)
        {
            try
            {
                var accountDetails = _context.AccountInfo.Where(x => x.UserId == userID).ToList();
                var balance = accountDetails.Sum(x => x.Credit - x.Debit);
                var commission = GetCommission(balance);

                var newDebit = new AccountInfo {
                    AccountBalance = balance - DebitAmount,
                    Credit = 0.0,
                    Debit = DebitAmount,
                    AdminTax = false,
                    UserId = userID,
                    Id = Guid.NewGuid(),
                    TransactionDate = System.DateTime.Now,
                };

                double newBalance;
                if (DebitAmount - commission < 0)
                    newBalance = 0;
                else
                    newBalance = DebitAmount - commission;

                var commisionDebit = new AccountInfo
                {
                    AccountBalance = (balance - DebitAmount) - commission,
                    Credit = 0.0,
                    Debit = commission,
                    AdminTax = true,
                    UserId = userID,
                    Id = Guid.NewGuid(),
                    TransactionDate = System.DateTime.Now,
                };

                _adminstratorService.CreditFunds(null, commission);

                _context.AccountInfo.Add(newDebit);
                _context.AccountInfo.Add(commisionDebit);
                _context.SaveChanges();
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

    }
}
