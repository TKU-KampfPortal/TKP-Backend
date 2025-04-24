using StackExchange.Redis;
using System.Text.Json;
using TKP.Server.Infrastructure.Caching.Strategies.Interface;

namespace TKP.Server.Infrastructure.Caching.Strategies
{
    public class RedisCacheStragegy : ICacheStragegy
    {
        private readonly IDatabase _database;
        public RedisCacheStragegy(IConnectionMultiplexer redis)
        {
            _database = redis.GetDatabase();
        }

        public async Task<T?> GetValueAsync<T>(string key)
        {
            var value = await _database.StringGetAsync(key);
            if (value.IsNullOrEmpty) return default;

            return JsonSerializer.Deserialize<T>(value!);
        }
        public async Task SetValueAsync<T>(string key, T value, TimeSpan expiration)
        {
            var serializedValue = JsonSerializer.Serialize(value);
            await _database.StringSetAsync(key, serializedValue, expiration);
        }
        public async Task RemoveKeyAsync(string key) => await _database.KeyDeleteAsync(key);
        public async Task<bool> ExistsKeyAsync(string key) => await _database.KeyExistsAsync(key);
    }
}
