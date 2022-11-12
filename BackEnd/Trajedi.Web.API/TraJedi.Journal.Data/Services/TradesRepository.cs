using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.Diagnostics;
using TraJediServer.Journal;

namespace TraJedi.Journal.Data.Services
{
    public class TradesRepository : ITradesRepository
    {
        private readonly TradingJournalDataContext dataContext;

        #region Ctor

        public TradesRepository(TradingJournalDataContext journalDbContext)
        {
            dataContext = journalDbContext ?? throw new ArgumentNullException(nameof(journalDbContext));

        }
        #endregion

        #region Trades Access
        public async Task<IEnumerable<TradeInputModel>> GetAllTradesOneLinerSummariesAsync()
        {
            var _oneLiners = new List<TradeInputModel>();

            foreach (var trade in dataContext.OverallTrades)
            {
                TradeInputModel tradeSummary = new TradeInputModel()
                {
                    TradeInputType = TradeInputType.OneLineSummation,
                    TradeComponents = new List<InputComponentModel>()
                };

                tradeSummary.TradeComponents = await dataContext.TradeInputComponents
                  .Where(tc => tc.TradeInputModel.TradeConstructId == trade.Id && tc.IsRelevantForOneLineSummation)
                  .OrderBy(tc => tc.Id)
                  .ToListAsync();

                _oneLiners.Add(tradeSummary);
            }

            return _oneLiners;
        }

        public async Task<TradeConstruct> AddTradeAsync()
        {
            TradeConstruct trade = new TradeConstruct();
            trade.TradeInputs.Add(new TradeInputModel()
            {
                TradeInputType = TradeInputType.Origin,
                TradeComponents = ComponentListsFactory.GetTradeOriginComponents()
            });
            await dataContext.OverallTrades.AddAsync(trade);
            await dataContext.SaveChangesAsync();

            return trade;
        }

        public async Task<IEnumerable<TradeConstruct>> GetAllTradesAsync()
        {
            return await dataContext.OverallTrades.ToListAsync();
        }
        #endregion

        #region Trade Inputs Access

        public async Task<TradeInputModel?> GetTradeOriginAsync(string tradeId)
        {
            return await dataContext.TradeInputs
                 .Where(t => t.TradeConstructId.ToString() == tradeId && t.TradeInputType == TradeInputType.Origin)
                 .FirstOrDefaultAsync();
        }

        public async Task<TradeInputModel?> GetTradeClosureAsync(string tradeId)
        {
            return await dataContext.TradeInputs
               .Where(t => t.TradeConstructId.ToString() == tradeId && t.TradeInputType == TradeInputType.Closure)
               .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<TradeInputModel>> GetTradeInterimsAsync(string tradeId)
        {
            return await dataContext.TradeInputs
               .Where(t => t.TradeConstructId.ToString() == tradeId && t.TradeInputType == TradeInputType.Interim)
               .ToListAsync();
        }

        public async Task<TradeInputModel?> CreateTradeClosureAsync(string tradeId)
        {
            await RemoveInterimInputByTypeAsync(tradeId, TradeInputType.InterimSummary);

            var analytics = await GetAvgEntryAndProfitAsync(tradeId);

            TradeInputModel tradeClosure = new TradeInputModel()
            {
                TradeInputType = TradeInputType.Closure,
                TradeComponents = ComponentListsFactory.GetTradeClosureComponents(profitValue: analytics.profit.ToString())
            };

            await dataContext.TradeInputs.AddAsync(tradeClosure);

            await dataContext.SaveChangesAsync();

            return tradeClosure;
        }

        #region Interims add/remove

        public async Task<TradeInputModel> AddTradeEntryAsync(string tradeId)
        {
            TradeInputModel tradeInput = new TradeInputModel()
            {
                TradeInputType = TradeInputType.Interim,
                TradeComponents = ComponentListsFactory.GetTradeEntryComponents(isActual: true)
            };

            return await AddTradeInputAsync(tradeId, tradeInput);
        }

        public async Task<TradeInputModel> AddTradeExitAsync(string tradeId)
        {
            TradeInputModel tradeInput = new TradeInputModel()
            {
                TradeInputType = TradeInputType.Interim,
                TradeComponents = ComponentListsFactory.GetTradeExitComponents()
            };

            return await AddTradeInputAsync(tradeId, tradeInput);
        }

        private async Task<TradeInputModel> AddInterimSummaryAsync(string tradeId)
        {
            var analytics = await GetAvgEntryAndProfitAsync(tradeId);

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

            TradeInputModel tradeInput = new TradeInputModel()
            {
                TradeInputType = TradeInputType.Interim,
                TradeComponents = ComponentListsFactory.GetInterimSummaryComponents(averageEntry, totalAmount, totalCost)
            };

            return await AddTradeInputAsync(tradeId, tradeInput);
        }

        public async Task RemoveInterimInputByTypeAsync(string tradeInputId, TradeInputType tradeInputType)
        {
            TradeInputModel? tradeInput =
                 await dataContext.TradeInputs.Where(t => t.Id.ToString() == tradeInputId && t.TradeInputType == tradeInputType).FirstOrDefaultAsync();

            if (tradeInput != null)
                dataContext.TradeInputs.Remove(tradeInput);
        }

        public async Task UpdateInterimSummaryAsync(string tradeInputId)
        {
            await RemoveInterimInputByTypeAsync(tradeInputId, TradeInputType.InterimSummary);
            await AddInterimSummaryAsync(tradeInputId);
        }

        #endregion

        public async Task<InputComponentModel?> UpdateTradeInputComponent(string componentId, string newContent)
        {
            InputComponentModel? inputComponent = await dataContext.TradeInputComponents.Where(t => t.Id.ToString() == componentId).FirstOrDefaultAsync();
            if (inputComponent != null)
            {
                inputComponent.History.Add(inputComponent.ContentWrapper);
                inputComponent.ContentWrapper = new ContentModel() { Content = newContent };

                await UpdateInterimSummaryAsync(inputComponent.TradeInputModel.TradeConstructId.ToString());

                await dataContext.SaveChangesAsync();
            }

            return inputComponent;
        }

        #endregion

        #region Helpers

        private async Task<TradeInputModel> AddTradeInputAsync(string tradeInputId, TradeInputModel tradeInput)
        {
            await dataContext.TradeInputs.AddAsync(tradeInput);
            await UpdateInterimSummaryAsync(tradeInputId);

            await dataContext.SaveChangesAsync();

            return tradeInput;
        }

        private async Task<(double totalCost, double totalAmount, double profit)> GetAvgEntryAndProfitAsync(string tradeId)
        {
            List<(double priceValue, double cost)> entriesWithAmount = new();
            double profit = 0.0;

            var interims = await dataContext.TradeInputs
                .Where(t => t.TradeConstructId.ToString() == tradeId && t.TradeInputType == TradeInputType.Interim)
                .ToListAsync();

            foreach (var tradeInput in interims)
            {
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
