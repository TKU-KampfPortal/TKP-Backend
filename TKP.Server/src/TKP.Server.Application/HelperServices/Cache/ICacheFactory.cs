namespace TKP.Server.Application.HelperServices.Cache
{
    /// <summary>
    /// Defines a factory for creating cache services for specific entity types.
    /// </summary>
    public interface ICacheFactory
    {
        /// <summary>
        /// Retrieves a cache service instance for the specified entity type.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity to be cached.</typeparam>
        /// <returns>An instance of <see cref="ICacheService{TEntity}"/> configured for the specified entity type.</returns>
        ICacheService<TEntity> GetCacheService<TEntity>();
    }

}
