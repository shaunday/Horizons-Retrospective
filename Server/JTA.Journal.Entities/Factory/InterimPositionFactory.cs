using DayJT.Journal.Data;
using DayJT.Journal.DataEntities.Entities;
using DayJT.Journal.DataEntities.Factory;
using JTA.Journal.Entities.Factory;

namespace DayJTrading.Journal.Data.Factory
{
    internal static class InterimPositionFactory
    {
        private static readonly List<EntryOverview> TradeOriginCells =
            [
                new EntryOverview("Ticker", ComponentType.Header),
                new EntryOverview("LongOrShort", ComponentType.Header),
                new EntryOverview("Thesis", ComponentType.Thesis),
                new EntryOverview("Expanded", ComponentType.Thesis),
                new EntryOverview("Confluences", ComponentType.Thesis),
                new EntryOverview("Triggers", ComponentType.Thesis),
                new EntryOverview("Position Plans", ComponentType.Thesis)
            ];

        private static readonly List<EntryOverview> AddToPositionCells =
            [
                new EntryOverview("Emotions", ComponentType.Addition),
                new EntryOverview("Entry Price", ComponentType.Addition) { ValueRelevance = ValueRelevance.Positive},
                new EntryOverview("Amount", ComponentType.Addition),
                new EntryOverview("Cost", ComponentType.Addition) { CostRelevance = ValueRelevance.Positive},
                new EntryOverview("SL", ComponentType.SLandTarget),
                new EntryOverview("SL Thoughts", ComponentType.SLandTarget),
                new EntryOverview("Target", ComponentType.SLandTarget),
                new EntryOverview("Risk", ComponentType.RiskReward),
                new EntryOverview("R:R", ComponentType.RiskReward)
            ];

        private static readonly List<EntryOverview> ReducePositionCells =
            [
                new EntryOverview("Emotions", ComponentType.Reduction),
                new EntryOverview("Exit Price", ComponentType.Reduction) { ValueRelevance = ValueRelevance.Negative},
                new EntryOverview("Amount", ComponentType.Reduction),
                new EntryOverview("Cost", ComponentType.Reduction) { CostRelevance = ValueRelevance.Negative},
                new EntryOverview("Reduce/Close Reason", ComponentType.Reduction)
            ];

        public static List<DataElement> GetPositionEntries(TradeActionType actionType, TradeElement elementRef)
        {
            switch (actionType)
            {
                case TradeActionType.Origin:
                    return EntriesFactory.CreateEntries(TradeOriginCells, elementRef);

                case TradeActionType.AddPosition:
                    return EntriesFactory.CreateEntries(AddToPositionCells, elementRef);

                case TradeActionType.ReducePosition:
                    return EntriesFactory.CreateEntries(ReducePositionCells, elementRef);

                default:
                    throw new ArgumentException($"Unsupported TradeActionType: {actionType}", nameof(actionType));
            }
        }

    }
}
