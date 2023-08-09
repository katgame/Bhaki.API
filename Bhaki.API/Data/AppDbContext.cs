﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Bhaki.API.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bhaki.API.Data
{
    public class AppDbContext:IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options):base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<Book_Author>()
            //    .HasOne(b => b.Book)
            //    .WithMany(ba => ba.Book_Authors)
            //    .HasForeignKey(bi => bi.BookId);

            //modelBuilder.Entity<Book_Author>()
            //  .HasOne(b => b.Author)
            //  .WithMany(ba => ba.Book_Authors)
            //  .HasForeignKey(bi => bi.AuthorId);

            modelBuilder.Entity<Log>().HasKey(n => n.Id);

            base.OnModelCreating(modelBuilder);
        }



        public DbSet<Log> Logs { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        public DbSet<Course> Course { get; set; }
        public DbSet<Registration> Registration { get; set; }
        public DbSet<CoursePrice> CoursePrice { get; set; }
        public DbSet<UserAccount> UserAccount { get; set; }

        public DbSet<Branch> Branch { get; set; }
    }
}
