using DayJT.Journal.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DayJTrading.Journal.Data.Factory
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
            cell.UpdateParentReference(elementRef);
            return cell;
        }

        internal static List<Cell> CreateCells(IEnumerable<(string Title, ComponentType Type, string Content)> cellConfigs, TradeElement elementRef)
        {
            return cellConfigs.Select(config => CreateCell(config.Title, config.Type, elementRef, config.Content)).ToList();
        }
    }
}
