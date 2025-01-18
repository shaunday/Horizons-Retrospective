namespace HsR.Journal.Entities.Factory
{
    internal static class SummaryPositionsLists
    {
        internal static List<DataElement> GetSummaryComponents(string averageEntry, string totalAmount, string totalCost)
        {
            var summaryCells = new List<DataElement>
                {
                    new DataElement("Average Entry Price", ComponentType.InterimSummary, averageEntry),
                    new DataElement("Total Amount", ComponentType.InterimSummary, totalAmount),
                    new DataElement("Total Cost", ComponentType.InterimSummary, totalCost)
                };
            return summaryCells;
        }

        internal static List<DataElement> GetTradeClosureComponents(string? profitValue)
        {
            var closureCells = new List<DataElement>
                {
                    new DataElement("Result", ComponentType.Closure, profitValue ?? ""),
                    new DataElement("Actual R:R", ComponentType.Closure, ""),
                    new DataElement("W/L", ComponentType.Closure, ""),
                    new DataElement("Lessons", ComponentType.Closure, "")
                };

            return closureCells;
        }
    }
}
