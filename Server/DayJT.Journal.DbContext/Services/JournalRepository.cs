using DayJT.Journal.Data;
using DayJTrading.Common.Extenders;
using DayJTrading.Journal.Data;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.Metrics;

namespace DayJT.Journal.DataContext.Services
{
    public class JournalRepository : IJournalRepository
    {
        private readonly TradingJournalDataContext dataContext;

        public JournalRepository(TradingJournalDataContext journalDbContext)
        {
            dataContext = journalDbContext ?? throw new ArgumentNullException(nameof(journalDbContext));
        }

        //Trade Composite 

        public async Task<(IEnumerable<TradeComposite>, PaginationMetadata)> GetAllTradeCompositesAsync(int pageNumber = 1, int pageSize = 10)
        {
            var trades = await dataContext.AllTradeComposites
                                         .AsNoTracking()
                                         .OrderBy(t => t.Id)  // Order by the actual ID to ensure correct sequential order
                                         .Skip((pageNumber - 1) * pageSize)
                                         .Take(pageSize)
                                         .ToListAsync();

            // Ensure there are trades
            if (trades == null || !trades.Any())
            {
                throw new InvalidOperationException("Could not get any trades.");
            }

            // Get total count of records
            var totalCount = await dataContext.AllTradeComposites.CountAsync();

            // Create pagination metadata
            var paginationMetadata = new PaginationMetadata(totalCount, pageSize, pageNumber)
            {
                TotalPageCount = (int)Math.Ceiling(totalCount / (double)pageSize)
            };

            // Return paginated results with metadata
            return (trades, paginationMetadata);
        }

        public async Task<TradeComposite> AddTradeCompositeAsync()
        {
            TradeComposite trade = new TradeComposite();

            TradeElement originElement = new TradeElement(trade, TradeActionType.Origin);
            trade.TradeElements.Add(originElement);

            dataContext.AllTradeComposites.Add(trade);
            await dataContext.SaveChangesAsync();
            return trade;
        }

        //Trade Elements 

        public async Task<(TradeElement newEntry, TradeElement summary)> AddInterimPositionAsync(string tradeId, bool isAdd)
        {
            var trade = await GetTradeCompositeAsync(tradeId);
            TradeElement tradeInput = new TradeElement(trade, isAdd? TradeActionType.AddPosition : TradeActionType.ReducePosition);

            trade.TradeElements.Add(tradeInput);
            UpdateSummary(ref trade);

            return (tradeInput, trade.Summary);
        }

        public async Task<TradeElement> RemoveInterimPositionAsync(string tradeId, string tradeInputId)
        {
            var trade = await GetTradeCompositeAsync(tradeId);
            if (trade.TradeElements.Count <= 1)
            {
                throw new InvalidOperationException($"No entries to remove on trade ID {tradeId} .");
            }
            JournalRepoHelpers.RemoveInterimInput(ref trade, tradeInputId);
            UpdateSummary(ref trade);

            return trade.Summary;
        }

        //Entries Update

        public async Task<(Cell updatedCell, TradeElement? summary)> UpdateCellContent(string componentId, string newContent, string changeNote)
        {
            if (!componentId.IsValidInteger())
            {
                throw new ArgumentException($"The EntryId '{componentId}' is not a valid integer.", nameof(componentId));
            }

            var cell = await dataContext.AllEntries.Where(t => t.Id.ToString() == componentId).SingleOrDefaultAsync();
            if (cell == null)
            {
                throw new InvalidOperationException($"Entry with ID {componentId} not found.");
            }

            cell.SetFollowupContent(newContent, changeNote);

            TradeElement? summary = null;
            if (cell.IsRelevantForOverview)
            {
                var trade = cell.TradeElementRef.TradeCompositeRef;
                summary = UpdateSummary(ref trade);
            }

            await dataContext.SaveChangesAsync();

            return (cell, summary);
        }

        //Closure

        public async Task<TradeElement> CloseAsync(string tradeId, string closingPrice)
        {
            var trade = await GetTradeCompositeAsync(tradeId);
            var analytics = JournalRepoHelpers.GetAvgEntryAndProfit(trade);

            // Create and add reduction entry for closing
            var tradeInput = JournalRepoHelpers.CreateTradeElementForClosure(trade, closingPrice, analytics);
            trade.TradeElements.Add(tradeInput);

            UpdateSummary(ref trade);

            return trade.Summary!;
        }

        private TradeElement UpdateSummary(ref TradeComposite trade)
        {
            TradeElement summary = JournalRepoHelpers.GetInterimSummary(trade);
            trade.Summary = summary;

            return summary;
        }

        private async Task<TradeComposite> GetTradeCompositeAsync(string tradeId)
        {
            if (!tradeId.IsValidInteger())
            {
                throw new ArgumentException($"The tradeId '{tradeId}' is not a valid integer.", nameof(tradeId));
            }

            var trade = await dataContext.AllTradeComposites.AsNoTracking().Where(t => t.Id.ToString() == tradeId).SingleOrDefaultAsync();

            if (trade == null)
            {
                throw new InvalidOperationException($"Trade with ID {tradeId} not found.");
            }

            return trade!;
        }

    }
}
