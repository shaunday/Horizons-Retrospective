namespace TraJedi.Journal.Data
{
    public enum ComponentType
    {
        Header, Thesis, Entry, SLandTarget, RiskandReward, Exit, InterimSummary, Closure, Indicators
    }

    public enum TradeInputType
    {
        Origin, Interim, InterimSummary, Closure, OneLineSummation
    }

    public enum ValueRelevance
    {
        None, Add, Substract
    }

}
