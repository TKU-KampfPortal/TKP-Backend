using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TKP.Server.Application.Features.Auth.Commands.Login;
using TKP.Server.Application.Features.Auth.Commands.RefreshToken;
using TKP.Server.Application.Features.Auth.Queries.GetMe;
using TKP.Server.Application.Features.Auth.Queries.Logout;
using TKP.Server.Application.Models;
using TKP.Server.Application.Models.Dtos.Auth;
using TKP.Server.WebAPI.Controllers.Base;

namespace TKP.Server.WebAPI.Controllers
{
    [Route("api/auth")]
    public class AuthController : BaseController
    {
        public AuthController(IMediator mediator) : base(mediator)
        {
        }
        [HttpPost("login")]
        [ProducesResponseType(typeof(ApiResult<AuthTokenResponseDto>), StatusCodes.Status200OK)] // OK with AuthTokenResponseDto
        public async Task<IActionResult> LoginAsync(LoginCommand command) => await SendCommandAsync<AuthTokenResponseDto>(command);

        [HttpPost("refresh-token")]
        [ProducesResponseType(typeof(ApiResult<AuthTokenResponseDto>), StatusCodes.Status200OK)] // OK with AuthTokenResponseDto
        public async Task<IActionResult> RefreshTokenAsync(RefreshTokenCommand command) => await SendCommandAsync<AuthTokenResponseDto>(command);

        [HttpGet("get-me")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResult<AuthUserDto>), StatusCodes.Status200OK)] // OK with AuthUserDto
        public async Task<IActionResult> GetMeAsync() => await SendQueryAsync<AuthUserDto>(new GetMeQuery());

        [HttpGet("logout")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResult<bool>), StatusCodes.Status200OK)] // OK with AuthUserDto
        public async Task<IActionResult> LogoutAsync() => await SendQueryAsync<bool>(new LogoutQuery());

    }
}
