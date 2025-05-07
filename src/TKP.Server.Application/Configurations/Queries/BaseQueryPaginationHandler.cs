using TKP.Server.Core.Entities;

namespace TKP.Server.Application.Configurations.Queries
{
    public abstract class BaseQueryPaginationHandler<TQuery, TResponse> : BaseQueryHandler<TQuery, PaginationResponse<TResponse>> where TQuery : BaseQueryPagination<TResponse>
    {
    }
}
