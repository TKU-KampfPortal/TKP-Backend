using TKP.Server.Application.Enum;
using TKP.Server.Domain.Enums;

namespace TKP.Server.Application.HelperServices.Cache
{
    /// <summary>
    /// Defines the contract for a generic cache service that supports various caching strategies.
    /// </summary>
    /// <typeparam name="T">The type of object to be cached.</typeparam>
    public interface ICacheService<T>
    {
        /// <summary>
        /// Retrieves a cached value by prefix and key.
        /// </summary>
        /// <param name="prefix">The prefix used to group keys logically.</param>
        /// <param name="key">The key of the cached item.</param>
        /// <returns>The cached value if it exists; otherwise, null.</returns>
        Task<T?> GetValueAsync(PrefixCacheKey prefix, string key);

        /// <summary>
        /// Caches a value with the specified prefix, key, and expiration time.
        /// </summary>
        /// <param name="prefix">The prefix used to group keys logically.</param>
        /// <param name="key">The key to store the cached item under.</param>
        /// <param name="value">The value to be cached.</param>
        /// <param name="expiration">The duration until the cached item expires.</param>
        Task SetValueAsync(PrefixCacheKey prefix, string key, T value, TimeSpan expiration);

        /// <summary>
        /// Caches a value with the specified prefix, key, and expiration time.
        /// </summary>
        /// <param name="prefix">The prefix used to group keys logically.</param>
        /// <param name="key">The key to store the cached item under.</param>
        /// <param name="value">The value to be cached.</param>
        /// <param name="expirationDate">The date of cached item will expires.</param>
        Task SetValueAsync(PrefixCacheKey prefix, string key, T value, DateTime expirationDate);
        /// <summary>
        /// Removes a cached value by prefix and key.
        /// </summary>
        /// <param name="prefix">The prefix used to group keys logically.</param>
        /// <param name="key">The key of the cached item to remove.</param>
        Task RemoveKeyAsync(PrefixCacheKey prefix, string key);

        /// <summary>
        /// Checks whether a cached key exists.
        /// </summary>
        /// <param name="prefix">The prefix used to group keys logically.</param>
        /// <param name="key">The key to check for existence.</param>
        /// <returns>True if the key exists; otherwise, false.</returns>
        Task<bool> ExistsKeyAsync(PrefixCacheKey prefix, string key);

        /// <summary>
        /// Sets the current cache strategy (e.g., Redis, Memory).
        /// </summary>
        /// <param name="strategy">The caching strategy to use.</param>
        void SetStragety(CacheStragegyEnum strategy);
    }

}
