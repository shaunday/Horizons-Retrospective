namespace HsR.Journal.Entities.Factory.Assists
{
    internal static class OriginPositionList
    {
        internal static List<DataElement> GetTradeOriginObjects()
        {
            var tradeOriginObjects = new List<DataElement>
            {
                new DataElement("Ticker", ComponentType.Header) { IsRelevantForOverview = true },
                new DataElement("LongOrShort", ComponentType.Header) { IsRelevantForOverview = true },

                new DataElement("Thesis", ComponentType.Thesis),
                new DataElement("Expanded", ComponentType.Thesis),
                new DataElement("Confluences", ComponentType.Thesis),
                new DataElement("Confidence levels", ComponentType.Thesis),
                new DataElement("Triggers", ComponentType.Thesis),
                new DataElement("Position Plans", ComponentType.Thesis)
            };

            // Add Position Boundaries objects to the list
            var positionBoundariesObjects = PositionBoundariesHelper.GetPositionBoundariesObjects();
            tradeOriginObjects.AddRange(positionBoundariesObjects);

            return tradeOriginObjects;
        }
    }
}
