using HsR.Common.Extenders;
using HsR.Journal.TradeAnalytics;

namespace HsR.Journal.Entities.Factory
{
    public static class TradeElementCRUDs
    {
        public static TradeElement CreateTradeElementForClosure(TradeComposite trade, string closingPrice)
        {
            // Create a TradeElement for ReducePosition
            var tradeInput = new TradeElement(trade, TradeActionType.ReducePosition);
            tradeInput.Entries = EntriesFactory.GetReducePositionEntries(tradeInput);

            // Find price entry
            var priceEntry = tradeInput.Entries.SingleOrDefault(ti => ti.UnitPriceRelevance == ValueRelevance.Negative);
            if (priceEntry == null)
            {
                throw new InvalidOperationException("Could not find price entry to reduce / close position");
            }
            priceEntry.ContentWrapper = new ContentRecord(closingPrice);

            // Find cost entry
            var costEntry = tradeInput.Entries.SingleOrDefault(ti => ti.TotalCostRelevance == ValueRelevance.Negative);
            if (costEntry == null)
            {
                throw new InvalidOperationException("Could not find cost entry to reduce / close position");
            }

            if (double.TryParse(closingPrice, out double closingPriceValue))
            {
                var analytics = Analytics.GetTradingCosts(trade);
                double netAmountInPosition = analytics.addPositions.TotalAmount - analytics.reducePositions.TotalAmount;
                double costToClose = closingPriceValue * netAmountInPosition;
                costEntry.ContentWrapper = new ContentRecord(costToClose.ToF2String());

                // Create TradeElement for Closure

                analytics.reducePositions.TotalCost += costToClose;
                analytics.reducePositions.TotalAmount += netAmountInPosition;

                var tradeClosure = new TradeElement(trade, TradeActionType.Closure);
                TradeAnalyticsSummary analyticsSummary = new(analytics);
                tradeClosure.Entries = EntriesFactory.GetTradeClosureComponents(tradeClosure, analyticsSummary);
            }
            else
            {
                throw new FormatException("Could not parse closing price");
            }

            return tradeInput; // Return tradeInput, as this is the entry we are adding
        }

        public static TradeElement CreateInterimTradeElement(TradeComposite trade, bool isAdd)
        {
            TradeElement tradeInput = new(trade, isAdd ? TradeActionType.AddPosition : TradeActionType.ReducePosition);
            if (isAdd)
            {
                tradeInput.Entries = EntriesFactory.GetAddPositionEntries(tradeInput);
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

            TradeElement summary = new(trade, TradeActionType.InterimSummary);
            summary.Entries = EntriesFactory.GetSummaryComponents(summary, analyticsSummary);

            return summary;
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
