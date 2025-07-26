using HsR.Journal.Entities.TradeJournal;
using HsR.Journal.TradeAnalytics;

namespace HsR.Journal.Entities.Factory
{
    public static partial class TradeElementsFactory
    {
        #region Origin
        private static List<DataElement> GetOriginEntries(InterimTradeElement elementRef)
        {
            return CreateEntries(GetTradeOriginObjects(), elementRef);
        }
        #endregion

        #region Interim elements
        private static List<DataElement> GetAddPositionEntries(InterimTradeElement elementRef)
        {
            return CreateEntries(GetAddToPositionObjects(), elementRef);
        }

        private static List<DataElement> GetEvalutationEntries(InterimTradeElement elementRef)
        {
            return CreateEntries(GetEvalutationObjects(), elementRef);
        }

        private static List<DataElement> GetFirstPositionEntries(InterimTradeElement elementRef)
        {
            return CreateEntries(GetFirstPositionObjects(), elementRef);
        }

        private static List<DataElement> GetReducePositionEntries(InterimTradeElement elementRef)
        {
            return CreateEntries(GetReducePositionObjects(), elementRef);
        }
        #endregion

        #region Summary and Closure
        private static List<DataElement> GetSummaryComponents(TradeSummary elementRef, TradeAnalyticsSummary analytics)
        {
            var summaryCells = GetSummaryComponents(analytics);
            return CreateEntries(summaryCells, elementRef);
        }

        private static List<DataElement> GetTradeClosureComponents(TradeSummary elementRef, TradeAnalyticsSummary analytics)
        {
            var closureCells = GetTradeClosureComponents(analytics);
            return CreateEntries(closureCells, elementRef);
        } 
        #endregion

        private static DataElement CreateEntry(DataElement overview, TradeElement elementRef)
        {
            overview.UpdateParentRefs(elementRef);
            return overview;
        }

        private static List<DataElement> CreateEntries(IEnumerable<DataElement> cellConfigs, TradeElement elementRef)
        {
            return cellConfigs.Select(config => CreateEntry(config, elementRef)).ToList();
        }
    }
}
