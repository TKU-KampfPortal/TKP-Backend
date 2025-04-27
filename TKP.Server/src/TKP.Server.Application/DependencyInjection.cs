using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TKP.Server.Application.HelperServices;
using TKP.Server.Application.HelperServices.Cookie;
using TKP.Server.Application.OptionSetting;

namespace TKP.Server.Application
{
    public static class DependencyInjection
    {
        public static WebApplicationBuilder AddApplicationDI(this WebApplicationBuilder builder)
        {
            var authConfigSetting = new AuthConfigSetting(builder.Configuration.GetValue<int>("AuthConfigSetting:MaxFailedLoginAttempts", 5));

            builder.Services.AddSingleton(authConfigSetting);

            builder.Services.AddApplicationServices();
            return builder;
        }

        private static void AddApplicationServices(this IServiceCollection services)
        {
            // Add your application services here
            // Example: services.AddScoped<IYourService, YourService>();
            services.AddSingleton<IPermissionService, PermissionService>();
        }
    }
}
