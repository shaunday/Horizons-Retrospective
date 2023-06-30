using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.Diagnostics;
using TraJediServer.Journal;

namespace TraJedi.Journal.Data.Services
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

        public async Task<IEnumerable<TradeInfoSingleLine>> GetAllTradeInfoLinesAsync()
        {
            var _oneLiners = new List<TradeInfoSingleLine>();

            foreach (var trade in dataContext.OverallTrades)
            {
                TradeInfoSingleLine tradeSummary = new TradeInfoSingleLine()
                {
                    TradeActionType = TradeActionType.Overview1Liner,
                    TradeActionInfoCells = new List<Cell>()
                };

                tradeSummary.TradeActionInfoCells = await dataContext.TradeInputComponents
                  .Where(tc => tc.TradeInfoSingleLine.TradePositionCompositeId == trade.Id && tc.IsRelevantForOverview)
                  .OrderBy(tc => tc.Id)
                  .ToListAsync();

                _oneLiners.Add(tradeSummary);
            }

            return _oneLiners;
        }

        public async Task<TradePositionComposite> AddTradeCompositeAsync()
        {
            TradePositionComposite trade = new TradePositionComposite();
            trade.TradeActions.Add(new TradeInfoSingleLine()
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

        public async Task<TradeInfoSingleLine?> GetTradeOriginAsync(string tradeId)
        {
            return await GetTradeInputByTypeAsync(tradeId, TradeActionType.Origin);
        }

        public async Task<TradeInfoSingleLine?> GetTradeClosureAsync(string tradeId)
        {
            return await GetTradeInputByTypeAsync(tradeId, TradeActionType.Closure);
        }

        #endregion

        #region add/remove

        public async Task<(TradeInfoSingleLine newEntry, TradeInfoSingleLine summary)> NewEntryAddPositionAsync(string tradeId)
        {
            TradeInfoSingleLine tradeInput = new TradeInfoSingleLine()
            {
                TradeActionType = TradeActionType.Interim,
                TradeActionInfoCells = TradeInfoFactory.GetAddToPositionComponents(isActual: true)
            };

            return await AddInterimTradeInputAsync(tradeId, tradeInput);
        }

        public async Task<(TradeInfoSingleLine newEntry, TradeInfoSingleLine summary)> NewEntryReducePositionAsync(string tradeId)
        {
            TradeInfoSingleLine tradeInput = new TradeInfoSingleLine()
            {
                TradeActionType = TradeActionType.Interim,
                TradeActionInfoCells = TradeInfoFactory.GetReducePositionComponents()
            };

            return await AddInterimTradeInputAsync(tradeId, tradeInput);
        }

        public async Task<(bool result, TradeInfoSingleLine? summary)> RemoveInterimEntry(string tradeInputId)
        {
            bool res = await RemoveInterimInputAsync(tradeInputId);
            TradeInfoSingleLine? summary = null;

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

                await UpdateInterimSummaryAsync(inputComponent.TradeActionSingleLine.TradePositionCompositeId.ToString());

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
            TradeInfoSingleLine tradeInput = new TradeInfoSingleLine()
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
            TradeInfoSingleLine tradeClosure = new TradeInfoSingleLine()
            {
                TradeActionType = TradeActionType.Closure,
                TradeActionInfoCells = TradeInfoFactory.GetTradeClosureComponents(profitValue: analytics.profit.ToString())
            };

            await dataContext.TradeInputs.AddAsync(tradeClosure);

            await dataContext.SaveChangesAsync();
        }

        #endregion

        #region Helpers

        public async Task<TradeInfoSingleLine?> GetTradeInputByTypeAsync(string tradeId, TradeActionType tradeInputType)
        {
            return await dataContext.TradeInputs
               .Where(t => t.TradePositionCompositeId.ToString() == tradeId && t.TradeActionType == tradeInputType)
               .FirstOrDefaultAsync();
        }

        private async Task<bool> RemoveInterimInputAsync(string tradeInputId)
        {
            TradeInfoSingleLine? tradeInput =
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
            TradeInfoSingleLine? tradeInput =
                 await dataContext.TradeInputs.Where(t => t.TradeActionType == tradeInputType).FirstOrDefaultAsync();

            if (tradeInput != null)
            {
                dataContext.TradeInputs.Remove(tradeInput);
                return true;
            }

            return false;
        }

        private async Task<(TradeInfoSingleLine newEntry, TradeInfoSingleLine summary)> AddInterimTradeInputAsync(string tradeInputId, TradeInfoSingleLine tradeInput)
        {
            await dataContext.TradeInputs.AddAsync(tradeInput);
            TradeInfoSingleLine summary = await UpdateInterimSummaryAsync(tradeInputId);

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

        private async Task<TradeInfoSingleLine> AddInterimSummaryAsync(string tradeId)
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

            TradeInfoSingleLine tradeInput = new TradeInfoSingleLine()
            {
                TradeActionType = TradeActionType.Interim,
                TradeActionInfoCells = TradeInfoFactory.GetSummaryComponents(averageEntry, totalAmount, totalCost)
            };

            await dataContext.TradeInputs.AddAsync(tradeInput);
            return tradeInput;
        }

        private async Task<TradeInfoSingleLine> UpdateInterimSummaryAsync(string tradeInputId)
        {
            await RemoveInterimInputAsync(TradeActionType.InterimSummary);
            return await AddInterimSummaryAsync(tradeInputId);
        }

        #endregion
    }
}
