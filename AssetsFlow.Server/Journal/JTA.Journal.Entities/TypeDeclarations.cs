namespace HsR.Journal.Entities
{
    public enum ComponentType
    {
        Header, Emotions, Thoughts, Technicals, PriceRelated, Risk, Results
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
}
