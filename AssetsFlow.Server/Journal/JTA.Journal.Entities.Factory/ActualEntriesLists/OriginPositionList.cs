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
                new DataElement("Confluences", ComponentType.EntryLogic),
                new DataElement("Triggers", ComponentType.EntryLogic),
                new DataElement("Position Plans", ComponentType.EntryLogic),
                new DataElement("Contras", ComponentType.Technicals),
                new DataElement("Time Frame", ComponentType.ExitLogic),
                new DataElement("Confidence Lvl", ComponentType.Emotions),
            };

            // Add Position Boundaries objects to the list
            var positionBoundariesObjects = PositionBoundariesHelper.GetPositionBoundariesObjects();
            tradeOriginObjects.AddRange(positionBoundariesObjects);

            return tradeOriginObjects;
        }
    }
}
