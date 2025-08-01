﻿namespace HsR.Journal.Entities.Factory
{
    public static partial class TradeElementsFactory
    {
        private static List<DataElement> GetReducePositionObjects()
        {
            return new List<DataElement>
            {
                new("Emotions", ComponentType.Context),

                new("Exit Price", ComponentType.PriceRelated) {  IsRelevantForLocalOverview = true, UnitPriceRelevance = ValueRelevance.Negative },
                new("Amount", ComponentType.PriceRelated) { IsRelevantForLocalOverview = true, },
                new("Cost", ComponentType.PriceRelated) {  IsRelevantForLocalOverview = true, TotalCostRelevance = ValueRelevance.Negative },

                new("Rationale", ComponentType.Context),
                new("R:R", ComponentType.Risk)
            };
        }
    }
}
