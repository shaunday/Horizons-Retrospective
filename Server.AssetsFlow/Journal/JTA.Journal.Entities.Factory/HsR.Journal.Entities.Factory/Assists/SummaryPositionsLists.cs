namespace HsR.Journal.Entities.Factory
{
    internal static class SummaryPositionsLists
    {
        internal static List<DataElement> GetSummaryComponents(string averageEntry, string totalAmount, string totalCost)
        {
            var summaryCells = new List<DataElement>
                {
                    new DataElement("Average Entry Price", ComponentType.InterimSummary, averageEntry) {IsRelevantForOverview = true },
                    new DataElement("Total Amount", ComponentType.InterimSummary, totalAmount) { IsRelevantForOverview = true },
                    new DataElement("Total Cost", ComponentType.InterimSummary, totalCost) { IsRelevantForOverview = true }
                };
            return summaryCells;
        }

        internal static List<DataElement> GetTradeClosureComponents(string? profitValue)
        {
            var closureCells = new List<DataElement>
                {
                    new DataElement("Result", ComponentType.Closure, profitValue ?? "") {IsRelevantForOverview = true },
                    new DataElement("Actual R:R", ComponentType.Closure, "") {IsRelevantForOverview = true },
                    new DataElement("W/L", ComponentType.Closure, "") { IsRelevantForOverview = true },
                    new DataElement("Lessons", ComponentType.Closure, "")
                };

            return closureCells;
        }
    }
}
