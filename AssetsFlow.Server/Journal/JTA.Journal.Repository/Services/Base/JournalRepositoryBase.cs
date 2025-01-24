using HsR.Common;
using HsR.Journal.DataContext;
using HsR.Journal.Entities;
using HsR.Journal.Entities.Factory;

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

            trade.Summary = newSummary;

            return newSummary;
        }
    }
}
