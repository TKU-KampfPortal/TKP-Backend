using MediatR;

namespace TKP.Server.Application.Configurations.Commands
{
    public abstract record BaseCommand<T> : IRequest<T>;
}
