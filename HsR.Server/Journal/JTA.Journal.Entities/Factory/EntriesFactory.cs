namespace HsR.Journal.Entities.Factory
{
    internal static class EntriesFactory
    {
        internal static DataElement CreateEntry(EntryOverview overview, TradeElement elementRef)
        {
            var cell = new DataElement(overview.Title, overview.Type);
            if (!string.IsNullOrEmpty(overview.Content))
            {
                cell.ContentWrapper = new ContentRecord(overview.Content);
            }
            cell.UpdateParentRefs(elementRef);
            return cell;
        }

        internal static List<DataElement> CreateEntries(IEnumerable<EntryOverview> cellConfigs, TradeElement elementRef)
        {
            return cellConfigs.Select(config => CreateEntry(config, elementRef)).ToList();
        }
    }
}
