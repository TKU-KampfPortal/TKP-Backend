using AutoMapper;
using Microsoft.AspNetCore.Identity;
using TKP.Server.Application.Configurations.Commands;
using TKP.Server.Application.Exceptions;
using TKP.Server.Application.HelperServices.Cookie;
using TKP.Server.Application.Repositories.Interface;
using TKP.Server.Domain.Entites;

namespace TKP.Server.Application.Features.Roles.Commands.UpdateRole
{
    public sealed class UpdateRoleCommandHandler : BaseCommandHandler<UpdateRoleCommand, bool>
    {
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IRolePermissionRepository _rolePermissionRepository;
        private readonly IMapper _mapper;
        private readonly IPermissionService _permissionHelperService;
        private readonly IUnitOfWork _unitOfWork;
        public UpdateRoleCommandHandler(RoleManager<ApplicationRole> roleManager
            , IRolePermissionRepository rolePermissionRepository
            , IMapper mapper
            , IPermissionService permissionHelperService
            , IUnitOfWork unitOfWork)
        {
            _roleManager = roleManager;
            _rolePermissionRepository = rolePermissionRepository;
            _mapper = mapper;
            _permissionHelperService = permissionHelperService;
            _unitOfWork = unitOfWork;
        }

        protected override async Task<bool> HandleAsync(UpdateRoleCommand request, CancellationToken cancellationToken)
        {
            var role = await _roleManager.FindByIdAsync(request.RoleId.ToString());

            if (role is null)
            {
                throw new NotFoundException($"Role with Id: '{request.RoleId}' is not found");
            }

            // Check new role existed
            var permissionKeys = request.Body.PermissionKeys.Distinct().ToList();
            var isPermissionExisted = _permissionHelperService.IsListPermissionExists(permissionKeys);

            if (!isPermissionExisted)
                throw new NotFoundException($"Some of permission keys not found");

            // Create new role Permission
            var newRolePermissions = request.Body.PermissionKeys
                .Select(permission => new RolePermission() { RoleId = role.Id, Permission = permission }).ToList();

            _mapper.Map(request.Body, role);

            await _unitOfWork.ExecuteAsync(async () =>
            {
                // Update role detail data

                await _roleManager.UpdateAsync(role);

                // Delete all current role permission
                var rolePermissions = await _rolePermissionRepository.GetRolePermissionsByRoleIdAsync(request.RoleId, cancellationToken);

                if (rolePermissions is not null && rolePermissions.Count > 0)
                {
                    await _rolePermissionRepository.DeleteRangeAsync(rolePermissions, cancellationToken);
                }
                // Create all new current role permission
                await _rolePermissionRepository.AddRangeAsync(newRolePermissions, cancellationToken);

            }, cancellationToken);
            return true;
        }
    }
}
