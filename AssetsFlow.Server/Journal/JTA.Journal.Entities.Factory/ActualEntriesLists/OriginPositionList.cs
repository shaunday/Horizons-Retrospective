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
                new DataElement("Triggers", ComponentType.Technicals),
                new DataElement("Time Frame", ComponentType.Technicals),
                new DataElement("Contras", ComponentType.Technicals),
                new DataElement("Confidence Lvl", ComponentType.Emotions),
                new DataElement("Position Plans", ComponentType.Thoughts),
            };

            // Add Position Boundaries objects to the list
            var positionBoundariesObjects = PositionBoundariesHelper.GetPositionBoundariesObjects();
            tradeOriginObjects.AddRange(positionBoundariesObjects);

            return tradeOriginObjects;
        }
    }
}
