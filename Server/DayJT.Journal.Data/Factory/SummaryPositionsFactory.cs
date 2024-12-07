using DayJT.Journal.Data;
using DayJT.Journal.DataEntities.Entities;
using DayJT.Journal.DataEntities.Factory;

namespace DayJTrading.Journal.Data
{
    public static class SummaryPositionsFactory
    {

        public static List<Cell> GetSummaryComponents(TradeElement elementRef, string averageEntry, string totalAmount, string totalCost)
        {
            var summaryCells = new List<(string Title, ComponentType Type, string Content)>
            {
                ("Average Entry Price", ComponentType.InterimSummary, averageEntry),
                ("Total Amount", ComponentType.InterimSummary, totalAmount),
                ("Total Cost", ComponentType.InterimSummary, totalCost)
            };
            return EntriesFactory.CreateCells(summaryCells, elementRef);
        }

        public static List<Cell> GetTradeClosureComponents(TradeElement elementRef, string? profitValue)
        {
            var closureCells = new List<(string Title, ComponentType Type, string Content)>
            {
                ("Result", ComponentType.Closure, profitValue ?? ""),
                ("Actual R:R", ComponentType.Closure, ""),
                ("W/L", ComponentType.Closure, ""),
                ("Lessons", ComponentType.Closure, "")
            };

            return EntriesFactory.CreateCells(closureCells, elementRef);
        }
    }
}
