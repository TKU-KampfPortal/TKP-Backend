using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using TKP.Server.Domain.Entites;
using TKP.Server.Domain.Enums;

namespace TKP.Server.Infrastructure.Data
{
    public static class DbContextSeed
    {
        public static async Task SeedDatabaseAsync(IConfiguration configuration, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
        {
            // Seed Roles
            if (!roleManager.Roles.Any())
            {
                foreach (var role in Enum.GetNames(typeof(BaseRoleEnum)))
                {
                    var applicationRole = new ApplicationRole
                    {
                        Name = role
                    };

                    await roleManager.CreateAsync(applicationRole);
                }
            }

            // Seed Users
            if (!userManager.Users.Any())
            {
                var adminUser = new ApplicationUser
                {
                    Id = Guid.NewGuid(),
                    UserName = "admin",
                    DisplayName = "Administrator",
                    Email = "admin@gmail.com",
                    EmailConfirmed = true,
                    AvatarUrl = "",
                };

                var commonUser = new ApplicationUser
                {
                    Id = Guid.NewGuid(),
                    UserName = "commonUser",
                    DisplayName = "Common User",
                    Email = "commonUser@gmail.com",
                    EmailConfirmed = true,
                    AvatarUrl = "",
                };

                var adminPassword = configuration["SeedUser:AdminPassword"] ?? "Password123!";
                var commonUserPassword = configuration["SeedUser:CommonUserPassword"] ?? "Password123!";

                await userManager.CreateAsync(adminUser, adminPassword);
                await userManager.AddToRoleAsync(adminUser, BaseRoleEnum.Admin.ToString());

                await userManager.CreateAsync(commonUser, commonUserPassword);
                await userManager.AddToRoleAsync(commonUser, BaseRoleEnum.CommonUser.ToString());
            }
        }

    }
}
