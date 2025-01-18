using HsR.Journal.Entities;
using Microsoft.Extensions.Hosting;

namespace HsR.Journal.DataContext
{
    internal static class TradeAnalytics
    {
        internal static (double totalCost, double totalAmount) GetTradeTotals(TradeComposite trade)
        {
            List<(double priceValue, double cost)> entriesWithAmount = new List<(double priceValue, double cost)>();

            // Filter TradeElements based on action type
            var interims = trade.TradeElements
                .Where(t => t.TradeActionType == TradeActionType.AddPosition || t.TradeActionType == TradeActionType.ReducePosition)
                .ToList();

            foreach (var tradeInput in interims)
            {
                double costValue = 0.0;
                double priceValue = 0.0;

                // Iterate over entries in each TradeElement
                foreach (var component in tradeInput.Entries)
                {
                    costValue += ProcessCost(component);
                    priceValue += ProcessPrice(component);

                    if (costValue > 0 && priceValue > 0)
                    {
                        entriesWithAmount.Add((priceValue, costValue));
                        break;
                    }
                }
            }

            // Calculate total cost and amount from all valid entries
            var (totalCost, totalAmount) = CalculateTotal(entriesWithAmount);

            return (totalCost, totalAmount);
        }

        private static double ProcessCost(DataElement component)
        {
            double cost = 0.0;
            if (component.TotalCostRelevance != null)
            {
                // Try to parse the cost from the component's content
                double.TryParse(component.Content, out cost);

                if (component.TotalCostRelevance == ValueRelevance.Negative)
                {
                    cost *= -1; ;
                }
            }

            return cost;
        }

        private static double ProcessPrice(DataElement component)
        {
            double price = 0.0;
            if (component.UnitPriceRelevance != null)
            {
                // Try to parse the price from the component's content
                double.TryParse(component.Content, out price);
            }

            return price;
        }

        private static (double totalCost, double totalAmount) CalculateTotal(List<(double priceValue, double cost)> entriesWithAmount)
        {
            double totalAmount = 0.0;
            double totalCost = 0.0;

            // Sum the cost and amount for each valid entry
            foreach (var item in entriesWithAmount)
            {
                totalCost += item.cost * item.priceValue;
                totalAmount += item.cost / item.priceValue;
            }

            return (totalCost, totalAmount);
        }
    }
}
