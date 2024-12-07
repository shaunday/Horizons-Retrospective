using DayJT.Journal.Data;
using DayJT.Journal.DataEntities.Entities;
using DayJT.Journal.DataEntities.Factory;

namespace DayJTrading.Journal.Data.Factory
{
    internal static class InterimPositionFactory 
    {
        private static readonly List<(string Title, ComponentType Type)> TradeOriginCells = new List<(string, ComponentType)>
        {
            ("Ticker", ComponentType.Header),
            ("LongOrShort", ComponentType.Header),
            ("Thesis", ComponentType.Thesis),
            ("Expanded", ComponentType.Thesis),
            ("Confluences", ComponentType.Thesis),
            ("Triggers", ComponentType.Thesis),
            ("Position Plans", ComponentType.Thesis)
        };

        private static readonly List<(string Title, ComponentType Type)> AddToPositionCells = new List<(string, ComponentType)>
        {
            ("Emotions", ComponentType.Addition),
            ("Entry Price", ComponentType.Addition),
            ("Amount", ComponentType.Addition),
            ("Cost", ComponentType.Addition),
            ("SL", ComponentType.SLandTarget),
            ("SL Thoughts", ComponentType.SLandTarget),
            ("Target", ComponentType.SLandTarget),
            ("Risk", ComponentType.RiskReward),
            ("R:R", ComponentType.RiskReward)
        };

        private static readonly List<(string Title, ComponentType Type)> ReducePositionCells = new List<(string, ComponentType)>
        {
            ("Emotions", ComponentType.Reduction),
            ("Exit Price", ComponentType.Reduction),
            ("Amount", ComponentType.Reduction),
            ("Cost", ComponentType.Reduction),
            ("Reduce/Close Reason", ComponentType.Reduction)
        };

        public static List<Cell> GetPositionEntries(TradeActionType actionType, TradeElement elementRef)
        {
            switch (actionType)
            {
                case TradeActionType.Origin:
                    return EntriesFactory.CreateCells(TradeOriginCells.Select(c => (c.Title, c.Type, "")), elementRef);

                case TradeActionType.AddPosition:
                    return EntriesFactory.CreateCells(AddToPositionCells.Select(c => (c.Title, c.Type, "")), elementRef);

                case TradeActionType.ReducePosition:
                    return EntriesFactory.CreateCells(ReducePositionCells.Select(c => (c.Title, c.Type, "")), elementRef);

                default:
                    throw new ArgumentException($"Unsupported TradeActionType: {actionType}", nameof(actionType));
            }
        }

    }
}
