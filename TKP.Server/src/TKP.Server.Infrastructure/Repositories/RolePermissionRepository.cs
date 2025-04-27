using Microsoft.EntityFrameworkCore;
using TKP.Server.Application.Repositories.Interface;
using TKP.Server.Domain.Entites;
using TKP.Server.Infrastructure.Data;

namespace TKP.Server.Infrastructure.Repositories
{
    public class RolePermissionRepository : BaseRepository<RolePermission>, IRolePermissionRepository
    {
        public RolePermissionRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<List<string>> GetPermissionsByRoleIdAsync(Guid roleId, CancellationToken cancellationToken = default)
        {
            return await DbSet.Where(x => x.RoleId == roleId)
                .Select(p => p.Permission)
                .ToListAsync(cancellationToken);
        }
        public async Task<List<string>> GetPermissionsByListRoleIdAsync(List<Guid> roleIds, CancellationToken cancellationToken = default)
        {
            return await DbSet.Where(x => roleIds.Contains(x.RoleId)).Select(p => p.Permission).Distinct().ToListAsync(cancellationToken);
        }

        public async Task<List<RolePermission>> GetRolePermissionsByRoleIdAsync(Guid roleId, CancellationToken cancellationToken = default)
        {
            return await DbSet.Where(x => x.RoleId == roleId).ToListAsync(cancellationToken);
        }

        public async Task<List<RolePermission>> GetRolePermissionsByListRoleIdAsync(List<Guid> roleIds, CancellationToken cancellationToken = default)
        {
            return await DbSet.Where(x => roleIds.Contains(x.RoleId)).Distinct().ToListAsync(cancellationToken);
        }
    }
}
