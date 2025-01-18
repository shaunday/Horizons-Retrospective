using HsR.Common;
using HsR.Journal.Entities;
using HsR.Journal.Entities.Factory;
using Microsoft.EntityFrameworkCore;

namespace HsR.Journal.DataContext
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
            var query = _dataContext.TradeComposites.AsNoTracking().AsQueryable().ApplyFiltering(filter);
            return await GetPaginatedTradesAsync(query, pageNumber, pageSize);
        }

        private static async Task<(IEnumerable<TradeComposite>, PaginationMetadata)> 
                                                    GetPaginatedTradesAsync(IQueryable<TradeComposite> query, int pageNumber, int pageSize)
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
            originElement.Entries = EntriesFactory.GetOriginEntries(originElement);
            trade.TradeElements.Add(originElement);

            _dataContext.TradeComposites.Add(trade);
            await _dataContext.SaveChangesAsync();
            return trade;
        }
    } 
}
