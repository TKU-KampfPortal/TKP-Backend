using Microsoft.AspNetCore.Mvc;

namespace TKP.Server.Application.Models
{
    public class PaginationFilter
    {
        [FromQuery(Name = "pageIndex")]
        public int PageIndex { get; set; }

        [FromQuery(Name = "pageSize")]
        public int PageSize { get; set; }
        public PaginationFilter()
        {
            this.PageIndex = 0;
            this.PageSize = 10;
        }
        public PaginationFilter(int pageIndex, int pageSize)
        {
            this.PageIndex = pageIndex < 0 ? 0 : pageIndex;
            this.PageSize = pageSize > 100 ? 100 : pageSize;
        }
    }
}
