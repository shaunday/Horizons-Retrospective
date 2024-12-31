namespace JTA.Journal.Entities.Factory
{
    public static class SummaryPositionsFactory
    {

        public static List<DataElement> GetSummaryComponents(TradeElement elementRef, string averageEntry, string totalAmount, string totalCost)
        {
            var summaryCells = new List<EntryOverview>
                {
                    new EntryOverview("Average Entry Price", ComponentType.InterimSummary, averageEntry),
                    new EntryOverview("Total Amount", ComponentType.InterimSummary, totalAmount),
                    new EntryOverview("Total Cost", ComponentType.InterimSummary, totalCost)
                };
            return EntriesFactory.CreateEntries(summaryCells, elementRef);
        }

        public static List<DataElement> GetTradeClosureComponents(TradeElement elementRef, string? profitValue)
        {
            var closureCells = new List<EntryOverview>
                {
                    new EntryOverview("Result", ComponentType.Closure, profitValue ?? ""),
                    new EntryOverview("Actual R:R", ComponentType.Closure, ""),
                    new EntryOverview("W/L", ComponentType.Closure, ""),
                    new EntryOverview("Lessons", ComponentType.Closure, "")
                };

            return EntriesFactory.CreateEntries(closureCells, elementRef);
        }
    }
}
