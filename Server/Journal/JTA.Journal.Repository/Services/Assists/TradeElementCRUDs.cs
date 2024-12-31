using JTA.Journal.Entities;
using JTA.Journal.Entities.Factory;

namespace JTA.Journal.DataContext
{
    internal static class TradeElementCRUDs
    {
        internal static TradeElement CreateTradeElementForClosure(TradeComposite trade, string closingPrice)
        {
            // Create a TradeElement for ReducePosition
            var tradeInput = new TradeElement(trade, TradeActionType.ReducePosition);

            // Find price entry
            var priceEntry = tradeInput.Entries.SingleOrDefault(ti => ti.PriceRelevance == ValueRelevance.Negative);
            if (priceEntry == null)
            {
                throw new InvalidOperationException("Could not find price entry to reduce / close position");
            }
            priceEntry.ContentWrapper = new ContentRecord(closingPrice);

            // Find cost entry
            var costEntry = tradeInput.Entries.SingleOrDefault(ti => ti.CostRelevance == ValueRelevance.Negative);
            if (costEntry == null)
            {
                throw new InvalidOperationException("Could not find cost entry to reduce / close position");
            }

            var analytics = TradeAnalytics.GetAvgEntryAndProfit(trade);
            if (double.TryParse(closingPrice, out double closingPriceValue))
            {
                costEntry.ContentWrapper = new ContentRecord((closingPriceValue * analytics.totalAmount).ToString());
            }
            else
            {
                throw new FormatException("Could not parse closing price");
            }

            // Create TradeElement for Closure
            var tradeClosure = new TradeElement(trade, TradeActionType.Closure);
            tradeClosure.Entries = SummaryPositionsFactory.GetTradeClosureComponents(tradeClosure, profitValue: analytics.profit.ToString());

            return tradeInput; // Return tradeInput, as this is the entry we are adding
        }

        internal static TradeElement GetInterimSummary(TradeComposite trade)
        {
            var analytics = TradeAnalytics.GetAvgEntryAndProfit(trade);

            string averageEntry = string.Empty, totalAmount = string.Empty, totalCost = string.Empty;
            if (analytics.totalCost > 0)
            {
                totalCost = analytics.totalCost.ToString();

                if (analytics.totalAmount > 0)
                {
                    totalAmount = analytics.totalAmount.ToString();
                    averageEntry = (analytics.totalCost / analytics.totalAmount).ToString();
                }
            }

            TradeElement summary = new(trade, TradeActionType.InterimSummary);
            summary.Entries = SummaryPositionsFactory.GetSummaryComponents(summary, averageEntry, totalAmount, totalCost);

            return summary;
        }

        internal static void RemoveInterimInput(TradeComposite trade, string tradeInputId)
        {
            if (!int.TryParse(tradeInputId, out var parsedId))
            {
                throw new ArgumentException($"The element Id '{tradeInputId}' is not a valid integer.", nameof(tradeInputId));
            }

            var tradeInputToRemove = trade.TradeElements.Where(t => t.Id == parsedId).SingleOrDefault();

            if (tradeInputToRemove != null && (tradeInputToRemove.TradeActionType == TradeActionType.ReducePosition || tradeInputToRemove.TradeActionType == TradeActionType.ReducePosition))
            {
                trade.TradeElements.Remove(tradeInputToRemove);
            }
            else
            {
                throw new Exception("weird");
            } 
        }
    }
}
