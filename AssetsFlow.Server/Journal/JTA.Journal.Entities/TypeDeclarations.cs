namespace HsR.Journal.Entities
{
    public enum ComponentType
    {
        Header, Thesis, DevilsAdvocate, SLandTarget, RiskReward, Addition, Reduction, InterimSummary, Closure
    }

    public enum TradeActionType
    {
        Origin, AddPosition, ReducePosition, InterimSummary, Closure,
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
