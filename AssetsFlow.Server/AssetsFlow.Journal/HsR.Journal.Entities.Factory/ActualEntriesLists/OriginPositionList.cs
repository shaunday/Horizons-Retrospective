namespace HsR.Journal.Entities.Factory.Assists
{
    internal static class OriginPositionList
    {
        internal static List<DataElement> GetTradeOriginObjects()
        {
            var tradeOriginObjects = new List<DataElement>
            {
                new DataElement("Ticker", ComponentType.Header) { IsRelevantForLocalOverview = true, IsRelevantForTradeOverview = true },
                new DataElement("Direction", ComponentType.Header, "")
                                          {  IsRelevantForLocalOverview = true, IsRelevantForTradeOverview = true, Restrictions = ["Long", "Short"]},

                new DataElement("Thesis", ComponentType.Context) {  IsRelevantForLocalOverview = true, IsRelevantForTradeOverview = true },
                new DataElement("Aligned TA", ComponentType.EntryLogic),
                new DataElement("Sector/FA", ComponentType.EntryLogic),

                new DataElement("Certainty", ComponentType.Context) {Restrictions = ["Low", "Mid", "High"]} ,
                new DataElement("Contras", ComponentType.EntryLogic),

                new DataElement("Invalidation", ComponentType.Risk) { IsRelevantForLocalOverview = true },
                new DataElement("Target", ComponentType.ExitLogic) { IsRelevantForLocalOverview = true },
                new DataElement("Time Frame", ComponentType.ExitLogic),
            };

            return tradeOriginObjects;
        }
    }
}
