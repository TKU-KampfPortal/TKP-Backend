using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TKP.Server.Infrastructure.Data.Interceptor;

namespace TKP.Server.Infrastructure.Data
{
    public static class EntityFrameworkRegistration
    {
        public static WebApplicationBuilder AddEntityFrameworkCore(this WebApplicationBuilder builder)
        {
            var configuration = builder.Configuration;

            // Register the custom interceptor for handling logic before/after SaveChanges
            builder.Services.AddScoped<ContextSaveChangeInterceptor>();

            // Register ApplicationDbContext with PostgreSQL provider
            builder.Services.AddDbContext<ApplicationDbContext>((serviceProvider, options) =>
            {
                var connectionString = configuration.GetConnectionString("PostgreSql");

                options
                    .AddInterceptors(serviceProvider.GetRequiredService<ContextSaveChangeInterceptor>())
                    .UseNpgsql(connectionString, npgsqlOptions =>
                    {
                        // Set the assembly where EF Core should look for migrations
                        npgsqlOptions.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName);
                        // Enable retry on failure in case of transient database errors
                        npgsqlOptions.EnableRetryOnFailure();
                    });
            });

            // Register DbContext as a scoped service to be injected as an abstraction
            builder.Services.AddScoped<DbContext>(sp => sp.GetRequiredService<ApplicationDbContext>());

            return builder;
        }
    }


}
