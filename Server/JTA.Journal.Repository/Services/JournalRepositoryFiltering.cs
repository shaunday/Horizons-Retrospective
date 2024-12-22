using DayJT.Journal.Data;
using DayJT.Journal.DataContext.Services;
using DayJT.Journal.DataEntities.Entities;
using DayJTrading.Journal.Data;
using JTA.Journal.Entities;
using Microsoft.EntityFrameworkCore;

namespace DayJT.Journal.Repository.Services
{
    public partial class JournalRepository: IJournalRepository
    {
        public async Task<(IEnumerable<TradeComposite>, PaginationMetadata)> GetFilteredTradeCompositesAsync(
            TradesFilterModel filter, int pageNumber = 1, int pageSize = 10)
        {
            var query = dataContext.TradeComposites.AsNoTracking().AsQueryable();

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

            var trades = await query
                                .OrderBy(t => t.Id)
                                .Skip((pageNumber - 1) * pageSize)
                                .Take(pageSize)
                                .ToListAsync();

            var paginationMetadata = new PaginationMetadata(totalCount, pageSize, pageNumber)
            {
                TotalPageCount = (int)Math.Ceiling(totalCount / (double)pageSize)
            };

            return (trades, paginationMetadata);
        }
        
    }
}
