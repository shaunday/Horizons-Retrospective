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
        public static Cell GetCellByType(TradeElement tradeEleReference, BasicCellType cellType, string content = "") 
        {
            Cell cell = cellType switch
            {
                BasicCellType.Ticker => new Cell(cellType, "Ticker") { ComponentType = ComponentType.Header, IsRelevantForOverview = true },

                BasicCellType.LongOrShort => new Cell(cellType, "LongOrShort") { ComponentType = ComponentType.Header, IsRelevantForOverview = true },
                BasicCellType.Thesis => new Cell(cellType, "Thesis") { ComponentType = ComponentType.Thesis, IsRelevantForOverview = true },
                BasicCellType.ThesisExpanded => new Cell(cellType, "Expanded") { ComponentType = ComponentType.Thesis },
                BasicCellType.Confluences => new Cell(cellType, "Confluences") { ComponentType = ComponentType.Thesis },
                BasicCellType.Triggers => new Cell(cellType, "Triggers") { ComponentType = ComponentType.Thesis },
                BasicCellType.PositionPlans => new Cell(cellType, "Position Plans") { ComponentType = ComponentType.Thesis },

                BasicCellType.AddEmotions => new Cell(cellType, "Emotions") { ComponentType = ComponentType.Addition },
                BasicCellType.AddThoughts => new Cell(cellType, "Thoughts") { ComponentType = ComponentType.Addition },
                BasicCellType.AddPrice => new Cell(cellType, "Entry Price") { ComponentType = ComponentType.Addition, PriceRelevance = ValueRelevance.Add },
                BasicCellType.AddAmount => new Cell(cellType, "Amount") { ComponentType = ComponentType.Addition },
                BasicCellType.AddCost => new Cell(cellType, "Cost") { ComponentType = ComponentType.Addition, CostRelevance = ValueRelevance.Add },

                BasicCellType.SL => new Cell(cellType, "SL") { ComponentType = ComponentType.SLandTarget },
                BasicCellType.SL_Thoughts => new Cell(cellType, "SL Thoughts") { ComponentType = ComponentType.SLandTarget },
                BasicCellType.Target => new Cell(cellType, "Target") { ComponentType = ComponentType.SLandTarget },

                BasicCellType.Risk => new Cell(cellType, "Risk") { ComponentType = ComponentType.RiskReward },
                BasicCellType.RR => new Cell(cellType, "R:R") { ComponentType = ComponentType.RiskReward },

                BasicCellType.ReduceEmotions => new Cell(cellType, "Emotions") { ComponentType = ComponentType.Reduction },
                BasicCellType.ReduceThoughts => new Cell(cellType, "Thoughts") { ComponentType = ComponentType.Reduction },
                BasicCellType.ReducePrice => new Cell(cellType, "Exit Price") { ComponentType = ComponentType.Reduction, PriceRelevance = ValueRelevance.Substract },
                BasicCellType.ReduceAmount => new Cell(cellType, "Amount") { ComponentType = ComponentType.Reduction },
                BasicCellType.ReduceCost => new Cell(cellType, "Cost") { ComponentType = ComponentType.Reduction, CostRelevance = ValueRelevance.Substract },
                BasicCellType.ReduceReason => new Cell(cellType, "Reduce/Close Reason") { ComponentType = ComponentType.Reduction },

                BasicCellType.AverageEntryPrice => new Cell(cellType, "Average Entry Price") { ComponentType = ComponentType.InterimSummary },
                BasicCellType.TotalAmount => new Cell(cellType, "Total Amount") { ComponentType = ComponentType.InterimSummary },
                BasicCellType.TotalCost => new Cell(cellType, "Total Cost") { ComponentType = ComponentType.InterimSummary },

                BasicCellType.Result => new Cell(cellType, "Result") { ComponentType = ComponentType.Closure, IsRelevantForOverview = true },
                BasicCellType.ActualRR => new Cell(cellType, "Actual R:R") { ComponentType = ComponentType.Closure, IsRelevantForOverview = true },
                BasicCellType.WinOrLoss => new Cell(cellType, "W/L") { ComponentType = ComponentType.Closure, IsRelevantForOverview = true },
                BasicCellType.Lessons => new Cell(cellType, "Lessons") { ComponentType = ComponentType.Closure },
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
