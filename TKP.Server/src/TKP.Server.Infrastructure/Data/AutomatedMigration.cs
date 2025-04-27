using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TKP.Server.Application.HelperServices.Interface;
using TKP.Server.Application.Repositories.Interface;
using TKP.Server.Domain.Entites;

namespace TKP.Server.Infrastructure.Data
{
    public static class AutomatedMigration
    {
        public static async Task MigrateAsync(IServiceProvider services, IConfiguration configuration)
        {
            var context = services.GetRequiredService<ApplicationDbContext>();

            var database = context.Database;

            var pendingMigrations = await database.GetPendingMigrationsAsync();

            if (pendingMigrations.Any())
            {
                await database.MigrateAsync();
            }

            var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = services.GetRequiredService<RoleManager<ApplicationRole>>();
            var unitOfWork = services.GetRequiredService<IUnitOfWork>();
            var rolePermissionRepository = services.GetRequiredService<IRolePermissionRepository>();
            var permissionHelper = services.GetRequiredService<IPermissionService>();

            await DbContextSeed.SeedDatabaseAsync(configuration, unitOfWork, userManager, roleManager
                , rolePermissionRepository, permissionHelper);
        }
    }
}
