using DayJT.Journal.Data;
using DayJTrading.Journal.Data;
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

        public async Task<TradeComposite> AddTradeCompositeAsync()
        {
            TradeComposite trade = new TradeComposite();
            try
            {
                TradeElement originElement = new TradeElement(trade, TradeActionType.Origin);
                trade.TradeElements.Add(originElement);

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

        public async Task<(TradeElement? element, TradeElement? summary)> GetTradeElement(string tradeId, string tradeElementId)
        {
            TradeElement? tradeElement = null;
            TradeElement? summary = null;

            var trade = await GetTradeCompositeAsync(tradeId);
            tradeElement = trade?.TradeElements.Where(t => t.Id.ToString() == tradeId).SingleOrDefault();

            if (tradeElement != null)
            {
                summary = JournalRepoHelpers.GetInterimSummary(trade);
            }

            return (tradeElement, summary);
        }

        public async Task<(TradeElement? newEntry, TradeElement? summary)> AddPositionAsync(string tradeId)
        {
            var trade = await GetTradeCompositeAsync(tradeId);
            TradeElement tradeInput = new TradeElement(trade, TradeActionType.AddPosition);
            return await AddInterimTradeInputAsync(trade, tradeInput);
        }

        public async Task<(TradeElement? newEntry, TradeElement? summary)> ReducePositionAsync(string tradeId)
        {
            var trade = await GetTradeCompositeAsync(tradeId);
            TradeElement tradeInput = new TradeElement(trade, TradeActionType.ReducePosition);

            return await AddInterimTradeInputAsync(trade, tradeInput);
        }

        public async Task<(bool result, TradeElement? summary)> RemoveInterimEntry(string tradeId, string tradeInputId)
        {
            var trade = await GetTradeCompositeAsync(tradeId);
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
            Cell? cell = await dataContext.AllEntries.Where(t => t.Id.ToString() == componentId).SingleOrDefaultAsync();
            if (cell != null)
            {
                cell.SetFollowupContent(newContent, changeNote);

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
            var trade = await GetTradeCompositeAsync(tradeId);
            var analytics = JournalRepoHelpers.GetAvgEntryAndProfit(trade);

            #region add reduction for current amount at specified price
            TradeElement tradeInput = new TradeElement(trade, TradeActionType.ReducePosition);

            Cell? price = tradeInput.Entries.Where(ti => ti.PriceRelevance == ValueRelevance.Substract).SingleOrDefault();
            if (price == null)
                throw new Exception("could not find price entry to reduce / close position");
            else
            {
                price.Content = closingPrice;
            }

            Cell? cost = tradeInput.Entries.Where(ti => ti.CostRelevance == ValueRelevance.Substract).SingleOrDefault();
            if (cost == null)
                throw new Exception("could not find cost entry to reduce / close position");
            else if (double.TryParse(closingPrice, out double amountValue))
            {
                cost.Content = (amountValue * analytics.totalAmount).ToString();
            }
            else
                throw new Exception("could not parse closing price to close position");

            trade.TradeElements.Add(tradeInput);
            #endregion


            analytics = JournalRepoHelpers.GetAvgEntryAndProfit(trade);
            TradeElement tradeClosure = new TradeElement(trade, TradeActionType.Closure);
            tradeInput.Entries = SummaryPositionsFactory.GetTradeClosureComponents(tradeClosure, profitValue: analytics.profit.ToString());

            trade.Summary = tradeClosure;

            await dataContext.SaveChangesAsync();

            return tradeClosure;
        }

        #endregion

        #region Helpers

        private async Task<TradeComposite?> GetTradeCompositeAsync(string tradeId)
        {
            return await dataContext.AllTradeComposites.Where(t => t.Id.ToString() == tradeId).SingleOrDefaultAsync();
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

        #endregion
    }
}
