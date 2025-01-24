namespace HsR.Journal.Entities.Factory.Assists
{
    internal static class ReducePositionList
    {
        internal static List<DataElement> GetReducePositionObjects()
        {
            return new List<DataElement>
            {
                new("Emotions", ComponentType.Reduction),
                new("Exit Price", ComponentType.Reduction) { UnitPriceRelevance = ValueRelevance.Negative },
                new("Amount", ComponentType.Reduction),
                new("Cost", ComponentType.Reduction) { TotalCostRelevance = ValueRelevance.Negative },
                new("Rationale", ComponentType.Reduction),
                new("R:R", ComponentType.RiskReward)
            };
        }
    }
}
