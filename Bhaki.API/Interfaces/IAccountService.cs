using Dice.API.Data.Models;
using Dice.API.Data.Dto;
using System.Collections.Generic;
using System;

namespace Dice.API.Interfaces
{
    public interface IAccountService
    {
        AccountInfo GetAccountInformation(Guid userID);
        bool DebitFunds(Guid userID, double price);
        bool CreditFunds(Guid? userID, double price);

    }
}
