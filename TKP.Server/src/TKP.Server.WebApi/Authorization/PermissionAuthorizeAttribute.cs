using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;
using TKP.Server.Application.HelperServices.Interface;
using TKP.Server.Application.Repositories.Interface;
using TKP.Server.Domain.Entites;

namespace TKP.Server.WebAPI.Authorization
{

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public class PermissionAuthorizeAttribute : Attribute, IAsyncAuthorizationFilter
    {
        private readonly List<string> _requiredPermissions;

        public PermissionAuthorizeAttribute(params string[] requiredPermissions)
        {
            _requiredPermissions = requiredPermissions.ToList();
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var permissionService = context.HttpContext.RequestServices.GetRequiredService<IPermissionService>();
            var rolePermissionRepository = context.HttpContext.RequestServices.GetRequiredService<IRolePermissionRepository>();
            var roleManager = context.HttpContext.RequestServices.GetRequiredService<RoleManager<ApplicationRole>>();

            // Nếu không có user => cấm
            var user = context.HttpContext.User;
            if (user == null || !user.Identity.IsAuthenticated)
            {
                context.Result = new ForbidResult();
                return;
            }

            // Lấy tất cả role claims
            var roleClaimIds = user.Claims
                .Where(c => c.Type == ClaimTypes.Role)
                .Select(c => new Guid(c.Value))
                .ToList();

            if (roleClaimIds == null || !roleClaimIds.Any())
            {
                context.Result = new ForbidResult();
                return;
            }

            // Get all permissions by roleIds
            var userPermissions = await rolePermissionRepository.GetPermissionsByListRoleIdAsync(roleClaimIds);

            if (!IsAuthorized(userPermissions, _requiredPermissions))
            {
                context.Result = new ForbidResult();
                return;
            }
        }

        private bool IsAuthorized(List<string> userPermissions, List<string> requiredPermissions)
        {
            foreach (var required in requiredPermissions)
            {
                bool hasPermission = userPermissions.Any(userPermission =>
                    userPermission.Equals(required, StringComparison.OrdinalIgnoreCase) ||
                    required.StartsWith(userPermission + ".", StringComparison.OrdinalIgnoreCase)
                );

                if (!hasPermission)
                {
                    return false;
                }
            }
            return true;
        }
    }

}
