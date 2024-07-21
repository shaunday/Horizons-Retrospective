namespace DayJT.Journal.Data
{
    public enum ComponentType
    {
        Header, Thesis, Addition, SLandTarget, RiskReward, Reduction, InterimSummary, Closure
    }

    public enum TradeActionType
    {
        Origin, Interim, InterimSummary, Overview1Liner, Closure, 
    }

    public enum ValueRelevance
    {
        None, Add, Substract
    }

    public enum CellType
    {
        Ticker, LongOrShort,
        Thesis, ThesisExpanded, Confluences, Triggers, PositionPlans,
        AddEmotions, AddThoughts, AddPrice, AddAmount, AddCost, SL, SL_Thoughts, Target, Risk, RR,
        ReduceEmotions, ReduceThoughts, ReducePrice, ReduceAmount, ReduceCost, ReduceReason,
        AverageEntryPrice, TotalAmount, TotalCost,
        Result, ActualRR, WinOrLoss, Lessons
    }

}
