using Microsoft.EntityFrameworkCore;
using TKP.Server.Application.Repositories.Interface;
using TKP.Server.Core.Entities;
using TKP.Server.Infrastructure.Data;
using TKP.Server.Infrastructure.Helpers;

namespace TKP.Server.Infrastructure.Repositories
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : BaseEntity
    {
        protected readonly ApplicationDbContext Context;
        protected readonly DbSet<TEntity> DbSet;
        public BaseRepository(ApplicationDbContext context)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
            DbSet = context.Set<TEntity>();
        }
        public async Task<List<TEntity>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await DbSet.ToListAsync(cancellationToken);
        }
        public Task<PaginationResponse<TEntity>> GetAllByPaginationAsync(int pageIndex, int pageSize, CancellationToken cancellationToken = default)
        {
            return DbSet.ToPaginatedListAsync(pageIndex, pageSize, cancellationToken);
        }

        public async Task<TEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await DbSet.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
        }
        public async Task AddAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            await DbSet.AddAsync(entity);
            await Context.SaveChangesAsync(cancellationToken);
        }
        public async Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
        {
            await DbSet.AddRangeAsync(entities);
            await Context.SaveChangesAsync(cancellationToken);
        }
        public async Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            DbSet.Update(entity);
            await Context.SaveChangesAsync(cancellationToken);
        }
        public async Task UpdateRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
        {
            DbSet.UpdateRange(entities);
            await Context.SaveChangesAsync(cancellationToken);
        }
        public async Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            DbSet.Remove(entity);
            await Context.SaveChangesAsync(cancellationToken);
        }
        public async Task DeleteRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
        {
            DbSet.RemoveRange(entities);
            await Context.SaveChangesAsync(cancellationToken);
        }

    }
}
