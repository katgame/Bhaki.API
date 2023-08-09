using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Bhaki.API.Data.Models;
using Bhaki.API.Data.ViewModels.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bhaki.API.Data
{
    public class AppDbInitializer
    {
        public static void Seed(IApplicationBuilder applicationBuilder)
        {
          
        }

        public static async Task SeedRoles(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                if (!await roleManager.RoleExistsAsync(UserRoles.Admin))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));

                if (!await roleManager.RoleExistsAsync(UserRoles.SuperUser))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.SuperUser));

                if (!await roleManager.RoleExistsAsync(UserRoles.Assessor))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.Assessor));

                if (!await roleManager.RoleExistsAsync(UserRoles.Moderator))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.Moderator));
                if (!await roleManager.RoleExistsAsync(UserRoles.Finance))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.Finance));
                if (!await roleManager.RoleExistsAsync(UserRoles.Manager))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.Manager));
                if (!await roleManager.RoleExistsAsync(UserRoles.Manager))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.Manager));
                if (!await roleManager.RoleExistsAsync(UserRoles.Clerk))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.Clerk));
            }
        }
    }
}
