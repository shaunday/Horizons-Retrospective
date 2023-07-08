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

        public async Task<IEnumerable<TradeComponent>> GetAllTradeComponentsAsync()
        {
            var _oneLiners = new List<TradeComponent>();

            foreach (var trade in dataContext.OverallTrades)
            {
                TradeComponent tradeSummary = new TradeComponent()
                {
                    TradeActionType = TradeActionType.Overview1Liner,
                    TradeActionInfoCells = new List<Cell>()
                };

                tradeSummary.TradeActionInfoCells = await dataContext.TradeInputComponents
                  .Where(tc => tc.TradeComponent.TradePositionCompositeId == trade.Id && tc.IsRelevantForOverview)
                  .OrderBy(tc => tc.Id)
                  .ToListAsync();

                _oneLiners.Add(tradeSummary);
            }

            return _oneLiners;
        }

        public async Task<TradePositionComposite> AddTradeCompositeAsync()
        {
            TradePositionComposite trade = new TradePositionComposite();
            trade.TradeComponents.Add(new TradeComponent()
            {
                TradeActionType = TradeActionType.Origin,
                TradeActionInfoCells = TradeInfoFactory.GetTradeOriginComponents()
            });
            await dataContext.OverallTrades.AddAsync(trade);
            await dataContext.SaveChangesAsync();

            return trade;
        }

        public async Task<(IEnumerable<TradePositionComposite>, PaginationMetadata)> GetAllTradeCompositesAsync(int pageNumber = 1, int pageSize = 10)
        {
            //collection to start from
            var collection = dataContext.OverallTrades as IQueryable<TradePositionComposite>;

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

        public async Task<(TradeComponent newEntry, TradeComponent summary)> NewEntryAddPositionAsync(string tradeId)
        {
            TradeComponent tradeInput = new TradeComponent()
            {
                TradeActionType = TradeActionType.Interim,
                TradeActionInfoCells = TradeInfoFactory.GetAddToPositionComponents(isActual: true)
            };

            return await AddInterimTradeInputAsync(tradeId, tradeInput);
        }

        public async Task<(TradeComponent newEntry, TradeComponent summary)> NewEntryReducePositionAsync(string tradeId)
        {
            TradeComponent tradeInput = new TradeComponent()
            {
                TradeActionType = TradeActionType.Interim,
                TradeActionInfoCells = TradeInfoFactory.GetReducePositionComponents()
            };

            return await AddInterimTradeInputAsync(tradeId, tradeInput);
        }

        public async Task<(bool result, TradeComponent? summary)> RemoveInterimEntry(string tradeInputId)
        {
            bool res = await RemoveInterimInputAsync(tradeInputId);
            TradeComponent? summary = null;

            if (res)
            {
                summary = await UpdateInterimSummaryAsync(tradeInputId);
                await dataContext.SaveChangesAsync();
            }

            return (res, summary);
        }

        #endregion

        public async Task<Cell?> UpdateCellContent(string componentId, string newContent, string changeNote)
        {
            Cell? inputComponent = await dataContext.TradeInputComponents.Where(t => t.Id.ToString() == componentId).FirstOrDefaultAsync();
            if (inputComponent != null)
            {
                inputComponent.History.Add(inputComponent.ContentWrapper);
                inputComponent.ContentWrapper = new CellContent() { Content = newContent, ChangeNote = changeNote };

                await UpdateInterimSummaryAsync(inputComponent.TradeComponent.TradePositionCompositeId.ToString());

                await dataContext.SaveChangesAsync();
            }

            return inputComponent;
        }

        #endregion

        #region Closure

        public async Task Close(string tradeId, string closingPrice)
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


            await dataContext.TradeInputs.AddAsync(tradeInput);
            #endregion

            // remove interim summary

            await RemoveInterimInputAsync(TradeActionType.InterimSummary);

            // create actual closure

            analytics = await GetAvgEntryAndProfitAsync(tradeId);
            TradeComponent tradeClosure = new TradeComponent()
            {
                TradeActionType = TradeActionType.Closure,
                TradeActionInfoCells = TradeInfoFactory.GetTradeClosureComponents(profitValue: analytics.profit.ToString())
            };

            await dataContext.TradeInputs.AddAsync(tradeClosure);

            await dataContext.SaveChangesAsync();
        }

        #endregion

        #region Helpers

        public async Task<TradeComponent?> GetTradeInputByTypeAsync(string tradeId, TradeActionType tradeInputType)
        {
            return await dataContext.TradeInputs
               .Where(t => t.TradePositionCompositeId.ToString() == tradeId && t.TradeActionType == tradeInputType)
               .FirstOrDefaultAsync();
        }

        private async Task<bool> RemoveInterimInputAsync(string tradeInputId)
        {
            TradeComponent? tradeInput =
                 await dataContext.TradeInputs.Where(t => t.Id.ToString() == tradeInputId).FirstOrDefaultAsync();

            if (tradeInput != null && tradeInput.TradeActionType == TradeActionType.Interim)
            {
                dataContext.TradeInputs.Remove(tradeInput);
                return true;
            }

            return false;
        }

        private async Task<bool> RemoveInterimInputAsync(TradeActionType tradeInputType)
        {
            TradeComponent? tradeInput =
                 await dataContext.TradeInputs.Where(t => t.TradeActionType == tradeInputType).FirstOrDefaultAsync();

            if (tradeInput != null)
            {
                dataContext.TradeInputs.Remove(tradeInput);
                return true;
            }

            return false;
        }

        private async Task<(TradeComponent newEntry, TradeComponent summary)> AddInterimTradeInputAsync(string tradeInputId, TradeComponent tradeInput)
        {
            await dataContext.TradeInputs.AddAsync(tradeInput);
            TradeComponent summary = await UpdateInterimSummaryAsync(tradeInputId);

            await dataContext.SaveChangesAsync();

            return (tradeInput, summary);
        }

        private async Task<(double totalCost, double totalAmount, double profit)> GetAvgEntryAndProfitAsync(string tradeId)
        {
            List<(double priceValue, double cost)> entriesWithAmount = new();
            double profit = 0.0;

            var interims = await dataContext.TradeInputs
                .Where(t => t.TradePositionCompositeId.ToString() == tradeId && t.TradeActionType == TradeActionType.Interim)
                .ToListAsync();

            foreach (var tradeInput in interims)
            {
                double cost = 0.0;
                double priceValue = 0.0;
                foreach (var component in tradeInput.TradeActionInfoCells)
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

                    if (component.PriceRelevance == ValueRelevance.Add || component.PriceRelevance == ValueRelevance.Substract)
                    {
                        double.TryParse(component.ContentWrapper.Content, out priceValue);
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

        private async Task<TradeComponent> AddInterimSummaryAsync(string tradeId)
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

            TradeComponent tradeInput = new TradeComponent()
            {
                TradeActionType = TradeActionType.Interim,
                TradeActionInfoCells = TradeInfoFactory.GetSummaryComponents(averageEntry, totalAmount, totalCost)
            };

            await dataContext.TradeInputs.AddAsync(tradeInput);
            return tradeInput;
        }

        private async Task<TradeComponent> UpdateInterimSummaryAsync(string tradeInputId)
        {
            await RemoveInterimInputAsync(TradeActionType.InterimSummary);
            return await AddInterimSummaryAsync(tradeInputId);
        }

        #endregion
    }
}
