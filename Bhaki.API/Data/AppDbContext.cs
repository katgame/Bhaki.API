using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Dice.API.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dice.API.Data
{
    public class AppDbContext:IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options):base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Log>().HasKey(n => n.Id);

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Log> Logs { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<AccountInfo> AccountInfo { get; set; }
        public DbSet<School> School { get; set; }
        public DbSet<AdminTransaction> AdminTransaction { get; set; }
        

    }
}
