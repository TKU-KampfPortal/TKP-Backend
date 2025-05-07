using Microsoft.Extensions.DependencyInjection;
using TKP.Server.Application.HelperServices.Cache;

namespace TKP.Server.Infrastructure.Caching
{
    public sealed class CacheFactory : ICacheFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public CacheFactory(IServiceProvider serviceProvider)
        {
            this._serviceProvider = serviceProvider;
        }

        public ICacheService<TEntity> GetCacheService<TEntity>() => this._serviceProvider.GetRequiredService<ICacheService<TEntity>>();
    }
}
