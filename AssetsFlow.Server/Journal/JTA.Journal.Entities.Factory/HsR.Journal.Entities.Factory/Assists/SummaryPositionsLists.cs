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
                    new("Average Entry", ComponentType.InterimSummary, analytics.AverageEntryPrice.ToF2String()) 
                                                                                   { IsRelevantForOverview = true },
                    new("Average Close", ComponentType.InterimSummary, analytics.AverageExitPrice.ToF2String())
                                                                                   { IsRelevantForOverview = true },
                    new("Total Amount", ComponentType.InterimSummary, analytics.NetAmount.ToF2String()) 
                                                                                   { IsRelevantForOverview = true },
                    new("Total Cost", ComponentType.InterimSummary, analytics.NetCost.ToF2String()) 
                                                                                  { IsRelevantForOverview = true },
                };
            return summaryCells;
        }

        internal static List<DataElement> GetTradeClosureComponents(TradeAnalyticsSummary analytics)
        {
            var closureCells = new List<DataElement>
                {
                    new("Average Entry", ComponentType.Closure, analytics.AverageEntryPrice.ToF2String())
                                                                                 { IsRelevantForOverview = true },
                    new("Net Result", ComponentType.Closure, analytics.Profit.ToF2String()) { IsRelevantForOverview = true },
                    new("W/L", ComponentType.Closure, "") { IsRelevantForOverview = true },
                    new("Actual R:R", ComponentType.Closure, "") { IsRelevantForOverview = true },
                    new("Lessons", ComponentType.Closure, "")
                };

            return closureCells;
        }
    }
}
