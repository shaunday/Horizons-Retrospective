using HsR.Journal.Entities;

namespace HsR.Journal.DataContext
{
    internal static class TradeAnalytics
    {
        internal static (double totalCost, double totalAmount, double profit) GetAvgEntryAndProfit(TradeComposite trade)
        {
            List<(double priceValue, double cost)> entriesWithAmount = [];
            double profit = 0.0;

            var interims = trade.TradeElements
                .Where(t => t.TradeActionType == TradeActionType.AddPosition || t.TradeActionType == TradeActionType.ReducePosition)
                .ToList();

            foreach (var tradeInput in interims)
            {
                double cost = 0.0;
                double priceValue = 0.0;

                foreach (var component in tradeInput.Entries)
                {
                    (cost, profit) = ProcessCostAndProfit(component, cost, profit);
                    priceValue = ProcessPrice(component, priceValue);
                }

                // Should have both cost and price now
                if (cost > 0 && priceValue > 0)
                {
                    entriesWithAmount.Add((priceValue, cost));
                }
            }

            var (totalCost, totalAmount) = CalculateTotal(entriesWithAmount);

            return (totalCost, totalAmount, profit);
        }

        // Helper method to process the cost and profit
        private static (double cost, double profit) ProcessCostAndProfit(DataElement component, double cost, double profit)
        {
            if (component.CostRelevance == ValueRelevance.Positive || component.CostRelevance == ValueRelevance.Negative)
            {
                double.TryParse(component.Content, out cost);

                if (component.CostRelevance == ValueRelevance.Positive)
                {
                    profit += cost;
                }
                else if (component.CostRelevance == ValueRelevance.Negative)
                {
                    profit -= cost;
                }
            }

            return (cost, profit);
        }

        // Helper method to process the price
        private static double ProcessPrice(DataElement component, double priceValue)
        {
            if (component.PriceRelevance == ValueRelevance.Positive || component.PriceRelevance == ValueRelevance.Negative)
            {
                double.TryParse(component.Content, out priceValue);

                if (component.PriceRelevance == ValueRelevance.Negative)
                {
                    priceValue *= -1;
                }
            }

            return priceValue;
        }

        // Helper method to calculate the total cost and amount
        private static (double totalCost, double totalAmount) CalculateTotal(List<(double priceValue, double cost)> entriesWithAmount)
        {
            double totalAmount = 0.0;
            double totalCost = 0.0;

            foreach (var item in entriesWithAmount)
            {
                totalCost += item.cost * item.priceValue;
                totalAmount += item.cost / item.priceValue;
            }

            return (totalCost, totalAmount);
        }

    }
}
