using Microsoft.Extensions.DependencyInjection;
using TKP.Server.Application.MappingProfile;

namespace TKP.Server.Infrastructure.Mapper
{
    internal static class MapperRegisteration
    {
        internal static IServiceCollection AddApplicationMapper(this IServiceCollection services)
        {
            // Add AutoMapper
            services.AddAutoMapper(typeof(IMappingProfileMarker));
            return services;
        }
    }
}
