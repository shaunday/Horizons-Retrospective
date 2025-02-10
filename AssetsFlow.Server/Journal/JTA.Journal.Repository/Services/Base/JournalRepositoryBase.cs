using HsR.Common;
using HsR.Journal.DataContext;
using HsR.Journal.Entities;
using HsR.Journal.Entities.Factory;
using Microsoft.EntityFrameworkCore;

namespace HsR.Journal.Repository.Services.Base
{
    public abstract class JournalRepositoryBase
    {
        private protected readonly TradingJournalDataContext _dataContext;

        private protected JournalRepositoryBase(TradingJournalDataContext dataContext)
        {
            _dataContext = dataContext ?? throw new ArgumentNullException(nameof(dataContext));
        }

        private protected TradeElement RefreshSummary(TradeComposite trade)
        {
            TradeElement newSummary = TradeElementsFactory.GetNewSummary(trade);

            // Remove the old Summary if it exists
            if (trade.Summary != null)
            {
                _dataContext.Entry(trade.Summary).State = EntityState.Deleted;
            }

            // Update trade status, if we got a closure as summary
            if (newSummary.TradeActionType == TradeActionType.Closure)
            {
                trade.Status = TradeStatus.Closed;
            }

            trade.Summary = newSummary;

            return newSummary;
        }

        private protected async Task<TradeComposite> GetTradeCompositeAsync(string tradeId)
        {
            if (!int.TryParse(tradeId, out var parsedId))
            {
                throw new ArgumentException($"The tradeId '{tradeId}' is not a valid integer.", nameof(tradeId));
            }

            var trade = await _dataContext.TradeComposites.FindAsync(parsedId)
                ?? throw new InvalidOperationException($"Trade with ID {tradeId} not found.");

            return trade!;
        }

        protected async Task<TradeElement> GetTradeElementAsync(string tradeEleId)
        {
            if (!int.TryParse(tradeEleId, out var parsedId))
            {
                throw new ArgumentException($"The tradeElementId '{tradeEleId}' is not a valid integer.", nameof(tradeEleId));
            }

            var tradeElement = await _dataContext.TradeElements.FindAsync(parsedId)
                 ?? throw new InvalidOperationException($"TradeElement with ID {tradeEleId} not found.");

            return tradeElement!;
        }
    }
}
