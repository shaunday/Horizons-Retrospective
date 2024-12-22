using JTA.Common;
using JTA.Journal.Entities;
using Microsoft.EntityFrameworkCore;

namespace JTA.Journal.DataContext
{
    public partial class TradeCompositeRepository(TradingJournalDataContext dataContext) : JournalRepositoryBase(dataContext), ITradeCompositeRepository
    {
        public async Task<(IEnumerable<TradeComposite>, Pagination)> GetAllTradeCompositesAsync(int pageNumber = 1, int pageSize = 10)
        {
            var trades = await _dataContext.TradeComposites
                                            .AsNoTracking()
                                            .OrderBy(t => t.Id)
                                            .Skip((pageNumber - 1) * pageSize)
                                            .Take(pageSize)
                                            .ToListAsync();

            if (trades == null || trades.Count == 0)
            {
                throw new InvalidOperationException("Could not get any trades.");
            }

            var totalCount = await _dataContext.TradeComposites.CountAsync();
            var paginationMetadata = new Pagination(totalCount, pageSize, pageNumber)
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

        private IQueryable<TradeComposite> ApplyPagination(IQueryable<TradeComposite> query, int pageNumber, int pageSize)
        {
            return query.OrderBy(t => t.Id)
                        .Skip((pageNumber - 1) * pageSize)
                        .Take(pageSize);
        }
    } 
}
