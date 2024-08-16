
using DayJTrading.Journal.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Channels;

namespace DayJT.Journal.Data
{
    public static class TradeElementEntriesListFactory
    {
        public static IEnumerable<CellType> GetTradeOriginCellTypes()
        {
            return new List<CellType>() { CellType.Ticker, CellType.LongOrShort,
                                          CellType.Thesis, CellType.ThesisExpanded, CellType.Confluences, CellType.Triggers, CellType.PositionPlans };
        }

        public static IEnumerable<CellType> GetAddToPositionCellTypes()
        {
            return new List<CellType>() { CellType.AddEmotions, CellType.AddPrice, CellType.AddAmount, CellType.AddCost, CellType.SL, CellType.SL_Thoughts, CellType.Target, CellType.Risk, CellType.RR };
        }

        public static IEnumerable<CellType> GetReducePositionCellTypes()
        {
            return new List<CellType>() { CellType.ReduceEmotions, CellType.ReducePrice, CellType.ReduceAmount, CellType.ReduceCost, CellType.ReduceReason };
        }


        private static Func<IEnumerable<CellType>, TradeElement, List<Cell>> GetActualCellsFromCellsTypeList = (cellTypes, elementRef) => cellTypes.Select(cellType => CellsFactory.GetCellByType(elementRef, cellType)).ToList();

        public static List<Cell> GetTradeOriginComponents(TradeElement elementRef)
        {
            return GetActualCellsFromCellsTypeList(GetTradeOriginCellTypes(), elementRef);
        }

        public static List<Cell> GetAddToPositionComponents(TradeElement elementRef)
        {
            return GetActualCellsFromCellsTypeList(GetAddToPositionCellTypes(), elementRef);
        }

        public static List<Cell> GetReducePositionComponents(TradeElement elementRef)
        {
            return GetActualCellsFromCellsTypeList(GetReducePositionCellTypes(), elementRef);
        }

        public static List<Cell> GetSummaryComponents(TradeElement elementRef, string averageEntry, string totalAmount, string totalCost)
        {
            return new List<Cell>
            {
                CellsFactory.GetCellByType(elementRef, CellType.AverageEntryPrice, averageEntry),
                CellsFactory.GetCellByType(elementRef, CellType.TotalAmount, totalAmount),
                CellsFactory.GetCellByType(elementRef, CellType.TotalCost, totalCost),
            };

            //todo indicators - distance from wmas,dmas, bb, kk
        }

        public static List<Cell> GetTradeClosureComponents(TradeElement elementRef, string profitValue)
        {
            List<Cell> closureCells = new List<Cell>
            {
                CellsFactory.GetCellByType(elementRef, CellType.Result, profitValue),
                CellsFactory.GetCellByType(elementRef, CellType.ActualRR),
                CellsFactory.GetCellByType(elementRef, CellType.WinOrLoss),
                CellsFactory.GetCellByType(elementRef, CellType.Lessons),
            };

            return closureCells;
        }
    }
}