using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Formatting.Json;

namespace TKP.Server.Infrastructure.Logging
{
    internal static class SerilogRegistration
    {
        internal static void AddHostSerilogConfiguration(this IHostBuilder host)
        {
            host.UseSerilog((ctx, service, config) =>
            {
                config
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.File(new JsonFormatter(), "logs/log-.txt", rollingInterval: RollingInterval.Day);
            });
        }
    }
}
