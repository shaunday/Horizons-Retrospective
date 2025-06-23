namespace HsR.Journal.Entities
{
    public enum ComponentType
    {
        Header, Emotions, Thoughts, Technicals, EntryLogic, ExitLogic, PriceRelated, Risk, Results
    }

    public enum TradeActionType
    {
        Origin, Add, Reduce, Evaluation, Summary,
    }

    public enum ValueRelevance
    {
        Positive, Negative
    }

    public enum TradeStatus
    {
        AnIdea,
        Open,
        Closed
    }

    public static class TradeActionTypeExtensions
    {
        public static bool IsSummaryRelevant(this TradeActionType actionType)
        {
            return actionType == TradeActionType.Add || actionType == TradeActionType.Reduce;
        }
    }
}
