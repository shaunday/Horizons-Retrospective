
using DayJTrading.Journal.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Channels;

namespace DayJT.Journal.Data
{
    public static class TradeElementEntriesListFactory
    {
        public static IEnumerable<BasicCellType> GetTradeOriginCellTypes()
        {
            return new List<BasicCellType>() { BasicCellType.Ticker, BasicCellType.LongOrShort,
                                          BasicCellType.Thesis, BasicCellType.ThesisExpanded, BasicCellType.Confluences, BasicCellType.Triggers, BasicCellType.PositionPlans };
        }

        public static IEnumerable<BasicCellType> GetAddToPositionCellTypes()
        {
            return new List<BasicCellType>() { BasicCellType.AddEmotions, BasicCellType.AddPrice, BasicCellType.AddAmount, BasicCellType.AddCost, BasicCellType.SL, BasicCellType.SL_Thoughts, BasicCellType.Target, BasicCellType.Risk, BasicCellType.RR };
        }

        public static IEnumerable<BasicCellType> GetReducePositionCellTypes()
        {
            return new List<BasicCellType>() { BasicCellType.ReduceEmotions, BasicCellType.ReducePrice, BasicCellType.ReduceAmount, BasicCellType.ReduceCost, BasicCellType.ReduceReason };
        }


        private static Func<IEnumerable<BasicCellType>, TradeElement, List<Cell>> GetActualCellsFromCellsTypeList = (cellTypes, elementRef) => cellTypes.Select(cellType => CellsFactory.GetCellByType(elementRef, cellType)).ToList();

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
                CellsFactory.GetCellByType(elementRef, BasicCellType.AverageEntryPrice, averageEntry),
                CellsFactory.GetCellByType(elementRef, BasicCellType.TotalAmount, totalAmount),
                CellsFactory.GetCellByType(elementRef, BasicCellType.TotalCost, totalCost),
            };

            //todo indicators - distance from wmas,dmas, bb, kk
        }

        public static List<Cell> GetTradeClosureComponents(TradeElement elementRef, string profitValue)
        {
            List<Cell> closureCells = new List<Cell>
            {
                CellsFactory.GetCellByType(elementRef, BasicCellType.Result, profitValue),
                CellsFactory.GetCellByType(elementRef, BasicCellType.ActualRR),
                CellsFactory.GetCellByType(elementRef, BasicCellType.WinOrLoss),
                CellsFactory.GetCellByType(elementRef, BasicCellType.Lessons),
            };

            return closureCells;
        }
    }
}