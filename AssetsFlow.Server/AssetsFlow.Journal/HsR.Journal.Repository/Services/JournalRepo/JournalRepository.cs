using HsR.Common;
using HsR.Journal.Entities;
using HsR.Journal.Entities.Factory;
using HsR.Journal.Repository.Services.Base;
using HsR.Journal.Repository.Services.TradeCompositeRepo;
using HsR.Journal.Services;
using Microsoft.EntityFrameworkCore;

namespace HsR.Journal.DataContext
{
    public partial class JournalRepository(TradingJournalDataContext dataContext)
        : JournalRepositoryBase(dataContext), IJournalRepository
    {
        public async Task<(IEnumerable<TradeComposite>?, int totalTradesCount)> GetAllTradeCompositesAsync(int pageNumber = 1, int pageSize = 10)
        {
            var query = _dataContext.TradeComposites.AsNoTracking().AsQueryable();
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

        private static async Task<(IEnumerable<TradeComposite>?, int totalTradesCount)>
            GetPaginatedTradesAsync(IQueryable<TradeComposite> query, int pageNumber, int pageSize)
        {
            var totalCount = await query.CountAsync();

            var trades = await query.OrderBy(t => t.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (trades, totalCount);
        }
    }
}
