using DayJT.Journal.Data;

namespace TraJediServer.Journal
{
    public static class TradeInfoFactory
    {
        public static List<Cell> GetTradeOriginComponents()
        {
            return new List<Cell>
            {
                new Cell("Ticker") { ComponentType = ComponentType.Header, IsRelevantForOverview =true }, //AttachedToggle for long/short
                                       
                new Cell("Thesis") { ComponentType = ComponentType.Thesis, IsRelevantForOverview =true },
                new Cell("Expanded") { ComponentType = ComponentType.Thesis },
                new Cell("Confluences") { ComponentType = ComponentType.Thesis },
                new Cell("Triggers") { ComponentType = ComponentType.Thesis },
                new Cell("Position Plans") { ComponentType = ComponentType.Thesis }, 
            }
            .Concat(GetAddToPositionComponents(isActual: false)).ToList();
        }

        public static List<Cell> GetAddToPositionComponents(bool isActual)
        {
            return new List<Cell>
            {
                new Cell("Emotions") { ComponentType = ComponentType.Addition },
                new Cell("Entry Price") { ComponentType = ComponentType.Addition, 
                                                                PriceRelevance = isActual? ValueRelevance.Add : ValueRelevance.None },
                new Cell("Amount") { ComponentType = ComponentType.Addition },
                new Cell("Cost") { ComponentType = ComponentType.Addition, 
                                                                CostRelevance = isActual? ValueRelevance.Add : ValueRelevance.None },

                new Cell("SL") { ComponentType = ComponentType.SLandTarget },
                new Cell("SL Thoughts") { ComponentType = ComponentType.SLandTarget },
                new Cell("Target") { ComponentType = ComponentType.SLandTarget },

                new Cell("Risk") { ComponentType = ComponentType.RiskandReward },
                new Cell("R:R") { ComponentType = ComponentType.RiskandReward },
            };
        }

        public static List<Cell> GetReducePositionComponents()
        {
            return new List<Cell>
            {
                new Cell("Emotions") { ComponentType = ComponentType.Reduction },
                new Cell("Exit Price") { ComponentType = ComponentType.Reduction, PriceRelevance = ValueRelevance.Substract },
                new Cell("Amount") { ComponentType = ComponentType.Reduction },
                new Cell("Cost") { ComponentType = ComponentType.Reduction, CostRelevance = ValueRelevance.Substract },

                new Cell("Exit Reason") { ComponentType = ComponentType.Reduction },
            };
        }

        public static List<Cell> GetSummaryComponents(string averageEntry, string totalAmount, string totalCost)
        {

            return new List<Cell>
            {
                new Cell("Average Entry Price") { ComponentType = ComponentType.InterimSummary,
                                                                    Content = averageEntry, IsRelevantForOverview =true  },
                new Cell("Total Amount") { ComponentType = ComponentType.InterimSummary,
                                                                    Content = totalAmount , IsRelevantForOverview =true },
                new Cell("Total Cost") { ComponentType = ComponentType.InterimSummary,
                                                                    Content =totalCost , IsRelevantForOverview =true },
            };

            //todo indicators - distance from wmas,dmas, bb, kk
        }

        public static List<Cell> GetTradeClosureComponents(string profitValue = "")
        {
            return new List<Cell>
            {
                new Cell("Result") { ComponentType = ComponentType.Closure, Content =profitValue, IsRelevantForOverview =true },
                new Cell("Actual R:R") { ComponentType = ComponentType.Closure, IsRelevantForOverview =true },
                new Cell("W/L") { ComponentType = ComponentType.Closure, IsRelevantForOverview = true },
                new Cell("Lessons") { ComponentType = ComponentType.Closure },
            };
        }
    }
}
