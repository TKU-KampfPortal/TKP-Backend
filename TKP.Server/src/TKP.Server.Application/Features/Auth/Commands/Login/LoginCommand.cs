using Microsoft.AspNetCore.Mvc;
using TKP.Server.Application.Configurations.Commands;
using TKP.Server.Application.Models.Dtos.Auth;

namespace TKP.Server.Application.Features.Auth.Commands.Login
{
    public sealed record LoginCommand : BaseCommand<AuthTokenResponseDto>
    {
        [FromBody]
        public BodyLoginCommand Body { get; init; } = new();
    }
    public sealed record BodyLoginCommand
    {
        public string UserName { get; init; } = string.Empty;
        public string Password { get; init; } = string.Empty;
    }
}
