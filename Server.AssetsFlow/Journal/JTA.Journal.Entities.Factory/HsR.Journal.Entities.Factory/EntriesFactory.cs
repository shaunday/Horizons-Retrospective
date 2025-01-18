namespace HsR.Journal.Entities.Factory
{
    public static class EntriesFactory
    {
        public static List<DataElement> GetSummaryComponents(TradeElement elementRef, string averageEntry, string totalAmount, string totalCost)
        {
            var summaryCells = SummaryPositionsLists.GetSummaryComponents(averageEntry, totalAmount, totalCost);
            return CreateEntries(summaryCells, elementRef);
        }

        public static List<DataElement> GetTradeClosureComponents(TradeElement elementRef, string? profitValue)
        {
            var closureCells = SummaryPositionsLists.GetTradeClosureComponents(profitValue);
            return CreateEntries(closureCells, elementRef);
        }

        public static List<DataElement> GetOriginEntries(TradeElement elementRef)
        {
            return CreateEntries(InterimPositionLists.TradeOriginObjects, elementRef);
        }

        public static List<DataElement> GetAddPositionEntries(TradeElement elementRef)
        {
            return CreateEntries(InterimPositionLists.AddToPositionObjects, elementRef);
        }

        public static List<DataElement> GetReducePositionEntries(TradeElement elementRef)
        {
            return CreateEntries(InterimPositionLists.ReducePositionObjects, elementRef);
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
