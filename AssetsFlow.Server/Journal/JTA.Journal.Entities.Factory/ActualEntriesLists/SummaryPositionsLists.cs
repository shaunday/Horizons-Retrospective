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
                    new("Average Entry", ComponentType.PriceRelated, analytics.AverageEntryPrice.ToF2String())
                                                                                   {  IsRelevantForLocalOverview = true, IsRelevantForTradeOverview = true },
                    new("Average Close", ComponentType.PriceRelated, analytics.AverageExitPrice.ToF2String())
                                                                                   {  IsRelevantForLocalOverview = true, IsRelevantForTradeOverview = true },
                    new("Total Amount", ComponentType.PriceRelated, analytics.NetAmount.ToF2String())
                                                                                   {  IsRelevantForLocalOverview = true, IsRelevantForTradeOverview = true },
                    new("Total Cost", ComponentType.PriceRelated, analytics.NetCost.ToF2String())
                                                                                  {  IsRelevantForLocalOverview = true, IsRelevantForTradeOverview = true },
                };
            return summaryCells;
        }

        internal static List<DataElement> GetTradeClosureComponents(TradeAnalyticsSummary analytics)
        {
            var closureCells = new List<DataElement>
                {
                    new("Average Entry", ComponentType.PriceRelated, analytics.AverageEntryPrice.ToF2String())
                                                                                 {  IsRelevantForLocalOverview = true, IsRelevantForTradeOverview = true },
                    new("Net Result", ComponentType.Results, analytics.Profit.ToF2String()) {  IsRelevantForLocalOverview = true, IsRelevantForTradeOverview = true },
                    new("W/L", ComponentType.Results, "")
                                            {  IsRelevantForLocalOverview = true, IsRelevantForTradeOverview = true, Restrictions = ["W", "L"] },
                    new("Actual R:R", ComponentType.Results, "") {  IsRelevantForLocalOverview = true, IsRelevantForTradeOverview = true },
                    new("Lessons", ComponentType.Thoughts, "")
                };

            return closureCells;
        }
    }
}
