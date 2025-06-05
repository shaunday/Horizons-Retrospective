namespace HsR.Journal.Entities.Factory.Assists
{
    internal static class ReducePositionList
    {
        internal static List<DataElement> GetReducePositionObjects()
        {
            return new List<DataElement>
            {
                new("Emotions", ComponentType.Emotions),

                new("Exit Price", ComponentType.PriceRelated) {  IsRelevantForLocalOverview = true, UnitPriceRelevance = ValueRelevance.Negative },
                new("Amount", ComponentType.PriceRelated) { IsRelevantForLocalOverview = true, },
                new("Cost", ComponentType.PriceRelated) {  IsRelevantForLocalOverview = true, TotalCostRelevance = ValueRelevance.Negative },

                new("Rationale", ComponentType.Thoughts),
                new("R:R", ComponentType.Risk)
            };
        }
    }
}
