using HsR.Common.Extenders;
using HsR.Journal.TradeAnalytics;

namespace HsR.Journal.Entities.Factory
{
    public static class TradeElementCRUDs
    {
        public static TradeElement CreateInterimTradeElement(TradeComposite trade, bool isAdd)
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

        public static TradeElement GetInterimSummary(TradeComposite trade)
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

        public static void RemoveInterimInputById(TradeComposite trade, string tradeInputId)
        {
            if (!int.TryParse(tradeInputId, out var parsedId))
            {
                throw new ArgumentException($"The element Id '{tradeInputId}' is not a valid integer.", nameof(tradeInputId));
            }

            var tradeInputToRemove = trade.TradeElements.Where(t => t.Id == parsedId).SingleOrDefault();

            if (tradeInputToRemove != null)
            {
                trade.TradeElements.Remove(tradeInputToRemove);
            }
            else
            {
                throw new ArgumentException($"The trade input (Id '{tradeInputId}') to remove is null.", nameof(tradeInputId));

            }
        }
    }
}
