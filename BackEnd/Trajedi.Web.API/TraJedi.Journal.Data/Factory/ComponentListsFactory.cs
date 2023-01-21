using TraJedi.Journal.Data;

namespace TraJediServer.Journal
{
    public static class ComponentListsFactory
    {
        public static List<InputComponentModel> GetTradeOriginComponents()
        {
            return new List<InputComponentModel>
            {
                new InputComponentModel("Ticker") { ComponentType = ComponentType.Header, IsRelevantForOverview =true }, //AttachedToggle for long/short
                                       
                new InputComponentModel("Thesis") { ComponentType = ComponentType.Thesis, IsRelevantForOverview =true },
                new InputComponentModel("Expanded") { ComponentType = ComponentType.Thesis },
                new InputComponentModel("Confluences") { ComponentType = ComponentType.Thesis },
                new InputComponentModel("Triggers") { ComponentType = ComponentType.Thesis },
                new InputComponentModel("Position Plans") { ComponentType = ComponentType.Thesis }, //todo
            }
            .Concat(GetAddToPositionComponents(isActual: false)).ToList();
        }

        public static List<InputComponentModel> GetAddToPositionComponents(bool isActual)
        {
            return new List<InputComponentModel>
            {
                new InputComponentModel("Emotions") { ComponentType = ComponentType.Addition },
                new InputComponentModel("Entry Price") { ComponentType = ComponentType.Addition, 
                                                                PriceRelevance = isActual? ValueRelevance.Add : ValueRelevance.None },
                new InputComponentModel("Amount") { ComponentType = ComponentType.Addition },
                new InputComponentModel("Cost") { ComponentType = ComponentType.Addition, 
                                                                CostRelevance = isActual? ValueRelevance.Add : ValueRelevance.None },

                new InputComponentModel("SL") { ComponentType = ComponentType.SLandTarget },
                new InputComponentModel("SL Thoughts") { ComponentType = ComponentType.SLandTarget },
                new InputComponentModel("Target") { ComponentType = ComponentType.SLandTarget },

                new InputComponentModel("Risk") { ComponentType = ComponentType.RiskandReward },
                new InputComponentModel("R:R") { ComponentType = ComponentType.RiskandReward },
            };
        }

        public static List<InputComponentModel> GetReducePositionComponents()
        {
            return new List<InputComponentModel>
            {
                new InputComponentModel("Emotions") { ComponentType = ComponentType.Reduction },
                new InputComponentModel("Exit Price") { ComponentType = ComponentType.Reduction, PriceRelevance = ValueRelevance.Substract },
                new InputComponentModel("Amount") { ComponentType = ComponentType.Reduction },
                new InputComponentModel("Cost") { ComponentType = ComponentType.Reduction, CostRelevance = ValueRelevance.Substract },

                new InputComponentModel("Exit Reason") { ComponentType = ComponentType.Reduction },
            };
        }

        public static List<InputComponentModel> GetSummaryComponents(string averageEntry, string totalAmount, string totalCost)
        {

            return new List<InputComponentModel>
            {
                new InputComponentModel("Average Entry Price") { ComponentType = ComponentType.InterimSummary,
                                                                    Content = averageEntry, IsRelevantForOverview =true  },
                new InputComponentModel("Total Amount") { ComponentType = ComponentType.InterimSummary,
                                                                    Content = totalAmount , IsRelevantForOverview =true },
                new InputComponentModel("Total Cost") { ComponentType = ComponentType.InterimSummary,
                                                                    Content =totalCost , IsRelevantForOverview =true },
            };

            //todo indicators - distance from wmas,dmas, bb, kk
        }

        public static List<InputComponentModel> GetTradeClosureComponents(string profitValue = "")
        {
            return new List<InputComponentModel>
            {
                new InputComponentModel("Result") { ComponentType = ComponentType.Closure, Content =profitValue, IsRelevantForOverview =true },
                new InputComponentModel("Actual R:R") { ComponentType = ComponentType.Closure, IsRelevantForOverview =true },
                new InputComponentModel("W/L") { ComponentType = ComponentType.Closure, IsRelevantForOverview = true },
                new InputComponentModel("Lessons") { ComponentType = ComponentType.Closure },
            };
        }
    }
}
