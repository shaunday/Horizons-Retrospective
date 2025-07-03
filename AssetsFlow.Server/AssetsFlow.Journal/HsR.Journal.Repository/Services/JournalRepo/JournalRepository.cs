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
        public async Task<(IEnumerable<TradeComposite>?, int totalTradesCount)> GetAllTradeCompositesAsync(Guid userId, int pageNumber = 1, int pageSize = 10)
        {
            var query = _dataContext.TradeComposites
                .AsNoTracking()
                .Where(tc => tc.UserId == userId)
                .AsQueryable();
            return await GetPaginatedTradesAsync(query, pageNumber, pageSize);
        }

        public async Task<TradeComposite> AddTradeCompositeAsync(Guid userId)
        {
            TradeComposite trade = new() { UserId = userId };
            if (TradeElementsFactory.GetNewElement(trade, TradeActionType.Origin) is not InterimTradeElement originElement)
            {
                throw new InvalidOperationException("TradeElementsFactory returned an invalid type for InterimTradeElement.");
            }

            trade.TradeElements.Add(originElement);

            _dataContext.TradeComposites.Add(trade);
            await _dataContext.SaveChangesAsync();
            return trade;
        }

        public async Task<TradeComposite?> GetTradeCompositeByIdAsync(int tradeId)
        {
            return await _dataContext.TradeComposites
                .Include(tc => tc.TradeElements)
                    .ThenInclude(te => te.Entries)
                .Include(tc => tc.Summary)
                .FirstOrDefaultAsync(tc => tc.Id == tradeId);
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
