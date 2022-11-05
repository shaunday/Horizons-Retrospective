using TraJedi.Journal.Data;

namespace TraJediServer.Journal
{
    public static class ComponentListsFactory
    {
        public static List<InputComponentModel> GetTradeOriginComponents()
        {
            return new List<InputComponentModel>
            {
                new InputComponentModel() { Title = "Ticker", ComponentType = ComponentType.Header, RelevantForTradeSummary =true }, //AttachedToggle for long/short
                                       
                new InputComponentModel() { Title = "Thesis", ComponentType = ComponentType.Thesis, RelevantForTradeSummary =true },
                new InputComponentModel() { Title = "Expanded", ComponentType = ComponentType.Thesis },
                new InputComponentModel() { Title = "Confluences", ComponentType = ComponentType.Thesis },
                new InputComponentModel() { Title = "Triggers", ComponentType = ComponentType.Thesis },
                new InputComponentModel() { Title = "Position Plans", ComponentType = ComponentType.Thesis }, //todo
            }
            .Concat(GetTradeEntryComponents(isActual: false)).ToList();
        }

        public static List<InputComponentModel> GetTradeEntryComponents(bool isActual)
        {
            return new List<InputComponentModel>
            {
                new InputComponentModel() { Title = "Emotions", ComponentType = ComponentType.Entry },
                new InputComponentModel() { Title = "Entry Price", ComponentType = ComponentType.Entry,
                                                                                PriceValueRelevant = isActual? ValueRelevance.Add : ValueRelevance.None },
                new InputComponentModel() { Title = "Amount", ComponentType = ComponentType.Entry },
                new InputComponentModel() { Title = "Cost", ComponentType = ComponentType.Entry,
                                                                                    CostRelevant = isActual? ValueRelevance.Add : ValueRelevance.None },

                new InputComponentModel() { Title = "SL", ComponentType = ComponentType.SLandTarget },
                new InputComponentModel() { Title = "SL Thoughts", ComponentType = ComponentType.SLandTarget },
                new InputComponentModel() { Title = "Target", ComponentType = ComponentType.SLandTarget },

                new InputComponentModel() { Title = "Risk", ComponentType = ComponentType.RiskandReward },
                new InputComponentModel() { Title = "R:R",  ComponentType = ComponentType.RiskandReward },
            };
        }

        public static List<InputComponentModel> GetTradeExitComponents()
        {
            return new List<InputComponentModel>
            {
                new InputComponentModel() { Title = "Emotions", ComponentType = ComponentType.Exit },
                new InputComponentModel() { Title = "Exit Price", ComponentType = ComponentType.Exit, PriceValueRelevant = ValueRelevance.Substract },
                new InputComponentModel() { Title = "Amount", ComponentType = ComponentType.Exit },
                new InputComponentModel() { Title = "Cost", ComponentType = ComponentType.Exit, CostRelevant = ValueRelevance.Substract },

                new InputComponentModel() { Title = "Exit Reason", ComponentType = ComponentType.Exit },
            };
        }

        public static List<InputComponentModel> GetInterimSummaryComponents(string averageEntry, string totalAmount, string totalCost)
        {

            return new List<InputComponentModel>
            {
                new InputComponentModel() { Title = "Average Entry Price", ComponentType = ComponentType.InterimSummary, Content =averageEntry },
                new InputComponentModel() { Title = "Total Amount", ComponentType = ComponentType.InterimSummary , Content =totalAmount },
                new InputComponentModel() { Title = "Total Cost", ComponentType = ComponentType.InterimSummary, Content =totalCost },
            };

            //todo indicators - distance from wmas,dmas, bb, kk
        }

        public static List<InputComponentModel> GetTradeClosureComponents(string profitValue = "")
        {
            return new List<InputComponentModel>
            {
                new InputComponentModel() { Title = "Result",  ComponentType = ComponentType.Closure, Content =profitValue, RelevantForTradeSummary =true },
                new InputComponentModel() { Title = "Actual R:R",  ComponentType = ComponentType.Closure, RelevantForTradeSummary =true },
                new InputComponentModel() { Title = "W/L", ComponentType = ComponentType.Closure, RelevantForTradeSummary = true },
                new InputComponentModel() { Title = "Lessons", ComponentType = ComponentType.Closure },
            };
        }
    }
}
