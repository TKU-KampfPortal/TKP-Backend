using Microsoft.AspNetCore.Mvc;
using TKP.Server.Application.Configurations.Commands;
using TKP.Server.Application.Features.Roles.Shared.Dtos;

namespace TKP.Server.Application.Features.Roles.Commands.UpdateRole
{
    public sealed record UpdateRoleCommand : BaseCommand<bool>
    {
        [FromRoute(Name = "roleId")]
        public Guid RoleId { get; set; }

        [FromBody]
        public RoleRequestBody Body { get; set; }
    }

}
