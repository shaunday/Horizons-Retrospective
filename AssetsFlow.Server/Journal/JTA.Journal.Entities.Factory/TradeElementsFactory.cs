using HsR.Common.Extenders;
using HsR.Journal.Entities.TradeJournal;
using HsR.Journal.TradeAnalytics;

namespace HsR.Journal.Entities.Factory
{
    public static class TradeElementsFactory
    {
        public static InterimTradeElement GetNewInterimTradeElement(TradeComposite trade, bool isAdd)
        {
            InterimTradeElement tradeInput = new(trade, isAdd ? TradeActionType.AddPosition : TradeActionType.ReducePosition);
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

        public static (TradeSummary, bool shouldBeClosed) GetNewSummary(TradeComposite trade)
        {
            var analytics = Analytics.GetTradingCosts(trade);
            TradeAnalyticsSummary analyticsSummary = new(analytics);

            TradeSummary newSummary = new(trade, TradeActionType.Summary);
            bool shouldBeClosed = analyticsSummary.NetAmount == 0.0;
            if (shouldBeClosed)
            {
                newSummary.Entries = EntriesFactory.GetTradeClosureComponents(newSummary, analyticsSummary);
            }
            else
            {
                newSummary.Entries = EntriesFactory.GetSummaryComponents(newSummary, analyticsSummary);
            }

            return (newSummary, shouldBeClosed);
        }

        public static InterimTradeElement GetNewEvalutationElement(TradeComposite trade)
        {
            InterimTradeElement tradeOverview = new(trade, TradeActionType.Overview);
            tradeOverview.Entries = EntriesFactory.GetEvalutationEntries(tradeOverview);
            return tradeOverview;
        }
    }
}
