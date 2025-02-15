namespace HsR.Journal.Entities.Factory.Assists
{
    internal static class ReducePositionList
    {
        internal static List<DataElement> GetReducePositionObjects()
        {
            return new List<DataElement>
            {
                new("Emotions", ComponentType.Emotions),
                new("Exit Price", ComponentType.PriceRelated) { UnitPriceRelevance = ValueRelevance.Negative },
                new("Amount", ComponentType.PriceRelated),
                new("Cost", ComponentType.PriceRelated) { TotalCostRelevance = ValueRelevance.Negative },
                new("Rationale", ComponentType.Thoughts),
                new("R:R", ComponentType.Risk)
            };
        }
    }
}
