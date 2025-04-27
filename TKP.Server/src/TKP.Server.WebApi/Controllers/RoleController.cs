using MediatR;
using Microsoft.AspNetCore.Mvc;
using TKP.Server.Application.Features.Roles.Commands.CreateRole;
using TKP.Server.Application.Features.Roles.Commands.DeleteRole;
using TKP.Server.Application.Features.Roles.Commands.UpdateRole;
using TKP.Server.Application.Features.Roles.Queries.GetAllRole;
using TKP.Server.Application.Features.Roles.Queries.GetRoleById;
using TKP.Server.Application.Models;
using TKP.Server.Application.Models.Dtos.Roles;
using TKP.Server.Core.Entities;
using TKP.Server.WebAPI.Authorization;
using TKP.Server.WebAPI.Controllers.Base;

namespace TKP.Server.WebAPI.Controllers
{
    [Route("api/role")]
    public class RoleController : AuthorizeController
    {
        public RoleController(IMediator mediator) : base(mediator)
        {
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResult<bool>), StatusCodes.Status200OK)] // OK with AuthTokenResponseDto
        public async Task<IActionResult> CreateRoleAsync(CreateRoleCommand command, CancellationToken cancellationToken)
            => await SendCommandAsync<bool>(command, cancellationToken);

        [HttpPut("{roleId}")]
        [PermissionAuthorize("Permission.Post.View")]
        [ProducesResponseType(typeof(ApiResult<bool>), StatusCodes.Status200OK)] // OK with AuthTokenResponseDto
        public async Task<IActionResult> UpdateRoleByIdAsync(UpdateRoleCommand command, CancellationToken cancellationToken)
                        => await SendCommandAsync<bool>(command, cancellationToken);

        [HttpDelete("{roleId}")]
        [PermissionAuthorize("Permission.Post.Delete")]
        [ProducesResponseType(typeof(ApiResult<bool>), StatusCodes.Status200OK)] // OK with AuthTokenResponseDto
        public async Task<IActionResult> DeleteRoleByIdAsync(DeleteRoleCommand command, CancellationToken cancellationToken)
                        => await SendCommandAsync<bool>(command, cancellationToken);

        [HttpGet]
        [PermissionAuthorize("Permission.Post.View")]
        [ProducesResponseType(typeof(ApiResult<PaginationResponse<RoleListDto>>), StatusCodes.Status200OK)] // OK with AuthTokenResponseDto
        public async Task<IActionResult> GetAllRoleAsync(GetAllRoleQuery query, CancellationToken cancellationToken)
            => await SendPaginationQueryAsync<RoleListDto>(query, cancellationToken);

        [HttpGet("{roleId}")]
        public async Task<IActionResult> GetRoleByIdAsync(GetRoleByIdQuery query, CancellationToken cancellationToken)
            => await SendQueryAsync<RoleDto>(query, cancellationToken);
    }
}
