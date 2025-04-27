using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System.Data;
using TKP.Server.Application.Configurations.Queries;
using TKP.Server.Application.Exceptions;
using TKP.Server.Application.HelperServices;
using TKP.Server.Application.HelperServices.Cookie;
using TKP.Server.Application.Models.Dtos.Roles;
using TKP.Server.Application.Repositories.Interface;
using TKP.Server.Domain.Entites;

namespace TKP.Server.Application.Features.Roles.Queries.GetRoleById
{
    public sealed class GetRoleByIdQueryHandler : BaseQueryHandler<GetRoleByIdQuery, RoleDto>
    {
        private readonly IRolePermissionRepository _rolePermissionRepository;
        private readonly IPermissionService _permissionHelperService;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IMapper _mapper;

        public GetRoleByIdQueryHandler(IRolePermissionRepository rolePermissionRepository
            , IPermissionService permissionHelperService
            , IMapper mapper
            , RoleManager<ApplicationRole> roleManager)
        {
            _rolePermissionRepository = rolePermissionRepository;
            _permissionHelperService = permissionHelperService;
            _roleManager = roleManager;
            _mapper = mapper;
        }

        protected override async Task<RoleDto> HandleAsync(GetRoleByIdQuery request, CancellationToken cancellationToken = default)
        {
            var role = await _roleManager.FindByIdAsync(request.RoleId.ToString());

            if (role is null)
            {
                throw new NotFoundException($"Role with Id: '{request.RoleId}' is not found");
            }

            var rolePermissions = await _rolePermissionRepository.GetRolePermissionsByRoleIdAsync(role.Id);
            var permissions = rolePermissions.Select(x => x.Permission).ToList();
            var response = _mapper.Map<RoleDto>(role);

            response.Permissions = _permissionHelperService.GetPermisionByListKey(permissions);

            return response;
        }
    }
}
