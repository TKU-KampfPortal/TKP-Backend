using Microsoft.AspNetCore.Mvc;
using TKP.Server.Application.Configurations.Commands;

namespace TKP.Server.Application.Features.Roles.Commands.DeleteRole
{
    public sealed record DeleteRoleCommand : BaseCommand<bool>
    {
        [FromRoute(Name = "roleId")]
        public Guid RoleId { get; set; }
    }
}
