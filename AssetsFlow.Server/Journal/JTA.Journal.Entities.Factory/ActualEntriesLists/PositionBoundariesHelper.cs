namespace HsR.Journal.Entities.Factory.Assists
{
    internal static class PositionBoundariesHelper
    {
        public static List<DataElement> GetPositionBoundariesObjects()
        {
            return new List<DataElement>
            {
                new DataElement("SL", ComponentType.Risk),
                new DataElement("Target", ComponentType.PriceRelated),
                new DataElement("Risk", ComponentType.Risk),
                new DataElement("Projected R:R", ComponentType.PriceRelated)
            };
        }
    }
}
