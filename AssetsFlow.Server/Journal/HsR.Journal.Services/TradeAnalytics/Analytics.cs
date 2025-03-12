using HsR.Journal.Entities;

namespace HsR.Journal.TradeAnalytics
{
    public static class Analytics
    {
        public static (TradeAnalyticsInfo addPositions, TradeAnalyticsInfo reducePositions) GetTradingCosts(TradeComposite trade)
        {
            Dictionary<TradeActionType, TradeAnalyticsInfo> positionsData = [];
            positionsData.Add(TradeActionType.Add, new TradeAnalyticsInfo());
            positionsData.Add(TradeActionType.Reduce, new TradeAnalyticsInfo());

            // Filter TradeElements based on action type
            var interims = trade.TradeElements
                .Where(t => t.TradeActionType == TradeActionType.Add || t.TradeActionType == TradeActionType.Reduce)
                .ToList();

            foreach (var tradeInput in interims)
            {
                double cost = 0.0;
                double price = 0.0;

                // Iterate over entries in each TradeElement
                foreach (var component in tradeInput.Entries)
                {
                    if (component.TotalCostRelevance != null)
                    {
                        double.TryParse(component.Content, out cost);
                    }

                    if (component.UnitPriceRelevance != null)
                    {
                        double.TryParse(component.Content, out price);
                    }

                    if (cost > 0 && price > 0)
                    {
                        positionsData[tradeInput.TradeActionType].TotalCost += cost;
                        positionsData[tradeInput.TradeActionType].TotalAmount += cost / price;
                        break;
                    }
                }
            }
            return (positionsData[TradeActionType.Add], positionsData[TradeActionType.Reduce]);
        }  
    }
}
