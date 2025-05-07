using Microsoft.AspNetCore.Mvc;
using TKP.Server.Core.Entities;

namespace TKP.Server.Application.Configurations.Queries
{
    public abstract record BaseQueryPagination<T> : BaseQuery<PaginationResponse<T>>
    {
        [FromQuery(Name = "pageIndex")]
        public int PageIndex { get; set; } = 0;

        [FromQuery(Name = "pageSize")]
        public int PageSize { get; set; } = 100;

    }
}
