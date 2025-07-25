using HsR.Common;
using HsR.Journal.DataContext;
using HsR.Journal.Entities;
using HsR.Journal.Entities.Factory;
using HsR.Journal.Entities.TradeJournal;
using HsR.Journal.TradeAnalytics;
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

        private protected TradeSummary RefreshSummary(TradeComposite trade)
        {
            if (TradeElementsFactory.GetNewElement(trade, TradeActionType.Summary) is not TradeSummary newSummary)
            {
                throw new InvalidOperationException("TradeElementsFactory returned an invalid type for Summary.");
            }

            if (trade.Summary != null)
            {
                _dataContext.Entry(trade.Summary).State = EntityState.Deleted;
            }

            if (!newSummary.IsInterim) //check if summary returned closure
            {
                trade.Close();
            }

            trade.Summary = newSummary;
            return newSummary;
        }

        private protected async Task<TradeComposite> GetTradeCompositeAsync(int tradeId)
        {
            var trade = await _dataContext.TradeComposites.FindAsync(tradeId)
                ?? throw new InvalidOperationException($"Trade with ID {tradeId} not found.");
            return trade!;
        }

        protected async Task<InterimTradeElement> GetTradeElementAsync(int tradeEleId, bool loadComposite = false)
        {
            var query = _dataContext.TradeElements.OfType<InterimTradeElement>().AsQueryable();
            if (loadComposite)
            {
                query = query.Include(te => te.CompositeRef);
            }
            var tradeElement = await query.FirstOrDefaultAsync(te => te.Id == tradeEleId)
                ?? throw new InvalidOperationException($"TradeElement with ID {tradeEleId} not found.");
            return tradeElement;
        }
    }
}
