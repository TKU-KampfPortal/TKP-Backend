using MediatR;

namespace TKP.Server.Application.Configurations.Queries
{
    public abstract record BaseQuery<T> : IRequest<T>;
}
