using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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

            await DbContextSeed.SeedDatabaseAsync(configuration, userManager, roleManager);
        }
    }
}
