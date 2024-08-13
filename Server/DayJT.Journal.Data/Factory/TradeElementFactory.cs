
using DayJTrading.Journal.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Channels;

namespace DayJT.Journal.Data
{
    public static class TradeElementFactory
    {
        public static List<CellType> GetTradeOriginCellTypes()
        {
            return new List<CellType>() { CellType.Ticker, CellType.LongOrShort,
                                          CellType.Thesis, CellType.ThesisExpanded, CellType.Confluences, CellType.Triggers, CellType.PositionPlans };
        }

        public static List<CellType> GetAddToPositionCellTypes()
        {
            return new List<CellType>() { CellType.AddEmotions, CellType.AddPrice, CellType.AddAmount, CellType.AddCost, CellType.SL, CellType.SL_Thoughts, CellType.Target, CellType.Risk, CellType.RR };
        }

        public static List<CellType> GetReducePositionCellTypes()
        {
            return new List<CellType>() { CellType.ReduceEmotions, CellType.ReducePrice, CellType.ReduceAmount, CellType.ReduceCost, CellType.ReduceReason };
        }

        public static List<CellType> GetSummaryCellTypes()
        {
            return new List<CellType>() { CellType.AverageEntryPrice, CellType.TotalAmount, CellType.TotalCost };
        }


        private static Func<List<CellType>, List<Cell>> GetCellsListFromCellsTypeList = new(cellTypes => cellTypes.Select(cellType => CellsFactory.GetCellByType(cellType)).ToList());

        public static List<Cell> GetTradeOriginComponents()
        {
            return GetCellsListFromCellsTypeList(GetTradeOriginCellTypes());
        }

        public static List<Cell> GetAddToPositionComponents()
        {
            return GetCellsListFromCellsTypeList(GetAddToPositionCellTypes());
        }

        public static List<Cell> GetReducePositionComponents()
        {
            return GetCellsListFromCellsTypeList(GetReducePositionCellTypes());
        }


        public static List<Cell> GetSummaryComponents(string averageEntry, string totalAmount, string totalCost)
        {
            return new List<Cell>
            {
                CellsFactory.GetCellByType(CellType.AverageEntryPrice, averageEntry),
                CellsFactory.GetCellByType(CellType.TotalAmount, totalAmount),
                CellsFactory.GetCellByType(CellType.TotalCost, totalCost),
            };

            //todo indicators - distance from wmas,dmas, bb, kk
        }

        public static List<Cell> GetTradeClosureComponents(string profitValue = "")
        {
            List<Cell> closureCells = new List<Cell>
            {
                CellsFactory.GetCellByType(CellType.Result, profitValue),
                CellsFactory.GetCellByType(CellType.ActualRR),
                CellsFactory.GetCellByType(CellType.WinOrLoss),
                CellsFactory.GetCellByType(CellType.Lessons),
            };

            return closureCells;
        }
    }
}