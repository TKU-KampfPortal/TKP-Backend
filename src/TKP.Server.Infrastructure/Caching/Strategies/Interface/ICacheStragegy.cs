namespace TKP.Server.Infrastructure.Caching.Strategies.Interface
{
    /// <summary>
    /// Defines a generic caching strategy for storing and retrieving values.
    /// </summary>
    public interface ICacheStragegy
    {
        /// <summary>
        /// Retrieves the value associated with the specified key from the cache.
        /// </summary>
        /// <typeparam name="T">The type of the cached value.</typeparam>
        /// <param name="key">The key identifying the cached value.</param>
        /// <returns>The cached value if found; otherwise, <c>null</c>.</returns>
        Task<T?> GetValueAsync<T>(string key);

        /// <summary>
        /// Stores a value in the cache with the specified key and expiration time.
        /// </summary>
        /// <typeparam name="T">The type of the value to cache.</typeparam>
        /// <param name="key">The key to associate with the cached value.</param>
        /// <param name="value">The value to store in the cache.</param>
        /// <param name="expiration">The duration after which the cache entry should expire.</param>
        Task SetValueAsync<T>(string key, T value, TimeSpan expiration);

        /// <summary>
        /// Removes the value associated with the specified key from the cache.
        /// </summary>
        /// <param name="key">The key identifying the cached value to remove.</param>
        Task RemoveKeyAsync(string key);

        /// <summary>
        /// Checks whether a cache entry exists for the specified key.
        /// </summary>
        /// <param name="key">The key to check in the cache.</param>
        /// <returns><c>true</c> if the key exists in the cache; otherwise, <c>false</c>.</returns>
        Task<bool> ExistsKeyAsync(string key);
    }

}
