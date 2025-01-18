namespace HsR.Journal.Entities.Factory.Assists
{
    internal static class AddPositionLists
    {
        internal static List<DataElement> GetFirstPositionObjects()
        {
            var firstPositionObjects = new List<DataElement>
            {
                new DataElement("Broker", ComponentType.Header) { IsRelevantForOverview = true },
                new DataElement("Sector", ComponentType.Header) { IsRelevantForOverview = true },
            };

            // Add AddToPositionObjects to the list
            var addToPositionObjects = GetAddToPositionObjects();
            firstPositionObjects.AddRange(addToPositionObjects);

            return firstPositionObjects;
        }

        internal static List<DataElement> GetAddToPositionObjects()
        {
            var addToPositionObjects = new List<DataElement>
            {
                new DataElement("Emotions", ComponentType.Addition),
                new DataElement("Price", ComponentType.Addition) { UnitPriceRelevance = ValueRelevance.Positive },
                new DataElement("Amount", ComponentType.Addition),
                new DataElement("Total Cost", ComponentType.Addition) { TotalCostRelevance = ValueRelevance.Positive },
            };

            // Add Position Boundaries objects to the list
            var positionBoundariesObjects = PositionBoundariesHelper.GetPositionBoundariesObjects();
            addToPositionObjects.AddRange(positionBoundariesObjects);

            return addToPositionObjects;
        }
    }
}
