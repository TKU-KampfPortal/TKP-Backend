using TKP.Server.Domain.Helpers;
using TKP.Server.Infrastructure.Validation;
using TKP.Server.WebAPI.ConfigSettings;

namespace TKP.Server.WebApi
{
    public static class DependencyInjection
    {
        public static WebApplicationBuilder AddWebApiDI(this WebApplicationBuilder builder)
        {
            // Add WebApi services
            builder.Services.AddControllers(config => config.Filters.Add(typeof(ValidateModelAttribute))).AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new UnixTimestampConverter());
            });
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddHttpContextAccessor();

            // Add Cors
            builder.AddApplicationCors();

            return builder;
        }
        private static void AddApplicationCors(this WebApplicationBuilder builder)
        {
            builder.Services.Configure<CorsSettings>(builder.Configuration.GetSection("CorsSettings"));

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigins", policy =>
                {
                    var allowedOrigins = builder.Configuration.GetSection("CorsSettings:AllowedOrigins").Get<string[]>();
                    policy.WithOrigins(allowedOrigins) // Add allowed origins from config
                          .AllowAnyMethod()
                          .AllowAnyHeader();
                });
            });
        }
    }
}
