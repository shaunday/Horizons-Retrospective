using HsR.Common;
using HsR.Journal.Entities;
using HsR.Journal.Entities.Factory;
using HsR.Journal.Repository.Services.Base;
using HsR.Journal.Repository.Services.TradeCompositeRepo;
using Microsoft.EntityFrameworkCore;

namespace HsR.Journal.DataContext
{
    public partial class TradeCompositesRepository(TradingJournalDataContext dataContext) 
                                                : JournalRepositoryBase(dataContext), ITradeCompositesRepository
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

      
        public async Task<TradeComposite> AddTradeCompositeAsync()
        {
            TradeComposite trade = new();
            InterimTradeElement originElement = new(trade, TradeActionType.Origin);
            originElement.Entries = EntriesFactory.GetOriginEntries(originElement);
            trade.TradeElements.Add(originElement);

            _dataContext.TradeComposites.Add(trade);
            await _dataContext.SaveChangesAsync();
            return trade;
        }

        public async Task<TradeElement> CloseTradeAsync(string tradeId, string closingPrice)
        {
            var trade = await GetTradeCompositeAsync(tradeId);
            if (trade.Summary != null)
            {
                _dataContext.Entry(trade.Summary).State = EntityState.Deleted;
            }
            else
            {
                throw new InvalidOperationException("Trade summary is missing.");
            }
            TradeCompositeOperations.CloseTrade(trade, closingPrice);

            await _dataContext.SaveChangesAsync();
            return trade.Summary!;
        }


        //helper

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

    }
}
