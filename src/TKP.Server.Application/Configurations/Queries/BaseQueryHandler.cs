using MediatR;

namespace TKP.Server.Application.Configurations.Queries
{
    public abstract class BaseQueryHandler<TQuery, TResponse> : IRequestHandler<TQuery, TResponse> where TQuery : BaseQuery<TResponse>
    {
        public async Task<TResponse> Handle(TQuery request, CancellationToken cancellationToken)
        {
            return await HandleAsync(request, cancellationToken);
        }
        protected abstract Task<TResponse> HandleAsync(TQuery request, CancellationToken cancellationToken = default);
    }
}
