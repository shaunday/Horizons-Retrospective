using DayJT.Journal.Data;
using DayJT.Journal.DataEntities.Entities;

namespace DayJT.Journal.DataEntities.Factory
{
    internal static class EntriesFactory
    {
        internal static Cell CreateCell(string title, ComponentType type, TradeElement elementRef, string content = "")
        {
            var cell = new Cell(title, type);
            if (!string.IsNullOrEmpty(content))
            {
                cell.Content = content;
            }
            cell.UpdateParentRefs(elementRef);
            return cell;
        }

        internal static List<Cell> CreateCells(IEnumerable<(string Title, ComponentType Type, string Content)> cellConfigs, TradeElement elementRef)
        {
            return cellConfigs.Select(config => CreateCell(config.Title, config.Type, elementRef, config.Content)).ToList();
        }
    }
}
