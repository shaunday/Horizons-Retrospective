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
        None, Add, Substract
    }

    public enum BasicCellType
    {
        Ticker, LongOrShort,
        Thesis, ThesisExpanded, Confluences, Triggers, PositionPlans,
        AddEmotions, AddThoughts, AddPrice, AddAmount, AddCost, SL, SL_Thoughts, Target, Risk, RR,
        ReduceEmotions, ReduceThoughts, ReducePrice, ReduceAmount, ReduceCost, ReduceReason,
        AverageEntryPrice, TotalAmount, TotalCost,
        Result, ActualRR, WinOrLoss, Lessons
    }

}
