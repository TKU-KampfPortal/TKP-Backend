using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using TKP.Server.Application.HelperServices;
using TKP.Server.Application.HelperServices.Cookie;
using TKP.Server.Application.Repositories.Interface;
using TKP.Server.Infrastructure.Authentication;
using TKP.Server.Infrastructure.Caching;
using TKP.Server.Infrastructure.Data;
using TKP.Server.Infrastructure.Identity;
using TKP.Server.Infrastructure.Logging;
using TKP.Server.Infrastructure.Mapper;
using TKP.Server.Infrastructure.Mediator;
using TKP.Server.Infrastructure.Repositories;
using TKP.Server.Infrastructure.Services;
using TKP.Server.Infrastructure.Swagger;
using TKP.Server.Infrastructure.Validations;

namespace TKP.Server.Infrastructure
{
    public static class DependencyInjection
    {
        public static WebApplicationBuilder AddInfrastructureDI(this WebApplicationBuilder builder)
        {
            // Swagger
            builder.AddApplicationSwagger();

            // EFCore
            builder.AddEntityFrameworkCore();

            // Identity
            builder.AddApplicationIdentity();

            // Authentication
            builder.AddAuthenticationServices();
            // Caching
            builder.AddApplicationCaching();

            //Logging
            builder.Host.AddHostSerilogConfiguration();

            // Validators
            builder.AddApplicationFluentValidation();

            // MediaR
            builder.AddApplicationMediator();

            // Add infrastructure Services
            builder.Services.AddInfrastructureServices();

            // Add Application Automapper
            builder.Services.AddApplicationMapper();

            // Add infrastructure Repositoires
            builder.Services.AddRepositoires();
            return builder;
        }

        internal static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        {
            // Add infrastructure services
            // Example
            // services.AddScoped<IExampleService,ExampleService>();
            services.AddScoped<IClaimService, ClaimService>()
                    .AddScoped<ITokenService, TokenService>()
                    .AddScoped<ICookieService, CookieService>()
                    .AddScoped<IDeviceInfoService, DeviceInfoService>();

            return services;
        }
        internal static IServiceCollection AddRepositoires(this IServiceCollection services)
        {
            // Add infrastructure repositories
            // Example
            // services.AddScoped<IExampleRepository,ExampleRepository>();
            services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>))
                    .AddScoped<ILoginHistoryRepository, LoginHistoryRepository>()
                    .AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}
