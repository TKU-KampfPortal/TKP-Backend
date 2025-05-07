using Microsoft.AspNetCore.Identity;
using TKP.Server.Application.Configurations.Commands;
using TKP.Server.Application.Exceptions;
using TKP.Server.Application.Repositories.Interface;
using TKP.Server.Domain.Entites;

namespace TKP.Server.Application.Features.Roles.Commands.DeleteRole
{
    public sealed class DeleteRoleCommandHandler : BaseCommandHandler<DeleteRoleCommand, bool>
    {
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IRolePermissionRepository _rolePermissionRepository;
        private readonly IUnitOfWork _unitOfWork;
        public DeleteRoleCommandHandler(RoleManager<ApplicationRole> roleManager
            , IRolePermissionRepository rolePermissionRepository
            , IUnitOfWork unitOfWork)
        {
            _roleManager = roleManager;
            _rolePermissionRepository = rolePermissionRepository;
            _unitOfWork = unitOfWork;
        }
        protected override async Task<bool> HandleAsync(DeleteRoleCommand request, CancellationToken cancellationToken)
        {
            var role = await _roleManager.FindByIdAsync(request.RoleId.ToString());

            if (role is null)
            {
                throw new NotFoundException($"Role with Id: '{request.RoleId}' is not found");
            }

            await _unitOfWork.ExecuteAsync(async () =>
            {
                var rolePermissions = await _rolePermissionRepository.GetRolePermissionsByRoleIdAsync(role.Id, cancellationToken);

                if (rolePermissions is not null && rolePermissions.Count > 0)
                {
                    await _rolePermissionRepository.DeleteRangeAsync(rolePermissions);
                }

                await _roleManager.DeleteAsync(role);
            }, cancellationToken);
            return true;
        }
    }
}
