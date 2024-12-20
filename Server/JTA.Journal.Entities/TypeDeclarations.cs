namespace DayJT.Journal.Data
{
    public enum ComponentType
    {
        Header, Thesis, SLandTarget, RiskReward, Addition, Reduction, InterimSummary, Closure
    }

    public enum TradeActionType
    {
        Origin, AddPosition, ReducePosition, InterimSummary, Closure, 
    }

    public enum ValueRelevance
    {
        Positive, Negative
    }
}
