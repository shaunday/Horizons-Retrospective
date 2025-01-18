using HsR.Journal.Entities;
using HsR.Journal.Entities.Factory;

namespace HsR.Journal.DataContext
{
    internal static class TradeElementCRUDs
    {
        internal static TradeElement CreateTradeElementForClosure(TradeComposite trade, string closingPrice)
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
                var analytics = TradeAnalytics.GetTradeTotals(trade);
                costEntry.ContentWrapper = new ContentRecord((closingPriceValue * analytics.totalAmount).GetValueAsString());

                // Create TradeElement for Closure

                double profit = analytics.totalCost - (closingPriceValue * analytics.totalAmount);
                var tradeClosure = new TradeElement(trade, TradeActionType.Closure);
                tradeClosure.Entries = EntriesFactory.GetTradeClosureComponents(tradeClosure, profit.GetValueAsString());
            }
            else
            {
                throw new FormatException("Could not parse closing price");
            }            

            return tradeInput; // Return tradeInput, as this is the entry we are adding
        }

        internal static TradeElement CreateInterimTradeElement(TradeComposite trade, bool isAdd)
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


        internal static TradeElement GetInterimSummary(TradeComposite trade)
        {
            var analytics = TradeAnalytics.GetTradeTotals(trade);

            string averageEntry = string.Empty, totalAmount = string.Empty, totalCost = string.Empty;
            if (analytics.totalCost > 0)
            {
                totalCost = analytics.totalCost.GetValueAsString();

                if (analytics.totalAmount > 0)
                {
                    totalAmount = analytics.totalAmount.GetValueAsString();
                    averageEntry = (analytics.totalCost / analytics.totalAmount).GetValueAsString();
                }
            }

            TradeElement summary = new(trade, TradeActionType.InterimSummary);
            summary.Entries = EntriesFactory.GetSummaryComponents(summary, averageEntry, totalAmount, totalCost);

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

        private static string GetValueAsString(this double value)
        {
            return value.ToString("F2");
        }
    }
}
