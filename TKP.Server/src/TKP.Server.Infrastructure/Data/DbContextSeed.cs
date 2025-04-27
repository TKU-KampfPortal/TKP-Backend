using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using TKP.Server.Application.HelperServices.Cookie;
using TKP.Server.Application.Repositories.Interface;
using TKP.Server.Domain.Entites;
using TKP.Server.Domain.Enums;

namespace TKP.Server.Infrastructure.Data
{
    public static class DbContextSeed
    {
        public static async Task SeedDatabaseAsync(IConfiguration configuration
            , IUnitOfWork unitOfWork
            , UserManager<ApplicationUser> userManager
            , RoleManager<ApplicationRole> roleManager
            , IRolePermissionRepository rolePermissionRepository
            , IPermissionService permissionHelperService)
        {
            await unitOfWork.ExecuteAsync(async () =>
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

                // Seed Permission Repository
                var adminRole = await roleManager.FindByNameAsync(BaseRoleEnum.Admin.ToString());

                var permissions = permissionHelperService.GetAllPermissions();

                var rolePermissions = permissions.Select(item => new RolePermission()
                {
                    RoleId = adminRole.Id,
                    Permission = item.Key
                }).ToList();

                var existedPermission = await rolePermissionRepository
                    .GetRolePermissionsByRoleIdAsync(adminRole.Id, cancellationToken: default);


                await rolePermissionRepository.DeleteRangeAsync(existedPermission);
                await rolePermissionRepository.AddRangeAsync(rolePermissions);
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
            });
        }
    }
}
