namespace HsR.Journal.Entities.Factory.Assists
{
    internal static class OriginPositionList
    {
        internal static List<DataElement> GetTradeOriginObjects()
        {
            var tradeOriginObjects = new List<DataElement>
            {
                new DataElement("Ticker", ComponentType.Header) { IsRelevantForLocalOverview = true, IsRelevantForTradeOverview = true },
                new DataElement("Long/Short", ComponentType.Header, "")
                                          {  IsRelevantForLocalOverview = true, IsRelevantForTradeOverview = true, Restrictions = ["Long", "Short"]},
                new DataElement("Thesis", ComponentType.Thoughts) {  IsRelevantForLocalOverview = true, IsRelevantForTradeOverview = true },
                new DataElement("Confluences", ComponentType.EntryLogic),
                new DataElement("Trigger", ComponentType.EntryLogic),
                new DataElement("Contras", ComponentType.EntryLogic),
                new DataElement("Time Frame", ComponentType.ExitLogic)  { IsRelevantForTradeOverview = true },
                new DataElement("Certainty", ComponentType.Emotions) {Restrictions = ["Low", "Mid", "High"]} ,

                new DataElement("SL", ComponentType.Risk) { IsRelevantForLocalOverview = true },
                new DataElement("Target", ComponentType.ExitLogic) { IsRelevantForLocalOverview = true, },
                new DataElement("R:R", ComponentType.Risk)
            };

            return tradeOriginObjects;
        }
    }
}
