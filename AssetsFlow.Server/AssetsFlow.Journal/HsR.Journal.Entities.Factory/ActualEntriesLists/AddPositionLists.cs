namespace HsR.Journal.Entities.Factory.Assists
{
    internal static class AddPositionLists
    {
        internal static List<DataElement> GetFirstPositionObjects()
        {
            var firstPositionObjects = new List<DataElement>
            {
                new DataElement("Broker", ComponentType.Header) 
                                { Restrictions = ["Temp1", "Temp2"]}, //todo
                new DataElement("Sector", ComponentType.Header),
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
                new DataElement("Reasoning", ComponentType.Thoughts),
                new DataElement("Emotions", ComponentType.Emotions),

                new DataElement("Price", ComponentType.PriceRelated) { IsRelevantForLocalOverview = true, UnitPriceRelevance = ValueRelevance.Positive },
                new DataElement("Amount", ComponentType.PriceRelated) { IsRelevantForLocalOverview = true },
                new DataElement("Total Cost", ComponentType.PriceRelated) {  IsRelevantForLocalOverview = true, TotalCostRelevance = ValueRelevance.Positive },

                new DataElement("Time Frame", ComponentType.ExitLogic)  {  IsRelevantForLocalOverview = true },
                new DataElement("Target", ComponentType.ExitLogic) { IsRelevantForLocalOverview = true, },
                new DataElement("SL", ComponentType.Risk) { IsRelevantForLocalOverview = true },
                new DataElement("R:R", ComponentType.Risk)
            };

            return addToPositionObjects;
        }

        internal static List<DataElement> GetEvalutationObjects()
        {
            var evaluationObjects = new List<DataElement>
            {
                new DataElement("General", ComponentType.Header) { IsRelevantForLocalOverview = true, },
                new DataElement("D/W/M str?", ComponentType.Technicals) { IsRelevantForLocalOverview = true },
                new DataElement("Momentum", ComponentType.Technicals) { IsRelevantForLocalOverview = true },
                new DataElement("Fta reached?", ComponentType.Technicals) { IsRelevantForLocalOverview = true, }, 
                new DataElement("NTA", ComponentType.ExitLogic) { IsRelevantForLocalOverview = true },
            };

            return evaluationObjects;
        }
    }
}
