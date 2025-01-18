namespace HsR.Journal.Entities.Factory.Assists
{
    internal static class ReducePositionList
    {
        internal static List<DataElement> GetReducePositionObjects()
        {
            return new List<DataElement>
            {
                new DataElement("Emotions", ComponentType.Reduction),
                new DataElement("Exit Price", ComponentType.Reduction) { UnitPriceRelevance = ValueRelevance.Negative },
                new DataElement("Amount", ComponentType.Reduction),
                new DataElement("Cost", ComponentType.Reduction) { TotalCostRelevance = ValueRelevance.Negative },
                new DataElement("Reduce/Close Reason", ComponentType.Reduction),
                new DataElement("R:R", ComponentType.RiskReward)
            };
        }
    }
}
