using MediatR;
using Microsoft.AspNetCore.Authorization;

namespace TKP.Server.WebAPI.Controllers.Base
{
    [Authorize]
    public abstract class AuthorizeController : BaseController
    {
        protected AuthorizeController(IMediator mediator) : base(mediator)
        {
        }
    }
}
