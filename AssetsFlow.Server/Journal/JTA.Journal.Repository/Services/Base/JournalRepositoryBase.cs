using HsR.Common;
using HsR.Journal.DataContext;
using HsR.Journal.Entities;
using HsR.Journal.Entities.Factory;
using Microsoft.EntityFrameworkCore;

namespace HsR.Journal.Repository.Services.Base
{
    public abstract class JournalRepositoryBase
    {
        protected readonly TradingJournalDataContext _dataContext;

        protected JournalRepositoryBase(TradingJournalDataContext dataContext)
        {
            _dataContext = dataContext ?? throw new ArgumentNullException(nameof(dataContext));
        }

        protected TradeElement RefreshSummary(TradeComposite trade)
        {
            TradeElement newSummary = TradeElementCRUDs.GetInterimSummary(trade);

            // Remove the old Summary if it exists
            if (trade.Summary != null)
            {
                _dataContext.TradeElements.Remove(trade.Summary);
            }

            // Update trade status, if we got a closure as summary
            if (newSummary.TradeActionType == TradeActionType.Closure)
            {
                trade.Status = TradeStatus.Closed;
            }

            trade.Summary = newSummary;

            return newSummary;
        }

        protected async Task<TradeComposite> GetTradeCompositeAsync(string tradeId)
        {
            if (!int.TryParse(tradeId, out var parsedId))
            {
                throw new ArgumentException($"The tradeId '{tradeId}' is not a valid integer.", nameof(tradeId));
            }

            var trade = await _dataContext.TradeComposites
                                            .Where(t => t.Id == parsedId)
                                            .SingleOrDefaultAsync() ?? throw new InvalidOperationException($"Trade with ID {tradeId} not found.");
            return trade!;
        }

        protected async Task<TradeElement> GetTradeElementAsync(string tradeEleId)
        {
            if (!int.TryParse(tradeEleId, out var parsedId))
            {
                throw new ArgumentException($"The tradeElementId '{tradeEleId}' is not a valid integer.", nameof(tradeEleId));
            }

            var trade = await _dataContext.TradeElements
                                            .Where(t => t.Id == parsedId)
                                            .SingleOrDefaultAsync() ?? throw new InvalidOperationException($"TradeElement with ID {tradeEleId} not found.");
            return trade!;
        }
    }
}
