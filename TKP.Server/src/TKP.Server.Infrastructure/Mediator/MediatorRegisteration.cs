using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using TKP.Server.Application.Features;

namespace TKP.Server.Infrastructure.Mediator
{
    public static class MediatorRegisteration
    {
        public static WebApplicationBuilder AddApplicationMediator(this WebApplicationBuilder builder)
        {

            builder.Services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(typeof(IMarker).Assembly);
            });

            return builder;
        }
    }
}
