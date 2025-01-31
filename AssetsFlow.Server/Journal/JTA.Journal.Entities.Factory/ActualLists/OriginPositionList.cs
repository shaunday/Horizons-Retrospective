namespace HsR.Journal.Entities.Factory.Assists
{
    internal static class OriginPositionList
    {
        internal static List<DataElement> GetTradeOriginObjects()
        {
            var tradeOriginObjects = new List<DataElement>
            {
                new DataElement("Ticker", ComponentType.Header) { IsRelevantForOverview = true },
                new DataElement("Long/Short", ComponentType.Header, "") 
                                          { IsRelevantForOverview = true, Restrictions = ["Long", "Short"]},
                new DataElement("Thesis", ComponentType.Thesis) { IsRelevantForOverview = true },
                new DataElement("Confluences", ComponentType.Thesis),
                new DataElement("Confidence level", ComponentType.Thesis),
                new DataElement("Triggers", ComponentType.Thesis),
                new DataElement("Position Plans", ComponentType.Thesis),
                new DataElement("Am I wrong?", ComponentType.DevilsAdvocate),
                new DataElement("Anti confluences", ComponentType.DevilsAdvocate)
            };

            // Add Position Boundaries objects to the list
            var positionBoundariesObjects = PositionBoundariesHelper.GetPositionBoundariesObjects();
            tradeOriginObjects.AddRange(positionBoundariesObjects);

            return tradeOriginObjects;
        }
    }
}
