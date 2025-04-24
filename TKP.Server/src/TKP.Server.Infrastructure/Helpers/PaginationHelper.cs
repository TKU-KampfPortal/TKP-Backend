using Microsoft.EntityFrameworkCore;
using TKP.Server.Core.Entities;

namespace TKP.Server.Infrastructure.Helpers
{
    public static class PaginationHelper
    {
        /// <summary>
        /// Applies pagination to an IQueryable and returns a PaginationResponse.
        /// </summary>
        /// <typeparam name="T">Entity type.</typeparam>
        /// <param name="query">The base query to paginate.</param>
        /// <param name="pageIndex">Zero-based page index.</param>
        /// <param name="pageSize">Number of items per page.</param>
        /// <param name="cancellationToken">Cancelation token for paginated method.</param>
        /// <returns>A paginated response containing the items and metadata.</returns>
        public static async Task<PaginationResponse<T>> ToPaginatedListAsync<T>(
            this IQueryable<T> query, int pageIndex, int pageSize, CancellationToken cancellationToken = default)
        {
            var totalItems = await query.CountAsync();

            var response = new PaginationResponse<T>
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalCount = totalItems,
                Items = Enumerable.Empty<T>()
            };

            if (totalItems > 0)
            {
                response.Items = await query
                    .Skip(pageIndex * pageSize)
                    .Take(pageSize)
                    .ToListAsync(cancellationToken);
            }

            return response;
        }
    }

}
