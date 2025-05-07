using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TKP.Server.Application.Configurations.Queries;
using TKP.Server.Application.Exceptions;
using TKP.Server.Application.HelperServices;
using TKP.Server.Application.HelperServices.Cache;
using TKP.Server.Application.Models.Dtos.Auth;
using TKP.Server.Application.Repositories.Interface;
using TKP.Server.Domain.Entites;
using TKP.Server.Domain.Enums;

namespace TKP.Server.Application.Features.Auth.Queries.GetMe
{
    public sealed class GetMeQueryHandler : BaseQueryHandler<GetMeQuery, AuthUserDto>
    {
        private readonly ICacheService<AuthUserDto> _authUserCacheService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IClaimService _claimService;
        private readonly IMapper _mapper;
        private readonly IRolePermissionRepository _rolePermissionRepository;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public GetMeQueryHandler(ICacheFactory cacheFactory
            , IClaimService claimService
            , UserManager<ApplicationUser> userManager
            , IMapper mapper
            , IRolePermissionRepository rolePermissionRepository
            , RoleManager<ApplicationRole> roleManager)
        {
            _authUserCacheService = cacheFactory.GetCacheService<AuthUserDto>();
            _claimService = claimService;
            _userManager = userManager;
            _mapper = mapper;
            _rolePermissionRepository = rolePermissionRepository;
            _roleManager = roleManager;
        }

        protected override async Task<AuthUserDto> HandleAsync(GetMeQuery request, CancellationToken cancellationToken = default)
        {
            var userId = _claimService.GetUserId();
            var userResponse = await _authUserCacheService.GetValueAsync(PrefixCacheKey.AuthUser, userId);
            if (userResponse is null)
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user is null)
                {
                    throw new UnauthorizeException("User is not found");
                }

                // Get user role
                var userRoles = await _userManager.GetRolesAsync(user);

                // Find all role id;
                var roles = await _roleManager.Roles.Where(role => userRoles.ToList().Contains(role.Name)).ToListAsync(cancellationToken);

                // Get all role permission in role

                userResponse = _mapper.Map<ApplicationUser, AuthUserDto>(user);
                userResponse.Permissions = await _rolePermissionRepository
                    .GetPermissionsByListRoleIdAsync(roles.Select(r => r.Id).ToList());
                await _authUserCacheService.SetValueAsync(PrefixCacheKey.AuthUser, userId, userResponse, TimeSpan.FromHours(1));
            }

            return userResponse;

        }
    }
}
