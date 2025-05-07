using TKP.Server.Application.HelperServices.Cache;
using TKP.Server.Application.Repositories.Interface;
using TKP.Server.Core.Entities;
using TKP.Server.Domain.Enums;

namespace TKP.Server.Infrastructure.Repositories.Cache
{
    public abstract class BaseCacheRepository<TEntity> : IBaseRepository<TEntity> where TEntity : BaseEntity
    {
        protected readonly PrefixCacheKey PrefixCacheKey;
        private readonly IBaseRepository<TEntity> _baseRepository;
        protected readonly ICacheService<TEntity> CacheService;
        public BaseCacheRepository(ICacheFactory cacheFactory
            , IBaseRepository<TEntity> baseRepository
            , PrefixCacheKey prefixCacheKey)
        {
            _baseRepository = baseRepository;
            CacheService = cacheFactory.GetCacheService<TEntity>();
            PrefixCacheKey = prefixCacheKey;
        }
        public async Task AddAsync(TEntity entity, CancellationToken cancellationToken = default)
            => await _baseRepository.AddAsync(entity, cancellationToken);
        public async Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
            => await _baseRepository.AddRangeAsync(entities, cancellationToken);
        public async Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            await CacheService.RemoveKeyAsync(PrefixCacheKey, $"id-{entity.Id.ToString()}");
            await _baseRepository.DeleteAsync(entity, cancellationToken);
        }
        public async Task DeleteRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
        {
            await CacheService.RemoveListKeysAsync(PrefixCacheKey, entities.Select(x => $"id-{x.Id.ToString()}").ToList());
            await _baseRepository.DeleteRangeAsync(entities, cancellationToken);
        }
        public async Task<List<TEntity>> GetAllAsync(CancellationToken cancellationToken = default)
         => await _baseRepository.GetAllAsync(cancellationToken);
        public async Task<PaginationResponse<TEntity>> GetAllByPaginationAsync(int pageIndex, int pageSize, CancellationToken cancellationToken = default)
            => await _baseRepository.GetAllByPaginationAsync(pageIndex, pageSize, cancellationToken);
        public async Task<TEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var entity = await CacheService.GetValueAsync(PrefixCacheKey, $"id-{id.ToString()}");
            if (entity is null)
            {
                entity = await _baseRepository.GetByIdAsync(id, cancellationToken);
                if (entity is not null)
                {
                    await CacheService.SetValueAsync(PrefixCacheKey, $"id-{id.ToString()}", entity, null);
                }
            }
            return entity;
        }
        public Task<List<TEntity>> GetByListIdAsync(List<Guid> ids, CancellationToken cancellationToken = default)
        {
            return _baseRepository.GetByListIdAsync(ids, cancellationToken);
        }
        public async Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            await CacheService.RemoveKeyAsync(PrefixCacheKey, $"id-{entity.Id.ToString()}");
            await _baseRepository.UpdateAsync(entity, cancellationToken);
        }
        public async Task UpdateRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
        {
            await CacheService.RemoveListKeysAsync(PrefixCacheKey, entities.Select(x => $"id-{x.Id.ToString()}").ToList());
            await _baseRepository.UpdateRangeAsync(entities, cancellationToken);
        }
    }
}
