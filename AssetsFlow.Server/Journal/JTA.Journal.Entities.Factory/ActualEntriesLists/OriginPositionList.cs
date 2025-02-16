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
                new DataElement("Thesis", ComponentType.Thoughts) { IsRelevantForOverview = true },
                new DataElement("Confluences", ComponentType.Technicals),
                new DataElement("Confidence lvl", ComponentType.Emotions),
                new DataElement("Triggers", ComponentType.Technicals),
                new DataElement("Position Plans", ComponentType.Thoughts),
                new DataElement("Contra indictr", ComponentType.Technicals)
            };

            // Add Position Boundaries objects to the list
            var positionBoundariesObjects = PositionBoundariesHelper.GetPositionBoundariesObjects();
            tradeOriginObjects.AddRange(positionBoundariesObjects);

            return tradeOriginObjects;
        }
    }
}
