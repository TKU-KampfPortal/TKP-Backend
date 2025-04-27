using TKP.Server.Application.HelperServices.Cache;
using TKP.Server.Application.Repositories.Interface;
using TKP.Server.Domain.Entites;
using TKP.Server.Domain.Enums;

namespace TKP.Server.Infrastructure.Repositories.Cache
{
    public class RolePermissionCacheRepository : BaseCacheRepository<RolePermission>, IRolePermissionRepository
    {
        private readonly IRolePermissionRepository _inner;
        private readonly ICacheService<List<string>> _cacheRoleService;
        public RolePermissionCacheRepository(ICacheFactory cacheFactory
            , IRolePermissionRepository rolePermissionRepository) : base(cacheFactory, rolePermissionRepository, PrefixCacheKey.RolePermission)
        {
            _inner = rolePermissionRepository;
            _cacheRoleService = cacheFactory.GetCacheService<List<string>>();
        }


        public async Task<List<RolePermission>> GetRolePermissionsByListRoleIdAsync(List<Guid> roleIds, CancellationToken cancellationToken = default)
        {
            return await _inner.GetRolePermissionsByListRoleIdAsync(roleIds, cancellationToken);
        }
        public async Task<List<RolePermission>> GetRolePermissionsByRoleIdAsync(Guid roleId, CancellationToken cancellationToken = default)
        {
            return await _inner.GetRolePermissionsByRoleIdAsync(roleId, cancellationToken);
        }
        public async Task<List<string>> GetPermissionsByRoleIdAsync(Guid roleId, CancellationToken cancellationToken = default)
        {
            var cacheKey = GetCacheKey(roleId);

            var permissions = await _cacheRoleService.GetValueAsync(PrefixCacheKey, cacheKey);

            if (permissions is null)
            {
                permissions = await _inner.GetPermissionsByRoleIdAsync(roleId, cancellationToken);

                if (permissions is not null)
                {
                    await _cacheRoleService.SetValueAsync(PrefixCacheKey, cacheKey, permissions, TimeSpan.FromMinutes(2));
                }
            }

            return permissions ?? new List<string>();
        }


        public async Task<List<string>> GetPermissionsByListRoleIdAsync(List<Guid> roleIds, CancellationToken cancellationToken = default)
        {
            var roleIdsKey = string.Join("-", roleIds.Select(id => id.ToString()));

            var permissions = await _cacheRoleService.GetValueAsync(PrefixCacheKey, $"roleId-{roleIdsKey}");

            if (permissions is null)
            {
                permissions = await _inner.GetPermissionsByListRoleIdAsync(roleIds, cancellationToken);

                if (permissions is not null)
                {

                    await _cacheRoleService.SetValueAsync(PrefixCacheKey, $"roleId-{roleIdsKey}", permissions, TimeSpan.FromMinutes(2));
                }
            }

            return permissions ?? new List<string>();  // return empty list if null
        }
        private string GetCacheKey(Guid roleId)
        {
            return $"roleId-{roleId.ToString()}";
        }

    }
}
