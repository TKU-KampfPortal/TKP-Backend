using TKP.Server.Domain.Entites;

namespace TKP.Server.Application.Repositories.Interface
{
    /// <summary>
    /// Repository interface for managing RolePermission entities.
    /// </summary>
    public interface IRolePermissionRepository : IBaseRepository<RolePermission>
    {
        /// <summary>
        /// Retrieves a list of role permissions associated with a specific role ID.
        /// </summary>
        /// <param name="roleId">The ID of the role to retrieve permissions for.</param>
        /// <param name="cancellationToken">Cancellation token to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the list of role permissions.</returns>
        Task<List<RolePermission>> GetRolePermissionsByRoleIdAsync(Guid roleId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves a list of role permissions associated with a list of role IDs.
        /// </summary>
        /// <param name="roleIds">The list of role IDs to retrieve permissions for.</param>
        /// <param name="cancellationToken">Cancellation token to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the list of role permissions.</returns>
        Task<List<RolePermission>> GetRolePermissionsByListRoleIdAsync(List<Guid> roleIds, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves a list of role permissions associated with a specific role ID.
        /// </summary>
        /// <param name="roleId">The ID of the role to retrieve permissions for.</param>
        /// <param name="cancellationToken">Cancellation token to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the list of permissions.</returns>
        Task<List<string>> GetPermissionsByRoleIdAsync(Guid roleId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves a list of role permissions associated with a list of role IDs.
        /// </summary>
        /// <param name="roleIds">The list of role IDs to retrieve permissions for.</param>
        /// <param name="cancellationToken">Cancellation token to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the list of permissions.</returns>
        Task<List<string>> GetPermissionsByListRoleIdAsync(List<Guid> roleIds, CancellationToken cancellationToken = default);
    }

}
