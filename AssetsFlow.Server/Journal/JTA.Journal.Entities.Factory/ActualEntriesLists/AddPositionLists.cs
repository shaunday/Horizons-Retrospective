﻿namespace HsR.Journal.Entities.Factory.Assists
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
                new DataElement("Reasoning", ComponentType.Thoughts),
                new DataElement("Emotions", ComponentType.Emotions),
                new DataElement("Time Frame", ComponentType.ExitLogic),
                new DataElement("Price", ComponentType.PriceRelated) { UnitPriceRelevance = ValueRelevance.Positive },
                new DataElement("Amount", ComponentType.PriceRelated),
                new DataElement("Total Cost", ComponentType.PriceRelated) { TotalCostRelevance = ValueRelevance.Positive },
            };

            // Add Position Boundaries objects to the list
            var positionBoundariesObjects = PositionBoundariesHelper.GetPositionBoundariesObjects();
            addToPositionObjects.AddRange(positionBoundariesObjects);

            return addToPositionObjects;
        }

        internal static List<DataElement> GetEvalutationObjects()
        {
            var evaluationObjects = new List<DataElement>
            {
                new DataElement("General", ComponentType.Header),
                new DataElement("D/W/M str?", ComponentType.Technicals),
                new DataElement("Momentum", ComponentType.Technicals),
                new DataElement("Fta reached?", ComponentType.Technicals) ,
                new DataElement("NTA", ComponentType.ExitLogic),
            };

            return evaluationObjects;
        }
    }
}
