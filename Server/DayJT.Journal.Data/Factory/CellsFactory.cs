using DayJT.Journal.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace DayJTrading.Journal.Data
{
    public static class CellsFactory
    {
        public static Cell GetCellByType(CellType cellId, string content = "")
        {
            Cell cell = cellId switch
            {
                CellType.Ticker => new Cell(cellId, "Ticker") { ComponentType = ComponentType.Header, IsRelevantForOverview = true },

                CellType.LongOrShort => new Cell(cellId, "LongOrShort") { ComponentType = ComponentType.Header, IsRelevantForOverview = true },
                CellType.Thesis => new Cell(cellId, "Thesis") { ComponentType = ComponentType.Thesis, IsRelevantForOverview = true },
                CellType.ThesisExpanded => new Cell(cellId, "Expanded") { ComponentType = ComponentType.Thesis },
                CellType.Confluences => new Cell(cellId, "Confluences") { ComponentType = ComponentType.Thesis },
                CellType.Triggers => new Cell(cellId, "Triggers") { ComponentType = ComponentType.Thesis },
                CellType.PositionPlans => new Cell(cellId, "Position Plans") { ComponentType = ComponentType.Thesis },

                CellType.AddEmotions => new Cell(cellId, "Emotions") { ComponentType = ComponentType.Addition },
                CellType.AddThoughts => new Cell(cellId, "Thoughts") { ComponentType = ComponentType.Addition },
                CellType.AddPrice => new Cell(cellId, "Entry Price") { ComponentType = ComponentType.Addition, CostRelevance = ValueRelevance.Add },
                CellType.AddAmount => new Cell(cellId, "Amount") { ComponentType = ComponentType.Addition },
                CellType.AddCost => new Cell(cellId, "Cost") { ComponentType = ComponentType.Addition, CostRelevance = ValueRelevance.Add },

                CellType.SL => new Cell(cellId, "SL") { ComponentType = ComponentType.SLandTarget },
                CellType.SL_Thoughts => new Cell(cellId, "SL Thoughts") { ComponentType = ComponentType.SLandTarget },
                CellType.Target => new Cell(cellId, "Target") { ComponentType = ComponentType.SLandTarget },
                CellType.Risk => new Cell(cellId, "Risk") { ComponentType = ComponentType.RiskReward },
                CellType.RR => new Cell(cellId, "R:R") { ComponentType = ComponentType.RiskReward },

                CellType.ReduceEmotions => new Cell(cellId, "Emotions") { ComponentType = ComponentType.Reduction },
                CellType.ReduceThoughts => new Cell(cellId, "Thoughts") { ComponentType = ComponentType.Reduction },
                CellType.ReducePrice => new Cell(cellId, "Exit Price") { ComponentType = ComponentType.Reduction, PriceRelevance = ValueRelevance.Substract },
                CellType.ReduceAmount => new Cell(cellId, "Amount") { ComponentType = ComponentType.Reduction },
                CellType.ReduceCost => new Cell(cellId, "Cost") { ComponentType = ComponentType.Reduction, CostRelevance = ValueRelevance.Substract },
                CellType.ReduceReason => new Cell(cellId, "Reduce/Close Reason") { ComponentType = ComponentType.Reduction },

                CellType.AverageEntryPrice => new Cell(cellId, "Average Entry Price") { ComponentType = ComponentType.Reduction },
                CellType.TotalAmount => new Cell(cellId, "Total Amount") { ComponentType = ComponentType.Reduction, CostRelevance = ValueRelevance.Substract },
                CellType.TotalCost => new Cell(cellId, "Total Cost") { ComponentType = ComponentType.Reduction },

                CellType.Result => new Cell(cellId, "Result") { ComponentType = ComponentType.Closure, IsRelevantForOverview = true },
                CellType.ActualRR => new Cell(cellId, "Actual R:R") { ComponentType = ComponentType.Closure, IsRelevantForOverview = true },
                CellType.WinOrLoss => new Cell(cellId, "W/L") { ComponentType = ComponentType.Closure, IsRelevantForOverview = true },
                CellType.Lessons => new Cell(cellId, "Lessons") { ComponentType = ComponentType.Closure },
            };

            if (!string.IsNullOrEmpty(content))
            {
                cell.Content = content;
            }

            return cell;
        }
    }
}
