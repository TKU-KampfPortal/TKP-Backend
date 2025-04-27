using Microsoft.AspNetCore.Mvc;
using TKP.Server.Application.Configurations.Queries;
using TKP.Server.Application.Models.Dtos.Roles;

namespace TKP.Server.Application.Features.Roles.Queries.GetRoleById
{
    public sealed record GetRoleByIdQuery : BaseQuery<RoleDto>
    {
        [FromRoute(Name = "roleId")]
        public Guid RoleId { get; set; }
    }
}
