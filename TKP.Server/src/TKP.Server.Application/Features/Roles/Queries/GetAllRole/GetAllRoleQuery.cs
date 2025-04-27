using TKP.Server.Application.Configurations.Queries;
using TKP.Server.Application.Models.Dtos.Roles;

namespace TKP.Server.Application.Features.Roles.Queries.GetAllRole
{
    public sealed record GetAllRoleQuery : BaseQueryPagination<RoleListDto>
    {
    }
}
