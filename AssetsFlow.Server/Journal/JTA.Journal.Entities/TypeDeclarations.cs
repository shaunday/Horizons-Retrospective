namespace HsR.Journal.Entities
{
    public enum ComponentType
    {
        Header, Thesis, DevilsAdvocate, SLandTarget, RiskReward, Addition, Reduction, Summary
    }

    public enum TradeActionType
    {
        Origin, AddPosition, ReducePosition, InterimSummary, Summary,
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
