using Microsoft.AspNetCore.Mvc;
using TKP.Server.Application.Configurations.Commands;
using TKP.Server.Application.Features.Roles.Shared.Dtos;

namespace TKP.Server.Application.Features.Roles.Commands.CreateRole
{
    public sealed record CreateRoleCommand : BaseCommand<bool>
    {
        [FromBody]
        public RoleRequestBody Body { get; init; } = new();
    }
}
