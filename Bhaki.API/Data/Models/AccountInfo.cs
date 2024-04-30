using Microsoft.EntityFrameworkCore;
using System;

namespace Dice.API.Data.Models
{
    public class AccountInfo
    {
        public Guid Id { get; set; }
        public virtual Guid UserId { get; set; }
        public double AccountBalance { get; set; } 
        public double Credit { get; set; }
        public double Debit { get; set; }
        public bool AdminTax { get; set; }
        public DateTime TransactionDate { get; set; }

    }
}

