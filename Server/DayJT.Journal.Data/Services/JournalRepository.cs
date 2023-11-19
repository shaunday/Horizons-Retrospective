using Microsoft.AspNetCore.Mvc.Abstractions;
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

            //var allTrades = dataContext.AllTradeComposites.SelectMany(t => t.TradeComponents)
            //                                              .SelectMany(tc => tc.TradeActionInfoCells)
            //                                              .Where(aic => aic.IsRelevantForOverview);
                

            var allTrades = dataContext.AllTradeComposites;
            foreach (var trade in allTrades)
            {
                TradeComponent tradeSummary = new TradeComponent()
                {
                    TradeActionType = TradeActionType.Overview1Liner,
                    TradeActionInfoCells = new List<Cell>()
                };

                foreach (var tc in trade.TradeComponents)
                {
                    foreach (var actionInfo in tc.TradeActionInfoCells)
                    {
                        if (actionInfo.IsRelevantForOverview)
                        {
                            tradeSummary.TradeActionInfoCells.Add(actionInfo);
                        }
                    }
                }

                tradeSummary.TradeActionInfoCells = tradeSummary.TradeActionInfoCells.OrderBy(aic => aic.ContentWrapper.CreatedAt).ToList();
                _oneLiners.Add(tradeSummary);
            }

            _oneLiners = _oneLiners.OrderBy(t => t.CreatedAt).ToList();
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

        public async Task<(TradeComponent? newEntry, TradeComponent? summary)> NewEntryAddPositionAsync(string tradeId)
        {
            TradeComponent tradeInput = new TradeComponent()
            {
                TradeActionType = TradeActionType.Interim,
                TradeActionInfoCells = TradeInfoFactory.GetAddToPositionComponents(isActual: true)
            };

            var trade = GetTradeComposite(tradeId);
            return await AddInterimTradeInputAsync(trade, tradeInput);
        }

        public async Task<(TradeComponent? newEntry, TradeComponent? summary)> NewEntryReducePositionAsync(string tradeId)
        {
            TradeComponent tradeInput = new TradeComponent()
            {
                TradeActionType = TradeActionType.Interim,
                TradeActionInfoCells = TradeInfoFactory.GetReducePositionComponents()
            };

            var trade = GetTradeComposite(tradeId);
            return await AddInterimTradeInputAsync(trade, tradeInput);
        }

        public async Task<(bool result, TradeComponent? summary)> RemoveInterimEntry(string tradeId, string tradeInputId)
        {
            var trade = GetTradeComposite(tradeId);
            bool res = JournalRepoHelpers.RemoveInterimInput(trade, tradeInputId);
            TradeComponent? summary = null;

            if (res)
            {
                summary = JournalRepoHelpers.UpdateInterimSummary(trade);
                await dataContext.SaveChangesAsync();
            }

            return (res, summary);
        }

        #endregion

        #region Cell Content
        public async Task<(Cell? updatedCell, TradeComponent? summary)> UpdateCellContent(string componentId, string newContent, string changeNote)
        {
            TradeComponent? summary = null;
            Cell? cell = await dataContext.AllTradeInfoCells.Where(t => t.Id.ToString() == componentId).FirstOrDefaultAsync();
            if (cell != null)
            {
                cell.History.Add(cell.ContentWrapper);
                cell.ContentWrapper = new CellContent() { Content = newContent, ChangeNote = changeNote };

                if (cell.IsRelevantForOverview)
                {
                    summary = JournalRepoHelpers.UpdateInterimSummary(cell.TradeComponentRef.TradePositionCompositeRef);
                }

                await dataContext.SaveChangesAsync();
            }

            return (cell, summary);
        } 
        #endregion


        #region Closure

        public async Task CloseAsync(string tradeId, string closingPrice)
        {
            var trade = GetTradeComposite(tradeId);
            var analytics = JournalRepoHelpers.GetAvgEntryAndProfit(trade);

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

            JournalRepoHelpers.RemoveInterimInput(trade, TradeActionType.InterimSummary);

            // create actual closure

            analytics = JournalRepoHelpers.GetAvgEntryAndProfit(trade);
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

        private TradePositionComposite? GetTradeComposite(string tradeId)
        {
            return dataContext.AllTradeComposites.Where(t => t.Id.ToString() == tradeId).SingleOrDefault();
        }

        private async Task<(TradeComponent? newEntry, TradeComponent? summary)>  
                                                        AddInterimTradeInputAsync(TradePositionComposite trade, TradeComponent tradeInput)
        {
            TradeComponent? summary = null;

            if (trade != null)
            {
                trade.TradeComponents.Add(tradeInput);
                summary = JournalRepoHelpers.UpdateInterimSummary(trade);

                await dataContext.SaveChangesAsync();
            }

            return (tradeInput, summary);
        }

        #endregion
    }
}
