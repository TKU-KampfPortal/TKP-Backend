﻿using Microsoft.Extensions.DependencyInjection;
using TKP.Server.Application.Enum;
using TKP.Server.Application.HelperServices.Cache;
using TKP.Server.Domain.Enums;
using TKP.Server.Infrastructure.Caching.Strategies;
using TKP.Server.Infrastructure.Caching.Strategies.Interface;

namespace TKP.Server.Infrastructure.Caching.Services
{
    public sealed class CacheService<T> : ICacheService<T>
    {
        private readonly IServiceProvider _serviceProvider;
        private ICacheStragegy _cacheStragegy;
        private readonly double _cacheKeyInHours = 2;
        public CacheService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _cacheStragegy = GetDefaultCacheStragegy();
        }
        private ICacheStragegy GetDefaultCacheStragegy()
        {
            return _serviceProvider.GetService<RedisCacheStragegy>() ?? throw new InvalidOperationException("Redis Stragegy not registered");
        }
        public async Task<T?> GetValueAsync(PrefixCacheKey prefix, string key) => await _cacheStragegy.GetValueAsync<T>(GetKeyName(prefix, key));

        public async Task SetValueAsync(PrefixCacheKey prefix, string key, T value, TimeSpan? expiration)
            => await _cacheStragegy.SetValueAsync(GetKeyName(prefix, key), value, expiration ?? TimeSpan.FromHours(_cacheKeyInHours));

        public async Task SetValueAsync(PrefixCacheKey prefix, string key, T value, DateTime expirationDate)
        {
            var expiration = expirationDate - DateTime.UtcNow;
            await SetValueAsync(prefix, key, value, expiration);
        }

        public Task<bool> ExistsKeyAsync(PrefixCacheKey prefix, string key)
        {
            var cacheKey = GetKeyName(prefix, key);
            return _cacheStragegy.ExistsKeyAsync(cacheKey);
        }

        public async Task RemoveKeyAsync(PrefixCacheKey prefix, string key) => await _cacheStragegy.RemoveKeyAsync(GetKeyName(prefix, key));
        public async Task RemoveListKeysAsync(PrefixCacheKey prefix, List<string> keys)
        {
            var cacheKeys = keys.Select(key => GetKeyName(prefix, key)).ToList();
            var tasks = new List<Task>();

            foreach (var key in keys)
            {
                tasks.Add(_cacheStragegy.RemoveKeyAsync(GetKeyName(prefix, key)));
            }

            await Task.WhenAll(tasks);
        }

        public void SetStrategy(CacheStrategyEnum strategy)
        {
            _cacheStragegy = strategy switch
            {
                CacheStrategyEnum.Redis => GetDefaultCacheStragegy() ?? throw new InvalidOperationException("Redis strategy not registered"),
                _ => throw new NotImplementedException($"Cache strategy {strategy} is not implemented")
            };
        }

        private string GetKeyName(PrefixCacheKey prefix, string key)
        {
            return $"{prefix.ToString()}_{key}";
        }

    }
}
