using Microsoft.AspNetCore.Mvc;
using TKP.Server.Application.Configurations.Commands;
using TKP.Server.Application.Models.Dtos.Auth;

namespace TKP.Server.Application.Features.Auth.Commands.RefreshToken
{
    public sealed record RefreshTokenCommand : BaseCommand<AuthTokenResponseDto>
    {
        [FromBody]
        public RefreshTokenBody Body { get; set; }
    }
    public sealed record RefreshTokenBody
    {
        public string AccessToken { get; set; } = string.Empty;
    }
}
