using HsR.Common.Extenders;
using HsR.Journal.TradeAnalytics;

namespace HsR.Journal.Entities.Factory
{
    internal static class SummaryPositionsLists
    {
        internal static List<DataElement> GetSummaryComponents(TradeAnalyticsSummary analytics)
        {
            var summaryCells = new List<DataElement>
                {
                    new DataElement("Average Entry", ComponentType.InterimSummary, analytics.AverageEntryPrice.ToF2String()) 
                                                                                                    {IsRelevantForOverview = true },
                    new DataElement("Average Close", ComponentType.InterimSummary, analytics.AverageExitPrice.ToF2String())
                                                                                                    {IsRelevantForOverview = true },
                    new DataElement("Total Amount", ComponentType.InterimSummary, analytics.NetAmount.ToF2String()) 
                                                                                                    { IsRelevantForOverview = true } ,
                    new DataElement("Total Cost", ComponentType.InterimSummary, analytics.NetCost.ToF2String()) 
                                                                                                    { IsRelevantForOverview = true }
                };
            return summaryCells;
        }

        internal static List<DataElement> GetTradeClosureComponents(TradeAnalyticsSummary analytics)
        {
            var closureCells = new List<DataElement>
                {
                    new DataElement("Average Entry", ComponentType.InterimSummary, analytics.AverageEntryPrice.ToF2String())
                                                                                                    {IsRelevantForOverview = true },

                    new DataElement("Net Result", ComponentType.Closure, analytics.Profit.ToF2String()) {IsRelevantForOverview = true },
                    new DataElement("W/L", ComponentType.Closure, "") { IsRelevantForOverview = true },
                    new DataElement("Actual R:R", ComponentType.Closure, "") {IsRelevantForOverview = true },
                    new DataElement("Lessons", ComponentType.Closure, "")
                };

            return closureCells;
        }
    }
}
