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
        public static Cell GetCellByType(TradeElement tradeEleReference, CellType cellType, string content = "") 
        {
            Cell cell = cellType switch
            {
                CellType.Ticker => new Cell(cellType, "Ticker") { ComponentType = ComponentType.Header, IsRelevantForOverview = true },

                CellType.LongOrShort => new Cell(cellType, "LongOrShort") { ComponentType = ComponentType.Header, IsRelevantForOverview = true },
                CellType.Thesis => new Cell(cellType, "Thesis") { ComponentType = ComponentType.Thesis, IsRelevantForOverview = true },
                CellType.ThesisExpanded => new Cell(cellType, "Expanded") { ComponentType = ComponentType.Thesis },
                CellType.Confluences => new Cell(cellType, "Confluences") { ComponentType = ComponentType.Thesis },
                CellType.Triggers => new Cell(cellType, "Triggers") { ComponentType = ComponentType.Thesis },
                CellType.PositionPlans => new Cell(cellType, "Position Plans") { ComponentType = ComponentType.Thesis },

                CellType.AddEmotions => new Cell(cellType, "Emotions") { ComponentType = ComponentType.Addition },
                CellType.AddThoughts => new Cell(cellType, "Thoughts") { ComponentType = ComponentType.Addition },
                CellType.AddPrice => new Cell(cellType, "Entry Price") { ComponentType = ComponentType.Addition, CostRelevance = ValueRelevance.Add },
                CellType.AddAmount => new Cell(cellType, "Amount") { ComponentType = ComponentType.Addition },
                CellType.AddCost => new Cell(cellType, "Cost") { ComponentType = ComponentType.Addition, CostRelevance = ValueRelevance.Add },

                CellType.SL => new Cell(cellType, "SL") { ComponentType = ComponentType.SLandTarget },
                CellType.SL_Thoughts => new Cell(cellType, "SL Thoughts") { ComponentType = ComponentType.SLandTarget },
                CellType.Target => new Cell(cellType, "Target") { ComponentType = ComponentType.SLandTarget },
                CellType.Risk => new Cell(cellType, "Risk") { ComponentType = ComponentType.RiskReward },
                CellType.RR => new Cell(cellType, "R:R") { ComponentType = ComponentType.RiskReward },

                CellType.ReduceEmotions => new Cell(cellType, "Emotions") { ComponentType = ComponentType.Reduction },
                CellType.ReduceThoughts => new Cell(cellType, "Thoughts") { ComponentType = ComponentType.Reduction },
                CellType.ReducePrice => new Cell(cellType, "Exit Price") { ComponentType = ComponentType.Reduction, PriceRelevance = ValueRelevance.Substract },
                CellType.ReduceAmount => new Cell(cellType, "Amount") { ComponentType = ComponentType.Reduction },
                CellType.ReduceCost => new Cell(cellType, "Cost") { ComponentType = ComponentType.Reduction, CostRelevance = ValueRelevance.Substract },
                CellType.ReduceReason => new Cell(cellType, "Reduce/Close Reason") { ComponentType = ComponentType.Reduction },

                CellType.AverageEntryPrice => new Cell(cellType, "Average Entry Price") { ComponentType = ComponentType.Reduction },
                CellType.TotalAmount => new Cell(cellType, "Total Amount") { ComponentType = ComponentType.Reduction, CostRelevance = ValueRelevance.Substract },
                CellType.TotalCost => new Cell(cellType, "Total Cost") { ComponentType = ComponentType.Reduction },

                CellType.Result => new Cell(cellType, "Result") { ComponentType = ComponentType.Closure, IsRelevantForOverview = true },
                CellType.ActualRR => new Cell(cellType, "Actual R:R") { ComponentType = ComponentType.Closure, IsRelevantForOverview = true },
                CellType.WinOrLoss => new Cell(cellType, "W/L") { ComponentType = ComponentType.Closure, IsRelevantForOverview = true },
                CellType.Lessons => new Cell(cellType, "Lessons") { ComponentType = ComponentType.Closure },
            };

            if (!string.IsNullOrEmpty(content))
            {
                cell.Content = content;
            }

            cell.UpdateParentReference(tradeEleReference);

            return cell;
        }
    }
}
