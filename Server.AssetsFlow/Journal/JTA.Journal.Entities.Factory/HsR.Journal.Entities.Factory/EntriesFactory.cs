﻿using HsR.Journal.Entities.Factory.Assists;
using HsR.Journal.TradeAnalytics;

namespace HsR.Journal.Entities.Factory
{
    public static class EntriesFactory
    {
        public static List<DataElement> GetOriginEntries(TradeElement elementRef)
        {
            return CreateEntries(OriginPositionList.GetTradeOriginObjects(), elementRef);
        }

        public static List<DataElement> GetAddPositionEntries(TradeElement elementRef)
        {
            return CreateEntries(AddPositionLists.GetAddToPositionObjects(), elementRef);
        }

        public static List<DataElement> GetReducePositionEntries(TradeElement elementRef)
        {
            return CreateEntries(ReducePositionList.GetReducePositionObjects(), elementRef);
        }
        public static List<DataElement> GetSummaryComponents(TradeElement elementRef, TradeAnalyticsSummary analytics)
        {
            var summaryCells = SummaryPositionsLists.GetSummaryComponents(analytics);
            return CreateEntries(summaryCells, elementRef);
        }

        public static List<DataElement> GetTradeClosureComponents(TradeElement elementRef, TradeAnalyticsSummary analytics)
        {
            var closureCells = SummaryPositionsLists.GetTradeClosureComponents(analytics);
            return CreateEntries(closureCells, elementRef);
        }


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
