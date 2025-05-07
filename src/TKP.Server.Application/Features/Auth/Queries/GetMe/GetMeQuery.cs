using TKP.Server.Application.Configurations.Queries;
using TKP.Server.Application.Models.Dtos.Auth;

namespace TKP.Server.Application.Features.Auth.Queries.GetMe
{
    public sealed record GetMeQuery : BaseQuery<AuthUserDto>
    {
    }
}
