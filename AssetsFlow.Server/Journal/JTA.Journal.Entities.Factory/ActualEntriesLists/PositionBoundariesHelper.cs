namespace HsR.Journal.Entities.Factory.Assists
{
    internal static class PositionBoundariesHelper
    {
        public static List<DataElement> GetPositionBoundariesObjects()
        {
            return new List<DataElement>
            {
                new DataElement("SL", ComponentType.Risk) { IsRelevantForLocalOverview = true },
                new DataElement("Target", ComponentType.ExitLogic) { IsRelevantForLocalOverview = true, },
                new DataElement("Risk", ComponentType.Risk),
                new DataElement("R:R", ComponentType.Risk)
            };
        }
    }
}
