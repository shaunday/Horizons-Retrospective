namespace HsR.Journal.Entities.Factory.Assists
{
    internal static class AddPositionLists
    {
        internal static List<DataElement> GetFirstPositionObjects()
        {
            var firstPositionObjects = new List<DataElement>
            {
                new DataElement("Broker", ComponentType.Header) 
                                { IsRelevantForOverview = true, Restrictions = ["Temp1", "Temp2"]}, //todo
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
                new DataElement("Emotions", ComponentType.Header),
                new DataElement("Price", ComponentType.Thoughts) { UnitPriceRelevance = ValueRelevance.Positive },
                new DataElement("Amount", ComponentType.Thoughts),
                new DataElement("Total Cost", ComponentType.Thoughts) { TotalCostRelevance = ValueRelevance.Positive },
            };

            // Add Position Boundaries objects to the list
            var positionBoundariesObjects = PositionBoundariesHelper.GetPositionBoundariesObjects();
            addToPositionObjects.AddRange(positionBoundariesObjects);

            return addToPositionObjects;
        }

        internal static List<DataElement> GetEvalutationObjects()
        {
            var addToPositionObjects = new List<DataElement>
            {
                new DataElement("General", ComponentType.Header),
                new DataElement("Fta reached?", ComponentType.Thoughts) ,
                new DataElement("NTA", ComponentType.Thoughts),
                new DataElement("D/W/M str?", ComponentType.Technicals),
                new DataElement("Momentum", ComponentType.Technicals),
            };

            // Add Position Boundaries objects to the list
            var positionBoundariesObjects = PositionBoundariesHelper.GetPositionBoundariesObjects();
            addToPositionObjects.AddRange(positionBoundariesObjects);

            return addToPositionObjects;
        }
    }
}
