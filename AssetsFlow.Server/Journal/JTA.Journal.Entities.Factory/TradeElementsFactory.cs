using HsR.Common.Extenders;
using HsR.Journal.TradeAnalytics;

namespace HsR.Journal.Entities.Factory
{
    public static class TradeElementsFactory
    {
        public static TradeElement GetNewInterimTradeElement(TradeComposite trade, bool isAdd)
        {
            TradeElement tradeInput = new(trade, isAdd ? TradeActionType.AddPosition : TradeActionType.ReducePosition);
            if (isAdd)
            {
                if (trade.Status == TradeStatus.AnIdea)
                {
                    tradeInput.Entries = EntriesFactory.GetFirstPositionEntries(tradeInput);
                }
                else
                {
                    tradeInput.Entries = EntriesFactory.GetAddPositionEntries(tradeInput);
                }
            }
            else
            {
                tradeInput.Entries = EntriesFactory.GetReducePositionEntries(tradeInput);
            }
            return tradeInput;
        }

        public static TradeElement GetNewSummary(TradeComposite trade)
        {
            var analytics = Analytics.GetTradingCosts(trade);
            TradeAnalyticsSummary analyticsSummary = new(analytics);

            TradeElement newSummary;
            if (analyticsSummary.NetAmount == 0.0)
            {
                newSummary = new TradeElement(trade, TradeActionType.Closure);
                newSummary.Entries = EntriesFactory.GetTradeClosureComponents(newSummary, analyticsSummary);
            }
            else
            {
                newSummary = new(trade, TradeActionType.InterimSummary);
                newSummary.Entries = EntriesFactory.GetSummaryComponents(newSummary, analyticsSummary);
            }

            return newSummary;
        } 
    }
}
