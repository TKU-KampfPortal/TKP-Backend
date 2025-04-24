using MediatR;

namespace TKP.Server.Application.Configurations.Commands
{
    public abstract class BaseCommandHandler<TCommand, TResponse> : IRequestHandler<TCommand, TResponse> where TCommand : BaseCommand<TResponse>
    {

        public async Task<TResponse> Handle(TCommand request, CancellationToken cancellationToken)
        {
            return await HandleAsync(request, cancellationToken);
        }

        protected abstract Task<TResponse> HandleAsync(TCommand request, CancellationToken cancellationToken);
    }
}
