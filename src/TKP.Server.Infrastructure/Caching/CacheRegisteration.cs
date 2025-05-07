using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using TKP.Server.Application.HelperServices.Cache;
using TKP.Server.Infrastructure.Caching.Services;
using TKP.Server.Infrastructure.Caching.Strategies;

namespace TKP.Server.Infrastructure.Caching
{
    public static class CacheRegisteration
    {
        internal static WebApplicationBuilder AddApplicationCaching(this WebApplicationBuilder builder)
        {
            builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
            {
                var configuration = builder.Configuration.GetSection("Redis")["ConnectionString"];
                return ConnectionMultiplexer.Connect(configuration!);
            });

            builder.Services.AddCacheServices();

            return builder;
        }
        private static IServiceCollection AddCacheServices(this IServiceCollection services)
        {
            services.AddScoped<RedisCacheStragegy>()
                    .AddScoped(typeof(ICacheService<>), typeof(CacheService<>))
                    .AddScoped<ICacheFactory, CacheFactory>();
            return services;
        }

    }
}
