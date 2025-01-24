namespace HsR.Journal.Entities.Factory.Assists
{
    internal static class PositionBoundariesHelper
    {
        public static List<DataElement> GetPositionBoundariesObjects()
        {
            return new List<DataElement>
            {
                new DataElement("SL", ComponentType.SLandTarget),
                new DataElement("Target", ComponentType.SLandTarget),
                new DataElement("Risk", ComponentType.RiskReward),
                new DataElement("Projected R:R", ComponentType.RiskReward)
            };
        }
    }
}
