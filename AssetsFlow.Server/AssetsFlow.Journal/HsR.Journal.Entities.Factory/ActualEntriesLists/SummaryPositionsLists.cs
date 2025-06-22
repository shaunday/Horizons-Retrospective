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
                                                                                   {  IsRelevantForTradeOverview = true },
                    new("Average Close", ComponentType.PriceRelated, analytics.AverageExitPrice.ToF2String())
                                                                                   {  IsRelevantForTradeOverview = true },
                    new("Total Amount", ComponentType.PriceRelated, analytics.NetAmount.ToF2String())
                                                                                   {  IsRelevantForTradeOverview = true },
                };
            return summaryCells;
        }

        internal static List<DataElement> GetTradeClosureComponents(TradeAnalyticsSummary analytics)
        {
            var closureCells = new List<DataElement>
                {
                    new("Net Result", ComponentType.Results, analytics.Profit.ToF2String()) {  IsRelevantForTradeOverview = true },
                    new("W/L", ComponentType.Results, analytics.IsWin ? "W" : "L") {  IsRelevantForTradeOverview = true, },
                    //new("Actual R:R", ComponentType.Results, "") {  IsRelevantForTradeOverview = true },
                    new("Lessons", ComponentType.Thoughts, "")
                };

            return closureCells;
        }
    }
}
