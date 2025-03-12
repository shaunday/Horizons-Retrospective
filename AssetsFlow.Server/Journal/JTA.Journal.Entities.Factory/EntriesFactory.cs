using HsR.Journal.Entities.Factory.Assists;
using HsR.Journal.Entities.TradeJournal;
using HsR.Journal.TradeAnalytics;

namespace HsR.Journal.Entities.Factory
{
    public static class EntriesFactory
    {
        #region Origin
        public static List<DataElement> GetOriginEntries(InterimTradeElement elementRef)
        {
            return CreateEntries(OriginPositionList.GetTradeOriginObjects(), elementRef);
        }
        #endregion

        #region Interim elements
        public static List<DataElement> GetAddPositionEntries(InterimTradeElement elementRef)
        {
            return CreateEntries(AddPositionLists.GetAddToPositionObjects(), elementRef);
        }

        public static List<DataElement> GetEvalutationEntries(InterimTradeElement elementRef)
        {
            return CreateEntries(AddPositionLists.GetEvalutationObjects(), elementRef);
        }

        public static List<DataElement> GetFirstPositionEntries(InterimTradeElement elementRef)
        {
            return CreateEntries(AddPositionLists.GetFirstPositionObjects(), elementRef);
        }

        public static List<DataElement> GetReducePositionEntries(InterimTradeElement elementRef)
        {
            return CreateEntries(ReducePositionList.GetReducePositionObjects(), elementRef);
        }
        #endregion

        #region Summary and Closure
        public static List<DataElement> GetSummaryComponents(TradeSummary elementRef, TradeAnalyticsSummary analytics)
        {
            var summaryCells = SummaryPositionsLists.GetSummaryComponents(analytics);
            return CreateEntries(summaryCells, elementRef);
        }

        public static List<DataElement> GetTradeClosureComponents(TradeSummary elementRef, TradeAnalyticsSummary analytics)
        {
            var closureCells = SummaryPositionsLists.GetTradeClosureComponents(analytics);
            return CreateEntries(closureCells, elementRef);
        } 
        #endregion

        private static DataElement CreateEntry(DataElement overview, TradeElement elementRef)
        {
            overview.UpdateParentRefs(elementRef);
            return overview;
        }

        public static List<DataElement> CreateEntries(IEnumerable<DataElement> cellConfigs, TradeElement elementRef)
        {
            return cellConfigs.Select(config => CreateEntry(config, elementRef)).ToList();
        }
    }
}
