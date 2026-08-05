using E_Commerce.Domain.Contracts;
using E_Commerce.Domain.Entities.Identity;
using E_Commerce.Infrastructure.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Infrastructure.DataSeeding;

public class IdentityDataSeeder(
    StoreIdentityDbContext context,
    RoleManager<IdentityRole> roleManger,
    UserManager<ApplicationUser> userManager,
    ILogger<IdentityDataSeeder> logger
    ) : IDataSeeder
{
    public async Task SeedDataAsync(CancellationToken ct = default)
    {
        try
        {
            var pendingMigrations = await context.Database.GetPendingMigrationsAsync();
            if (pendingMigrations.Any())
            {
               await context.Database.MigrateAsync(ct);
            }

            //Roles
            if (!await roleManger.Roles.AnyAsync())
            {
               await roleManger.CreateAsync(new IdentityRole("Admin"));
               await roleManger.CreateAsync(new IdentityRole("SuperAdmin"));
            }

            //Users
            if (!await userManager.Users.AnyAsync())
            {
                var user = new ApplicationUser()
                {
                    DisplayName = "Mohamed",
                    UserName = "Mohamed",
                    Email = "Mohamed@system.com",
                    PhoneNumber = "01220083274"
                };

                var result = await userManager.CreateAsync(user, "P@ssW0rd");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "SuperAdmin");
                }
                else
                {
                    var errors = string.Join(", \n", result.Errors.Select(e => e.Description));

                    logger.LogWarning($"can not seed deafault admin{errors}");
                }
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message);
        }
    }
}
