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
                    new("Average Entry", ComponentType.Summary, analytics.AverageEntryPrice.ToF2String()) 
                                                                                   { IsRelevantForOverview = true },
                    new("Average Close", ComponentType.Summary, analytics.AverageExitPrice.ToF2String())
                                                                                   { IsRelevantForOverview = true },
                    new("Total Amount", ComponentType.Summary, analytics.NetAmount.ToF2String()) 
                                                                                   { IsRelevantForOverview = true },
                    new("Total Cost", ComponentType.Summary, analytics.NetCost.ToF2String()) 
                                                                                  { IsRelevantForOverview = true },
                };
            return summaryCells;
        }

        internal static List<DataElement> GetTradeClosureComponents(TradeAnalyticsSummary analytics)
        {
            var closureCells = new List<DataElement>
                {
                    new("Average Entry", ComponentType.Summary, analytics.AverageEntryPrice.ToF2String())
                                                                                 { IsRelevantForOverview = true },
                    new("Net Result", ComponentType.Summary, analytics.Profit.ToF2String()) { IsRelevantForOverview = true },
                    new("W/L", ComponentType.Summary, "") 
                                            { IsRelevantForOverview = true, Restrictions = ["W", "L"] },
                    new("Actual R:R", ComponentType.Summary, "") { IsRelevantForOverview = true },
                    new("Lessons", ComponentType.Summary, "")
                };

            return closureCells;
        }
    }
}
