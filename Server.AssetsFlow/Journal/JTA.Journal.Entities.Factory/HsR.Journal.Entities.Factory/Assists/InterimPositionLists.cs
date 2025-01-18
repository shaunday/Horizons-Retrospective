namespace HsR.Journal.Entities.Factory
{
    internal static class InterimPositionLists
    {
        internal static readonly List<DataElement> TradeOriginObjects =
            [
                new DataElement("Ticker", ComponentType.Header) {IsRelevantForOverview = true },
                new DataElement("LongOrShort", ComponentType.Header) {IsRelevantForOverview = true },
                new DataElement("Broker", ComponentType.Header) { IsRelevantForOverview = true },
                new DataElement("Sector", ComponentType.Header) { IsRelevantForOverview = true },

                new DataElement("Thesis", ComponentType.Thesis),
                new DataElement("Expanded", ComponentType.Thesis),
                new DataElement("Confluences", ComponentType.Thesis),
                new DataElement("Triggers", ComponentType.Thesis),
                new DataElement("Position Plans", ComponentType.Thesis)
            ];

        internal static readonly List<DataElement> AddToPositionObjects =
            [
                new DataElement("Emotions", ComponentType.Addition),
                new DataElement("Per unit", ComponentType.Addition) { UnitPriceRelevance = ValueRelevance.Positive},
                new DataElement("Amount", ComponentType.Addition),
                new DataElement("Total Cost", ComponentType.Addition) { TotalCostRelevance = ValueRelevance.Positive},
                new DataElement("SL", ComponentType.SLandTarget),
                new DataElement("SL Thoughts", ComponentType.SLandTarget),
                new DataElement("Target", ComponentType.SLandTarget),
                new DataElement("Risk", ComponentType.RiskReward),
                new DataElement("Projected R:R", ComponentType.RiskReward)
            ];

        internal static readonly List<DataElement> ReducePositionObjects =
            [
                new DataElement("Emotions", ComponentType.Reduction),
                new DataElement("Exit Price", ComponentType.Reduction) { UnitPriceRelevance = ValueRelevance.Negative},
                new DataElement("Amount", ComponentType.Reduction),
                new DataElement("Cost", ComponentType.Reduction) { TotalCostRelevance = ValueRelevance.Negative},
                new DataElement("Reduce/Close Reason", ComponentType.Reduction),
                new DataElement("R:R", ComponentType.RiskReward)
            ];
    }
}
