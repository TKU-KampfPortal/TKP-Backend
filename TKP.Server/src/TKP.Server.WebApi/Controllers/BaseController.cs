using MediatR;
using Microsoft.AspNetCore.Mvc;
using TKP.Server.Application.Configurations.Commands;
using TKP.Server.Application.Configurations.Queries;
using TKP.Server.Application.Models;

namespace TKP.Server.WebApi.Controllers
{
    /// <summary>
    /// Base controller that provides common functionalities for derived API controllers.
    /// </summary>
    [ApiController]
    public abstract class BaseController : ControllerBase
    {
        private readonly IMediator _mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseController"/> class.
        /// </summary>
        /// <param name="mediator">The MediatR instance used for sending commands and queries.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="mediator"/> is null.</exception>
        protected BaseController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        /// <summary>
        /// Sends a command to the corresponding handler using MediatR.
        /// </summary>
        /// <typeparam name="TResponse">The type of the response expected from the handler.</typeparam>
        /// <param name="command">The command to be sent.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation, containing the handler's response.</returns>
        protected async Task<IActionResult> SendCommandAsync<TResponse>(BaseCommand<TResponse> command, CancellationToken cancellationToken = default)
        {
            return Ok(ApiResult<TResponse>.Success(await _mediator.Send(command, cancellationToken)));
        }

        /// <summary>
        /// Sends a query to the corresponding handler using MediatR.
        /// </summary>
        /// <typeparam name="TResponse">The type of the response expected from the handler.</typeparam>
        /// <param name="query">The query to be sent.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation, containing the handler's response.</returns>
        protected async Task<IActionResult> SendQueryAsync<TResponse>(BaseQuery<TResponse> query, CancellationToken cancellationToken = default)
        {
            return Ok(ApiResult<TResponse>.Success(await _mediator.Send(query, cancellationToken)));
        }
    }

}

