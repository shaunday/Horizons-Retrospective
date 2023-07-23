using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.Diagnostics;
using TraJediServer.Journal;

namespace DayJT.Journal.Data.Services
{
    public class JournalRepository : IJournalRepository
    {
        private readonly TradingJournalDataContext dataContext;

        #region Ctor

        public JournalRepository(TradingJournalDataContext journalDbContext)
        {
            dataContext = journalDbContext ?? throw new ArgumentNullException(nameof(journalDbContext));

        }
        #endregion

        #region Trades 

        public async Task<IEnumerable<TradeComponent>> GetAllTradeCompositesAs1LinerOverviewAsync()
        {
            var _oneLiners = new List<TradeComponent>();

            foreach (var trade in dataContext.AllTradeComposites)
            {
                TradeComponent tradeSummary = new TradeComponent()
                {
                    TradeActionType = TradeActionType.Overview1Liner,
                    TradeActionInfoCells = new List<Cell>()
                };

                tradeSummary.TradeActionInfoCells = await dataContext.AllTradeInfoCells
                  .Where(tc => tc.TradeComponentRef.TradePositionCompositeRefId == trade.Id && tc.IsRelevantForOverview)
                  .OrderBy(tc => tc.Id)
                  .ToListAsync();

                _oneLiners.Add(tradeSummary);
            }

            return _oneLiners;
        }

        public async Task<TradePositionComposite> AddTradeCompositeAsync()
        {
            TradePositionComposite trade = new TradePositionComposite();
            try
            {
               
                trade.TradeComponents.Add(new TradeComponent()
                {
                    TradeActionType = TradeActionType.Origin,
                    TradeActionInfoCells = TradeInfoFactory.GetTradeOriginComponents()
                });
                dataContext.AllTradeComposites.Add(trade);
                await dataContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                string t = ex.ToString();
            }
            return trade;
        }

        public async Task<(IEnumerable<TradePositionComposite>, PaginationMetadata)> GetAllTradeCompositesAsync(int pageNumber = 1, int pageSize = 10)
        {
            //collection to start from
            var collection = dataContext.AllTradeComposites as IQueryable<TradePositionComposite>;

            var totalItemCount = await collection.CountAsync();

            var paginationMetadata = new PaginationMetadata(totalItemCount, pageSize, pageNumber);

            var collectionToReturn = await collection.OrderBy(t => t.CreatedAt)
                .Skip(pageSize * (pageNumber - 1))
                .Take(pageSize)
                .ToListAsync();

            return (collectionToReturn, paginationMetadata);
        }

        #endregion

        #region Trade Inputs 

        #region Getters

        public async Task<TradeComponent?> GetTradeOriginAsync(string tradeId)
        {
            return await GetTradeInputByTypeAsync(tradeId, TradeActionType.Origin);
        }

        public async Task<TradeComponent?> GetTradeClosureAsync(string tradeId)
        {
            return await GetTradeInputByTypeAsync(tradeId, TradeActionType.Closure);
        }

        #endregion

        #region add/remove

        public async Task<(TradeComponent? newEntry, TradeComponent? summary)> NewEntryAddPositionAsync(string tradeId)
        {
            TradeComponent tradeInput = new TradeComponent()
            {
                TradeActionType = TradeActionType.Interim,
                TradeActionInfoCells = TradeInfoFactory.GetAddToPositionComponents(isActual: true)
            };

            return await AddInterimTradeInputAsync(tradeId, tradeInput);
        }

        public async Task<(TradeComponent? newEntry, TradeComponent? summary)> NewEntryReducePositionAsync(string tradeId)
        {
            TradeComponent tradeInput = new TradeComponent()
            {
                TradeActionType = TradeActionType.Interim,
                TradeActionInfoCells = TradeInfoFactory.GetReducePositionComponents()
            };

            return await AddInterimTradeInputAsync(tradeId, tradeInput);
        }

        public async Task<(bool result, TradeComponent? summary)> RemoveInterimEntry(string tradeId, string tradeInputId)
        {
            bool res = await RemoveInterimInputAsync(tradeId, tradeInputId);
            TradeComponent? summary = null;

            if (res)
            {
                summary = await UpdateInterimSummaryAsync(tradeId);
                await dataContext.SaveChangesAsync();
            }

            return (res, summary);
        }

        #endregion

        public async Task<Cell?> UpdateCellContent(string componentId, string newContent, string changeNote)
        {
            Cell? inputComponent = await dataContext.AllTradeInfoCells.Where(t => t.Id.ToString() == componentId).FirstOrDefaultAsync();
            //if (inputComponent != null)
            //{
            //    //inputComponent.History.Add(inputComponent.ContentWrapper);
            //    inputComponent.ContentWrapper = new CellContent() { Content = newContent, ChangeNote = changeNote };

            //    await UpdateInterimSummaryAsync(inputComponent.TradeComponentRef.TradePositionCompositeRefId.ToString());

            //    await dataContext.SaveChangesAsync();
            //}

            return inputComponent;
        }

        #endregion

        #region Closure

        public async Task CloseAsync(string tradeId, string closingPrice)
        {
            var analytics = await GetAvgEntryAndProfitAsync(tradeId);

            #region add reduction for current amount at specified price
            TradeComponent tradeInput = new TradeComponent()
            {
                TradeActionType = TradeActionType.Interim,
                TradeActionInfoCells = TradeInfoFactory.GetReducePositionComponents()
            };

            Cell? price = tradeInput.TradeActionInfoCells.Where(ti => ti.PriceRelevance == ValueRelevance.Substract).FirstOrDefault();
            if (price == null)
                throw new Exception("could not find price entry to reduce / close position");
            else
            {
                price.Content = closingPrice;
            }

            Cell? cost = tradeInput.TradeActionInfoCells.Where(ti => ti.CostRelevance == ValueRelevance.Substract).FirstOrDefault();
            if (cost == null)
                throw new Exception("could not find cost entry to reduce / close position");
            else if (double.TryParse(closingPrice, out double amountValue))
            {
                cost.Content = (amountValue * analytics.totalAmount).ToString();
            }
            else
                throw new Exception("could not parse closing price to close position");


            dataContext.AllTradeComposites.Where(tc => tc.Id.ToString() == tradeId).FirstOrDefault()?.TradeComponents.Add(tradeInput);
            #endregion

            // remove interim summary

            await RemoveInterimInputAsync(tradeId, TradeActionType.InterimSummary);

            // create actual closure

            analytics = await GetAvgEntryAndProfitAsync(tradeId);
            TradeComponent tradeClosure = new TradeComponent()
            {
                TradeActionType = TradeActionType.Closure,
                TradeActionInfoCells = TradeInfoFactory.GetTradeClosureComponents(profitValue: analytics.profit.ToString())
            };

            dataContext.AllTradeComposites.Where(tc => tc.Id.ToString() == tradeId).FirstOrDefault()?.TradeComponents.Add(tradeClosure);

            await dataContext.SaveChangesAsync();
        }

        #endregion

        #region Helpers

        public async Task<TradeComponent?> GetTradeInputByTypeAsync(string tradeId, TradeActionType tradeInputType)
        {
            return await dataContext.AllTradeComponents
               .Where(t => t.TradePositionCompositeRefId.ToString() == tradeId && t.TradeActionType == tradeInputType)
               .FirstOrDefaultAsync();
        }

        private async Task<bool> RemoveInterimInputAsync(string tradeId, string tradeInputId)
        {
            var trade = await dataContext.AllTradeComposites.Where(t => t.Id.ToString() == tradeId).SingleOrDefaultAsync();

            if (trade != null)
            {
                var tradeInputToRemove = trade.TradeComponents.Where(t => t.Id.ToString() == tradeInputId).SingleOrDefault();

                if (tradeInputToRemove != null && tradeInputToRemove.TradeActionType == TradeActionType.Interim)
                {
                    trade.TradeComponents.Remove(tradeInputToRemove);
                    return true;
                }
            }

            return false;
        }

        private async Task<bool> RemoveInterimInputAsync(string tradeId, TradeActionType tradeInputType)
        {
            var trade = await dataContext.AllTradeComposites.Where(t => t.Id.ToString() == tradeId).SingleOrDefaultAsync();

            if (trade != null)
            {
                var tradeInputToRemove = trade.TradeComponents.Where(t => t.TradeActionType == tradeInputType).SingleOrDefault();

                if (tradeInputToRemove != null)
                {
                    trade.TradeComponents.Remove(tradeInputToRemove);
                    return true;
                }
            }

            return false;
        }

        private async Task<(TradeComponent? newEntry, TradeComponent? summary)> AddInterimTradeInputAsync(string tradeId, TradeComponent tradeInput)
        {
            TradeComponent? summary = null;
            var trade = await dataContext.AllTradeComposites.Where(tc => tc.Id.ToString() == tradeId).SingleOrDefaultAsync();

            if (trade != null)
            {
                trade.TradeComponents.Add(tradeInput);
                summary = await UpdateInterimSummaryAsync(tradeId);

                await dataContext.SaveChangesAsync();
            }

            return (tradeInput, summary);
        }

        private async Task<(double totalCost, double totalAmount, double profit)> GetAvgEntryAndProfitAsync(string tradeId)
        {
            List<(double priceValue, double cost)> entriesWithAmount = new();
            double profit = 0.0;

            var interims = await dataContext.AllTradeComponents
                .Where(t => t.TradePositionCompositeRefId.ToString() == tradeId && t.TradeActionType == TradeActionType.Interim)
                .ToListAsync();

            foreach (var tradeInput in interims)
            {
                double cost = 0.0;
                double priceValue = 0.0;
                foreach (var component in tradeInput.TradeActionInfoCells)
                {
                    if (component.CostRelevance == ValueRelevance.Add || component.CostRelevance == ValueRelevance.Substract)
                    {
                        double.TryParse(component.Content, out cost);

                        if (component.CostRelevance == ValueRelevance.Add)
                        {
                            profit += cost;
                        }

                        else if (component.CostRelevance == ValueRelevance.Substract)
                        {
                            profit -= cost;
                        }
                    }

                    if (component.PriceRelevance == ValueRelevance.Add || component.PriceRelevance == ValueRelevance.Substract)
                    {
                        double.TryParse(component.Content, out priceValue);
                        if (component.PriceRelevance == ValueRelevance.Substract)
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


        //summary

        private async Task<TradeComponent?> AddInterimSummaryAsync(string tradeId)
        {
            var trade = await dataContext.AllTradeComposites.Where(tc => tc.Id.ToString() == tradeId).SingleOrDefaultAsync();
            TradeComponent? tradeInput = null;

            if (trade != null) 
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


                tradeInput = new TradeComponent()
                {
                    TradeActionType = TradeActionType.Interim,
                    TradeActionInfoCells = TradeInfoFactory.GetSummaryComponents(averageEntry, totalAmount, totalCost)
                };

                trade.TradeComponents.Add(tradeInput);
            }
           
            return tradeInput;
        }

        private async Task<TradeComponent?> UpdateInterimSummaryAsync(string tradeId)
        {
            await RemoveInterimInputAsync(tradeId, TradeActionType.InterimSummary);
            return await AddInterimSummaryAsync(tradeId);
        }

        #endregion
    }
}
