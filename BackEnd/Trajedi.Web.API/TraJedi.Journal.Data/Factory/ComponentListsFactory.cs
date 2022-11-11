using TraJedi.Journal.Data;

namespace TraJediServer.Journal
{
    public static class ComponentListsFactory
    {
        public static List<InputComponentModel> GetTradeOriginComponents()
        {
            return new List<InputComponentModel>
            {
                new InputComponentModel("Ticker") { ComponentType = ComponentType.Header, IsRelevantForOneLineSummation =true }, //AttachedToggle for long/short
                                       
                new InputComponentModel("Thesis") { ComponentType = ComponentType.Thesis, IsRelevantForOneLineSummation =true },
                new InputComponentModel("Expanded") { ComponentType = ComponentType.Thesis },
                new InputComponentModel("Confluences") { ComponentType = ComponentType.Thesis },
                new InputComponentModel("Triggers") { ComponentType = ComponentType.Thesis },
                new InputComponentModel("Position Plans") { ComponentType = ComponentType.Thesis }, //todo
            }
            .Concat(GetTradeEntryComponents(isActual: false)).ToList();
        }

        public static List<InputComponentModel> GetTradeEntryComponents(bool isActual)
        {
            return new List<InputComponentModel>
            {
                new InputComponentModel("Emotions") { ComponentType = ComponentType.Entry },
                new InputComponentModel("Entry Price") { ComponentType = ComponentType.Entry, PriceValueRelevance = isActual? ValueRelevance.Add : ValueRelevance.None },
                new InputComponentModel("Amount") { ComponentType = ComponentType.Entry },
                new InputComponentModel("Cost") { ComponentType = ComponentType.Entry, CostRelevance = isActual? ValueRelevance.Add : ValueRelevance.None },

                new InputComponentModel("SL") { ComponentType = ComponentType.SLandTarget },
                new InputComponentModel("SL Thoughts") { ComponentType = ComponentType.SLandTarget },
                new InputComponentModel("Target") { ComponentType = ComponentType.SLandTarget },

                new InputComponentModel("Risk") { ComponentType = ComponentType.RiskandReward },
                new InputComponentModel("R:R") { ComponentType = ComponentType.RiskandReward },
            };
        }

        public static List<InputComponentModel> GetTradeExitComponents()
        {
            return new List<InputComponentModel>
            {
                new InputComponentModel("Emotions") { ComponentType = ComponentType.Exit },
                new InputComponentModel("Exit Price") { ComponentType = ComponentType.Exit, PriceValueRelevance = ValueRelevance.Substract },
                new InputComponentModel("Amount") { ComponentType = ComponentType.Exit },
                new InputComponentModel("Cost") { ComponentType = ComponentType.Exit, CostRelevance = ValueRelevance.Substract },

                new InputComponentModel("Exit Reason") { ComponentType = ComponentType.Exit },
            };
        }

        public static List<InputComponentModel> GetInterimSummaryComponents(string averageEntry, string totalAmount, string totalCost)
        {

            return new List<InputComponentModel>
            {
                new InputComponentModel("Average Entry Price") { ComponentType = ComponentType.InterimSummary, Content =averageEntry },
                new InputComponentModel("Total Amount") { ComponentType = ComponentType.InterimSummary, Content =totalAmount },
                new InputComponentModel("Total Cost") { ComponentType = ComponentType.InterimSummary, Content =totalCost },
            };

            //todo indicators - distance from wmas,dmas, bb, kk
        }

        public static List<InputComponentModel> GetTradeClosureComponents(string profitValue = "")
        {
            return new List<InputComponentModel>
            {
                new InputComponentModel("Result") { ComponentType = ComponentType.Closure, Content =profitValue, IsRelevantForOneLineSummation =true },
                new InputComponentModel("Actual R:R") { ComponentType = ComponentType.Closure, IsRelevantForOneLineSummation =true },
                new InputComponentModel("W/L") { ComponentType = ComponentType.Closure, IsRelevantForOneLineSummation = true },
                new InputComponentModel("Lessons") { ComponentType = ComponentType.Closure },
            };
        }
    }
}
