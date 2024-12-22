using JTA.Common;
using JTA.Journal.Entities;
using Microsoft.EntityFrameworkCore;

namespace JTA.Journal.DataContext
{
    public partial class TradeCompositeRepository : JournalRepositoryBase, ITradeCompositeRepository
    {
        public async Task<(IEnumerable<TradeComposite>, Pagination)> GetFilteredTradesAsync(
               TradesFilterModel filter, int pageNumber = 1, int pageSize = 10)
        {
            var query = _dataContext.TradeComposites.AsNoTracking().AsQueryable();

            if (filter.OpenLowerLimit.HasValue)
            {
                query = query.Where(t => t.OpenedAt >= filter.OpenLowerLimit.Value);
            }

            if (filter.OpenUpperLimit.HasValue)
            {
                query = query.Where(t => t.OpenedAt <= filter.OpenUpperLimit.Value);
            }

            if (filter.CloseLowerLimit.HasValue)
            {
                query = query.Where(t => t.OpenedAt >= filter.CloseLowerLimit.Value);
            }

            if (filter.CloseUpperLimit.HasValue)
            {
                query = query.Where(t => t.OpenedAt <= filter.CloseUpperLimit.Value);
            }

            if (filter.FilterObjects != null && filter.FilterObjects.Count != 0)
            {
                foreach (var filterObject in filter.FilterObjects)
                {
                    query = query.Where(t => t.TradeElements.Any(te => te.Entries.Any(e =>
                                    e.Title == filterObject.Title &&
                                     (e.Content == null && string.IsNullOrEmpty(filterObject.FilterValue) ||
                                      e.Content != null && e.Content.Contains(filterObject.FilterValue)))));
                }
            }

            var totalCount = await query.CountAsync();

            var trades = await ApplyPagination(query, pageNumber, pageSize).ToListAsync();

            var paginationMetadata = new Pagination(totalCount, pageSize, pageNumber)
            {
                TotalPageCount = (int)Math.Ceiling(totalCount / (double)pageSize)
            };

            return (trades, paginationMetadata);
        }
    } 
}
