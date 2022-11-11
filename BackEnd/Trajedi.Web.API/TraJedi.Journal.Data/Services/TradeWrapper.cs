using TraJediServer.Journal;

namespace TraJedi.Journal.Data.Services
{
    public class TradeConstructAccess
    {
        #region Members

        private TradeConstruct _backendTradeModel = null!;

        private ICollection<TradeInputModel> TradeInputs
        {
            get => _backendTradeModel.TradeInputs;
            set => _backendTradeModel.TradeInputs = value;
        }

        private readonly TradingJournalDataContext dataContext;

        #endregion

        #region ctor

        public TradeConstructAccess(TradingJournalDataContext journalDbContext, Guid? id = null)
        {
            dataContext = journalDbContext ?? throw new ArgumentNullException(nameof(journalDbContext));

            if (id == null)
            {
                _backendTradeModel = new TradeConstruct();
                _backendTradeModel.TradeInputs.Add(new TradeInputModel()
                {
                    TradeInputType = TradeInputType.Origin,
                    TradeComponents = ComponentListsFactory.GetTradeOriginComponents()
                });

                dataContext.OverallTrades.Add(_backendTradeModel);
            }
            else
            {
                _backendTradeModel = dataContext.OverallTrades?.Where(t => t.Id == id).FirstOrDefault() ??
                                                            throw new ArgumentNullException(nameof(dataContext.OverallTrades));

            }
        }
        #endregion

        #region Getters

        public string GetIdString() => _backendTradeModel.Id.ToString();

        public TradeInputModel? GetTradeOrigin() => TradeInputs.Where(t => t.TradeInputType == TradeInputType.Origin).FirstOrDefault();

        public TradeInputModel? GetTradeClosure() => TradeInputs.Where(t => t.TradeInputType == TradeInputType.Closure).FirstOrDefault();

        public IEnumerable<TradeInputModel> GetTradeInterims() => TradeInputs.Where(ti => ti.TradeInputType == TradeInputType.Interim);

        public TradeInputModel? GetTradeSummary()
        {
            TradeInputModel tradeSummary = new TradeInputModel() 
            {
                TradeInputType = TradeInputType.OneLineSummation,
                TradeComponents = new List<InputComponentModel>() 
            };

            TradeInputModel? tradeOrigin = GetTradeOrigin();
            if (tradeOrigin != null)
            {
                foreach (var component in tradeOrigin.TradeComponents)
                {
                    if (component.IsRelevantForOneLineSummation)
                    {
                        tradeSummary.TradeComponents.Add(component);
                    }
                }
            }

            //todo expand

            return tradeSummary.TradeComponents.Count > 0 ? tradeSummary : null;
        }

        #endregion

        public InputComponentModel? UpdateTradeInputComponent(string tradeInputId, string componentId, string newContent)
        {
            var trade = TradeInputs.Where(t => t.Id.ToString() == tradeInputId).FirstOrDefault();
            if (trade != null)
            {
                var component = trade.TradeComponents?.Where(c => c.Id.ToString() == componentId).FirstOrDefault();
                if (component != null)
                {
                    component.History.Add(component.ContentWrapper);
                    component.ContentWrapper = new ContentModel() { Content = newContent };
                    trade.LastUpdatedAt = DateTime.Now;

                    UpdateInterimSummary();

                    dataContext.SaveChanges();

                    return component;
                }
            }

            return null;
        }

        #region Interim

        public TradeInputModel AddTradeEntry()
        {
            TradeInputModel tradeInput = new TradeInputModel()
            {
                TradeInputType = TradeInputType.Interim,
                TradeComponents = ComponentListsFactory.GetTradeEntryComponents(isActual: true)
            };

            TradeInputs.Add(tradeInput);
            UpdateInterimSummary();

            dataContext.SaveChanges();

            return tradeInput;
        }

        public TradeInputModel AddTradeExit()
        {
            TradeInputModel tradeInput = new TradeInputModel()
            {
                TradeInputType = TradeInputType.Interim,
                TradeComponents = ComponentListsFactory.GetTradeExitComponents()
            };

            TradeInputs.Add(tradeInput);
            UpdateInterimSummary();

            dataContext.SaveChanges();

            return tradeInput;
        }

        public void RemoveInterimInput(Guid tradeInputId)
        {
            var tradeInput = TradeInputs.Where(t => t.Id == tradeInputId && t.TradeInputType == TradeInputType.Interim).FirstOrDefault();

            if (tradeInput != null)
            {
                TradeInputs.Remove(tradeInput);
            }

            UpdateInterimSummary();

            dataContext.SaveChanges();
        }

        private void AddInterimSummary()
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

        private void RemoveInterimSummary()
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

            dataContext.SaveChanges();
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
                    if (component.CostRelevance == ValueRelevance.Add || component.CostRelevance == ValueRelevance.Substract)
                    {
                        double.TryParse(component.ContentWrapper.Content, out cost);

                        if (component.CostRelevance == ValueRelevance.Add)
                        {
                            profit += cost;
                        }

                        else if (component.CostRelevance == ValueRelevance.Substract)
                        {
                            profit -= cost;
                        }
                    }

                    if (component.PriceValueRelevance == ValueRelevance.Add || component.PriceValueRelevance == ValueRelevance.Substract)
                    {
                        double.TryParse(component.ContentWrapper.Content, out priceValue);
                        if (component.PriceValueRelevance == ValueRelevance.Substract)
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
                totalAmount += item.cost / item.priceValue;
            }

            return (totalCost, totalAmount, profit);
        }

        #endregion
    }
}
