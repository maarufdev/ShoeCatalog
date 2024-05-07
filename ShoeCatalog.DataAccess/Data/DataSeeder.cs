using Microsoft.AspNetCore.Identity;
using ShoeCatalog.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoeCatalog.Domain.Data
{
    public static class DataSeeder
    {
        public static async Task SeedData(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            var roles = new List<string>
            {
                AppUserRoles.Admin.ToString(),
                AppUserRoles.User.ToString()
            };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            var email = "maarufb@gmail.com";
            var password = "@Password123";

            var defaultUser = new AppUser
            {
                FirstName = "Ma-aruf",
                MiddleName = "Mohammad",
                LastName = "Burad",
                DateOfBirth = DateTime.UtcNow,
                CreatedBy = "Test",
                CreatedOn = DateTime.UtcNow,
                IsActive = true,
                Email = email,
                UserName = email,
            };

            if(await userManager.FindByEmailAsync(email) == null)
            {
                await userManager.CreateAsync(defaultUser, password);

                await userManager.AddToRoleAsync(defaultUser, AppUserRoles.Admin.ToString());
            }
            else
            {
                await userManager.UpdateAsync(defaultUser);
            }
        }
    }
}
