using DayJT.Journal.Data;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.Diagnostics;

namespace DayJT.Journal.DataContext.Services
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

        //public async Task<IEnumerable<TradeElement>> GetAllTradeCompositesAs1LinerOverviewAsync()
        //{
        //    //var allTrades1 = dataContext.AllTradeComposites.Select(tc => tc.TradeElements
        //    //                                              .SelectMany(te => te.Entries
        //    //                                              .Where(data => data.IsRelevantForOverview))
        //    //                                              .OrderBy(data => data.ContentWrapper.CreatedAt).ToList());

        //    var _oneLiners = new List<TradeElement>();

        //    await Task.Run(() =>
        //    {
        //        var allTrades = dataContext.AllTradeComposites;
        //        foreach (var trade in allTrades)
        //        {
        //            TradeElement tradeOverview = new TradeElement()
        //            {
        //                TradeActionType = TradeActionType.Overview1Liner,
        //                Entries = new List<Cell>()
        //            };

        //            foreach (var tc in trade.TradeElements)
        //            {
        //                foreach (var actionInfo in tc.Entries)
        //                {
        //                    if (actionInfo.IsRelevantForOverview)
        //                    {
        //                        tradeOverview.Entries.Add(actionInfo);
        //                    }
        //                }
        //            }

        //            tradeOverview.Entries = tradeOverview.Entries.OrderBy(aic => aic.ContentWrapper.CreatedAt).ToList();
        //            _oneLiners.Add(tradeOverview);
        //        }

        //        _oneLiners = _oneLiners.OrderBy(t => t.CreatedAt).ToList();
        //    });

        //    return _oneLiners;
        //}

        public async Task<TradeComposite> AddTradeCompositeAsync()
        {
            TradeComposite trade = new TradeComposite();
            try
            {
               
                trade.TradeElements.Add(new TradeElement(trade)
                {
                    TradeActionType = TradeActionType.Origin,
                    Entries = TradeElementFactory.GetTradeOriginComponents(),
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

        public async Task<(IEnumerable<TradeComposite>, PaginationMetadata)> GetAllTradeCompositesAsync(int pageNumber = 1, int pageSize = 10)
        {
            //collection to start from
            var collection = dataContext.AllTradeComposites as IQueryable<TradeComposite>;

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

        public async Task<(TradeElement? newEntry, TradeElement? summary)> AddPositionAsync(string tradeId)
        {
            var trade = GetTradeComposite(tradeId);
            TradeElement tradeInput = new TradeElement(trade)
            {
                TradeActionType = TradeActionType.Interim,
                Entries = TradeElementFactory.GetAddToPositionComponents()
            };

            return await AddInterimTradeInputAsync(trade, tradeInput);
        }

        public async Task<(TradeElement? newEntry, TradeElement? summary)> ReducePositionAsync(string tradeId)
        {
            var trade = GetTradeComposite(tradeId);
            TradeElement tradeInput = new TradeElement(trade)
            {
                TradeActionType = TradeActionType.Interim,
                Entries = TradeElementFactory.GetReducePositionComponents()
            };

            return await AddInterimTradeInputAsync(trade, tradeInput);
        }

        public async Task<(bool result, TradeElement? summary)> RemoveInterimEntry(string tradeId, string tradeInputId)
        {
            var trade = GetTradeComposite(tradeId);
            bool res = JournalRepoHelpers.RemoveInterimInput(trade, tradeInputId);
            TradeElement? summary = null;

            if (res)
            {
                summary = JournalRepoHelpers.GetInterimSummary(trade);
                if (summary != null)
                {
                    trade.Summary = summary;
                }

                await dataContext.SaveChangesAsync();
            }

            return (res, summary);
        }

        #endregion

        #region Cell Content
        public async Task<(Cell? updatedCell, TradeElement? summary)> UpdateCellContent(string componentId, string newContent, string changeNote)
        {
            TradeElement? summary = null;
            Cell? cell = await dataContext.AllTradeInfoCells.Where(t => t.Id.ToString() == componentId).FirstOrDefaultAsync();
            if (cell != null)
            {
                cell.History.Add(cell.ContentWrapper);
                cell.ContentWrapper = new CellContent() { Content = newContent, ChangeNote = changeNote, CellRef = cell, CellRefId = cell.Id };

                if (cell.IsRelevantForOverview)
                {
                    var trade = cell.TradeElementRef.TradeCompositeRef;
                    summary = JournalRepoHelpers.GetInterimSummary(trade);
                    trade.Summary = summary;
                }

                await dataContext.SaveChangesAsync();
            }

            return (cell, summary);
        } 
        #endregion


        #region Closure

        public async Task<TradeElement> CloseAsync(string tradeId, string closingPrice)
        {
            var trade = GetTradeComposite(tradeId);
            var analytics = JournalRepoHelpers.GetAvgEntryAndProfit(trade);

            #region add reduction for current amount at specified price
            TradeElement tradeInput = new TradeElement(trade)
            {
                TradeActionType = TradeActionType.Interim,
                Entries = TradeElementFactory.GetReducePositionComponents(),
            };

            Cell? price = tradeInput.Entries.Where(ti => ti.PriceRelevance == ValueRelevance.Substract).FirstOrDefault();
            if (price == null)
                throw new Exception("could not find price entry to reduce / close position");
            else
            {
                price.Content = closingPrice;
            }

            Cell? cost = tradeInput.Entries.Where(ti => ti.CostRelevance == ValueRelevance.Substract).FirstOrDefault();
            if (cost == null)
                throw new Exception("could not find cost entry to reduce / close position");
            else if (double.TryParse(closingPrice, out double amountValue))
            {
                cost.Content = (amountValue * analytics.totalAmount).ToString();
            }
            else
                throw new Exception("could not parse closing price to close position");


            dataContext.AllTradeComposites.Where(tc => tc.Id.ToString() == tradeId).FirstOrDefault()?.TradeElements.Add(tradeInput);
            #endregion


            analytics = JournalRepoHelpers.GetAvgEntryAndProfit(trade);
            TradeElement tradeClosure = new TradeElement(trade)
            {
                TradeActionType = TradeActionType.Closure,
                Entries = TradeElementFactory.GetTradeClosureComponents(profitValue: analytics.profit.ToString())
            };

            trade.Summary = tradeClosure;

            await dataContext.SaveChangesAsync();

            return tradeClosure;
        }

        #endregion

        #region Helpers

        private TradeComposite? GetTradeComposite(string tradeId)
        {
            return dataContext.AllTradeComposites.Where(t => t.Id.ToString() == tradeId).SingleOrDefault();
        }

        private async Task<(TradeElement? newEntry, TradeElement? summary)>  
                                                        AddInterimTradeInputAsync(TradeComposite trade, TradeElement tradeInput)
        {
            TradeElement? newSummary = null;

            if (trade != null)
            {
                trade.TradeElements.Add(tradeInput);
                newSummary = JournalRepoHelpers.GetInterimSummary(trade);

                if (newSummary == null)
                {
                    throw new Exception("todo");
                }

                trade.Summary = newSummary;
                await dataContext.SaveChangesAsync();
            }

            return (tradeInput, newSummary);
        }

        internal async Task UpdateSummaryAsync(string tradeId, TradeElement newSummary)
        {
            var trade = await dataContext.AllTradeComposites.Where(tc => tc.Id.ToString() == tradeId).FirstOrDefaultAsync();
            if (trade != null)
            {
                trade.Summary = newSummary;
            };
        }

        #endregion
    }
}
