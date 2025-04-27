using AutoMapper;
using Microsoft.AspNetCore.Identity;
using TKP.Server.Application.Configurations.Commands;
using TKP.Server.Application.Exceptions;
using TKP.Server.Application.Features.Roles.Shared.Dtos;
using TKP.Server.Application.HelperServices.Cookie;
using TKP.Server.Application.Repositories.Interface;
using TKP.Server.Domain.Entites;

namespace TKP.Server.Application.Features.Roles.Commands.CreateRole
{
    public sealed class CreateRoleCommandHandler : BaseCommandHandler<CreateRoleCommand, bool>
    {
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IMapper _mapper;
        private readonly IRolePermissionRepository _rolePermissionRepository;
        private readonly IPermissionService _permissionHelperService;
        public CreateRoleCommandHandler(RoleManager<ApplicationRole> roleManager
            , IMapper mapper
            , IRolePermissionRepository rolePermissionRepository
            , IPermissionService permissionHelperService)
        {

            _roleManager = roleManager;
            _mapper = mapper;
            _rolePermissionRepository = rolePermissionRepository;
            _permissionHelperService = permissionHelperService;

        }

        protected override async Task<bool> HandleAsync(CreateRoleCommand request, CancellationToken cancellationToken)
        {
            var isRoleExisted = await _roleManager.RoleExistsAsync(request.Body.Name);

            if (isRoleExisted)
                throw new NotFoundException($"Role {request.Body.Name} already exists");

            var permissionKeys = request.Body.PermissionKeys.Distinct().ToList();
            var isPermissionExisted = _permissionHelperService.IsListPermissionExists(permissionKeys);

            if (!isPermissionExisted)
                throw new NotFoundException($"Some of permission keys not found");


            var role = _mapper.Map<RoleRequestBody, ApplicationRole>(request.Body);

            await _roleManager.CreateAsync(role);

            if (permissionKeys.Count != 0)
            {
                var rolePermissions = permissionKeys.Select(x => new RolePermission
                {
                    RoleId = role.Id,
                    Permission = x
                }).ToList();

                await _rolePermissionRepository.AddRangeAsync(rolePermissions, cancellationToken);
            }

            return true;
        }
    }
}
