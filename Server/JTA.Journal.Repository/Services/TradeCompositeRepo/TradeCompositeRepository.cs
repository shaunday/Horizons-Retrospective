using JTA.Common;
using JTA.Journal.Entities;
using Microsoft.EntityFrameworkCore;

namespace JTA.Journal.DataContext
{
    public partial class TradeCompositeRepository(TradingJournalDataContext dataContext) : JournalRepositoryBase(dataContext), ITradeCompositeRepository
    {
        public async Task<(IEnumerable<TradeComposite>, PaginationMetadata)> GetAllTradeCompositesAsync(int pageNumber = 1, int pageSize = 10)
        {
            var query = _dataContext.TradeComposites.AsNoTracking().AsQueryable();

            return await GetPaginatedTradesAsync(query, pageNumber, pageSize);
        }

        public async Task<(IEnumerable<TradeComposite>, PaginationMetadata)> GetFilteredTradesAsync(
            TradesFilterModel filter, int pageNumber = 1, int pageSize = 10)
        {
            var query = _dataContext.TradeComposites.AsNoTracking().AsQueryable();

            query = query.ApplyFiltering(filter);

            return await GetPaginatedTradesAsync(query, pageNumber, pageSize);
        }

        private async Task<(IEnumerable<TradeComposite>, PaginationMetadata)> GetPaginatedTradesAsync(IQueryable<TradeComposite> query, 
                                                                                                                int pageNumber, int pageSize)
        {
            var totalCount = await query.CountAsync();

            var trades = await query.OrderBy(t => t.Id)
                        .Skip((pageNumber - 1) * pageSize)
                        .Take(pageSize)
                        .ToListAsync();

            var paginationMetadata = new PaginationMetadata(totalCount, pageSize, pageNumber)
            {
                TotalPageCount = (int)Math.Ceiling(totalCount / (double)pageSize)
            };

            return (trades, paginationMetadata);
        }


        public async Task<TradeComposite> AddTradeCompositeAsync()
        {
            TradeComposite trade = new();
            TradeElement originElement = new(trade, TradeActionType.Origin);
            trade.TradeElements.Add(originElement);

            _dataContext.TradeComposites.Add(trade);
            await _dataContext.SaveChangesAsync();
            return trade;
        }
    } 
}
