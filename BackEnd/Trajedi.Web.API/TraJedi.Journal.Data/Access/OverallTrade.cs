using TraJedi.Journal.Data;

namespace TraJediServer.Journal
{
    public class JournalTradeWrapper 
    {
        #region Accessors

        public OverallTradeModel BackendTradeModel { get; private set; }

        public List<TradeInputModel> TradeInputs
        {
            get => BackendTradeModel.TradeInputs;
            set => BackendTradeModel.TradeInputs = value;
        }

        #endregion

        #region Properties

        public TradeInputModel? TradeOrigin
        {
            get
            {
                if (TradeInputs.Where(t => t.TradeInputType == TradeInputType.Origin) is TradeInputModel tradeInput)
                    return tradeInput;
                else
                    return null;

            }
        }

        public TradeInputModel? TradeClosure
        {
            get
            {
                if (TradeInputs.Where(t => t.TradeInputType == TradeInputType.Closure) is TradeInputModel tradeInput)
                    return tradeInput;
                else
                    return null;
            }
        }

        public TradeInputModel? TradeSummary
        {
            get
            {
                TradeInputModel tradeSummary = new TradeInputModel() { TradeInputType = TradeInputType.TradeSummary, TradeComponents = new() };
                foreach (var component in TradeOrigin.TradeComponents)
                {
                    if (component.RelevantForTradeSummary)
                    {
                        tradeSummary.TradeComponents.Add(component);
                    }
                }

                return tradeSummary;
            }
        }

        #endregion

        public void Init(TraJediDataContext dataContext, Guid? id = null)
        {
            if (id == null)
            {
                BackendTradeModel = new OverallTradeModel();
                BackendTradeModel.TradeInputs.Add(new TradeInputModel()
                {
                    TradeInputType = TradeInputType.Origin,
                    TradeComponents = ComponentListsFactory.GetTradeOriginComponents()
                });
                dataContext.OverallTrades.Add(BackendTradeModel);
            }
            else
            {
                BackendTradeModel = dataContext.OverallTrades.Where(t => t.OverallTradeId == id).FirstOrDefault();
                if (BackendTradeModel == null)
                {
                    //todo handle
                }
            }
        }

        #region Adding Interim

        public void AddTradeEntry()
        {
            TradeInputs.Add(new TradeInputModel()
            {
                TradeInputType = TradeInputType.Interim,
                TradeComponents = ComponentListsFactory.GetTradeEntryComponents(isActual: true)
            });

            UpdateInterimSummary();
        }

        public void AddTradeExit()
        {
            TradeInputs.Add(new TradeInputModel()
            {
                TradeInputType = TradeInputType.Interim,
                TradeComponents = ComponentListsFactory.GetTradeExitComponents()
            });
            UpdateInterimSummary();
        }

        #endregion

        #region Removing

        public void RemoveTradeInput(Guid tradeInputId)
        {
            var tradeInput = TradeInputs.Where(t => t.Id == tradeInputId && t.TradeInputType == TradeInputType.Interim).FirstOrDefault();

            if (tradeInput != null)
            {
                TradeInputs.Remove(tradeInput);
            }

            UpdateInterimSummary();
        }

        #endregion

        #region Updating Components

        public void UpdateTradeInputComponent(Guid tradeInputId, Guid componentId, string newContent)
        {
            var trade = TradeInputs.Where(t => t.Id == tradeInputId).FirstOrDefault();
            if (trade != null)
            {
                var component = trade.TradeComponents?.Where(c => c.Id == componentId).FirstOrDefault();
                if (component != null)
                {
                    component.History.Add(component.ContentWrapper);
                    component.ContentWrapper = new ContentModel() { Content = newContent };
                }
            }
        }

        #endregion

        #region Closure

        public void CloseMe()
        {
            RemoveInterimSummary();

            var analytics = GetAvgEntryAndProfit();

            TradeInputs.Add(new TradeInputModel()
            {
                TradeInputType = TradeInputType.Closure,
                TradeComponents = ComponentListsFactory.GetTradeClosureComponents(profitValue: analytics.profit.ToString())
            });
        }

        #endregion

        #region Summary

        public void AddInterimSummary()
        {
            var analytics = GetAvgEntryAndProfit();

            string averageEntry = string.Empty, totalAmount = string.Empty, totalCost = string.Empty;
            if (analytics.totalCost > 0)
            {
                totalCost = analytics.totalCost.ToString();

                if (analytics.totalAmount > 0)
                {
                    totalAmount = analytics.totalAmount.ToString();
                    averageEntry = (analytics.totalCost / analytics.totalAmount).ToString();
                }
            }

            TradeInputs.Add(new TradeInputModel()
            {
                TradeInputType = TradeInputType.Interim,
                TradeComponents = ComponentListsFactory.GetInterimSummaryComponents(averageEntry, totalAmount, totalCost)
            });

        }

        public void RemoveInterimSummary()
        {
            var activeSummary = TradeInputs.Where(t => t.TradeInputType == TradeInputType.InterimSummary).FirstOrDefault();
            if (activeSummary != null)
            {
                TradeInputs.Remove(activeSummary);
            }
        }

        public void UpdateInterimSummary()
        {
            RemoveInterimSummary();
            AddInterimSummary();
        }

        #endregion

        #region Helpers

        private (double totalCost, double totalAmount, double profit) GetAvgEntryAndProfit()
        {
            List<(double priceValue, double cost)> entriesWithAmount = new();
            double profit = 0.0;

            var interims = TradeInputs.Where(t => t.TradeInputType == TradeInputType.Interim);
            foreach (var tradeInput in interims)
            {
                if (tradeInput == null) //shouldn't happen
                    continue;

                double cost = 0.0;
                double priceValue = 0.0;
                foreach (var component in tradeInput.TradeComponents)
                {
                    if (component.CostRelevant == ValueRelevance.Add || component.CostRelevant == ValueRelevance.Substract)
                    {
                        double.TryParse(component.ContentWrapper.Content, out cost);

                        if (component.CostRelevant == ValueRelevance.Add)
                        {
                            profit += cost;
                        }

                        else if (component.CostRelevant == ValueRelevance.Substract)
                        {
                            profit -= cost;
                        }
                    }

                    if (component.PriceValueRelevant == ValueRelevance.Add || component.PriceValueRelevant == ValueRelevance.Substract)
                    {
                        double.TryParse(component.ContentWrapper.Content, out priceValue);
                        if (component.PriceValueRelevant == ValueRelevance.Substract)
                        {
                            priceValue *= -1;
                        }
                    }
                }

                //should have both change and price now
                if (cost > 0 && priceValue > 0)
                {
                    entriesWithAmount.Add((priceValue, cost));
                }
            }


            double totalAmount = 0.0;
            double totalCost = 0.0;

            foreach (var item in entriesWithAmount)
            {
                //will substract if exit trade
                totalCost += item.cost * item.priceValue;
                totalAmount += (item.cost / item.priceValue);
            }

            return (totalCost, totalAmount, profit);
        }

        #endregion
    }
}
