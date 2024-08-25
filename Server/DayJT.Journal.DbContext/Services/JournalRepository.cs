using DayJT.Journal.Data;
using DayJTrading.Journal.Data;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
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

        public async Task<TradeElement?> RemoveInterimEntry(string tradeId, string tradeInputId)
        {
            var trade = await GetTradeCompositeAsync(tradeId);
            JournalRepoHelpers.RemoveInterimInput(ref trade, tradeInputId);
            TradeElement? summary = JournalRepoHelpers.GetInterimSummary(trade);
            if (summary != null)
            {
                trade.Summary = summary;
            }

            await dataContext.SaveChangesAsync();

            return summary;
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

            // Create and add reduction entry for closing
            var tradeInput = CreateTradeElementForClosure(trade, closingPrice, analytics);
            trade.TradeElements.Add(tradeInput);

            // Generate summary based on updated trade
            trade.Summary = JournalRepoHelpers.GetInterimSummary(trade);

            await dataContext.SaveChangesAsync();

            return trade.Summary!;
        }

        private TradeElement CreateTradeElementForClosure(TradeComposite trade, string closingPrice, (double totalCost, double totalAmount, double profit) analytics)
        {
            // Create a TradeElement for ReducePosition
            var tradeInput = new TradeElement(trade, TradeActionType.ReducePosition);

            // Find price entry
            var priceEntry = tradeInput.Entries.SingleOrDefault(ti => ti.PriceRelevance == ValueRelevance.Substract);
            if (priceEntry == null)
            {
                throw new InvalidOperationException("Could not find price entry to reduce / close position");
            }
            priceEntry.Content = closingPrice;

            // Find cost entry
            var costEntry = tradeInput.Entries.SingleOrDefault(ti => ti.CostRelevance == ValueRelevance.Substract);
            if (costEntry == null)
            {
                throw new InvalidOperationException("Could not find cost entry to reduce / close position");
            }

            if (double.TryParse(closingPrice, out double closingPriceValue))
            {
                costEntry.Content = (closingPriceValue * analytics.totalAmount).ToString();
            }
            else
            {
                throw new FormatException("Could not parse closing price");
            }

            // Create TradeElement for Closure
            var tradeClosure = new TradeElement(trade, TradeActionType.Closure);
            tradeClosure.Entries = SummaryPositionsFactory.GetTradeClosureComponents(tradeClosure, profitValue: analytics.profit.ToString());

            return tradeInput; // Return tradeInput, as this is the entry we are adding
        }

        #endregion

        #region Helpers

        private async Task<TradeComposite> GetTradeCompositeAsync(string tradeId)
        {
            var trade = await dataContext.AllTradeComposites.Where(t => t.Id.ToString() == tradeId).SingleOrDefaultAsync();

            if (trade == null)
            {
                throw new InvalidOperationException($"Trade with ID {tradeId} not found.");
            }

            return trade!;
        }

        private async Task<(TradeElement newEntry, TradeElement summary)>  
                                                        AddInterimTradeInputAsync(TradeComposite trade, TradeElement tradeInput)
        {
            trade.TradeElements.Add(tradeInput);
            TradeElement newSummary = JournalRepoHelpers.GetInterimSummary(trade);

            if (newSummary == null)
            {
                throw new Exception("todo");
            }

            trade.Summary = newSummary;
            await dataContext.SaveChangesAsync();

            return (tradeInput, newSummary);
        }

        #endregion
    }
}
