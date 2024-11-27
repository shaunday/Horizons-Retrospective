using DayJT.Journal.Data;
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

        //Gen. Data

        public async Task<List<string>?> GetAllSavedSectors()
        {
            var journalData = await dataContext.JournalData.AsNoTracking().SingleOrDefaultAsync();
            return journalData?.SavedSectors?.OrderBy(s => s).ToList();
        }

        //Trade Composite 

        public async Task<(IEnumerable<TradeComposite>, PaginationMetadata)> GetAllTradeCompositesAsync(int pageNumber = 1, int pageSize = 10)
        {
            var trades = await dataContext.AllTradeComposites
                                            .AsNoTracking()
                                            .OrderBy(t => t.Id)  // Order by the actual ID to ensure correct sequential order
                                            .Skip((pageNumber - 1) * pageSize)
                                            .Take(pageSize)
                                            .Include(t => t.TradeElements)
                                                .ThenInclude(te => te.Entries)
                                                    .ThenInclude(e => e.History)
                                            .ToListAsync();

            //todo monitor performance .AsSplitQuery() ? or other solutions

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

        public async Task<TradeElement> CloseTradeAsync(string tradeId, string closingPrice)
        {
            var trade = await GetTradeCompositeAsync(tradeId);

            // Create and add reduction entry for closing
            var tradeInput = TradeElementCRUDs.CreateTradeElementForClosure(trade, closingPrice);
            trade.TradeElements.Add(tradeInput);

            RecalculateSummary(trade);

            return trade.Summary!;
        }

        //Trade Elements 

        public async Task<(TradeElement newEntry, TradeElement summary)> AddInterimPositionAsync(string tradeId, bool isAdd)
        {
            var trade = await GetTradeCompositeAsync(tradeId);
            TradeElement tradeInput = new TradeElement(trade, isAdd ? TradeActionType.AddPosition : TradeActionType.ReducePosition);

            trade.TradeElements.Add(tradeInput);
            RecalculateSummary(trade);
            await dataContext.SaveChangesAsync();

            return (tradeInput, trade.Summary);
        }

        public async Task<TradeElement> RemoveInterimPositionAsync(string tradeId, string tradeInputId)
        {
            var trade = await GetTradeCompositeAsync(tradeId);
            if (trade.TradeElements.Count <= 1)
            {
                throw new InvalidOperationException($"No entries to remove on trade ID {tradeId} .");
            }
            TradeElementCRUDs.RemoveInterimInput(trade, tradeInputId);
            RecalculateSummary(trade);
            await dataContext.SaveChangesAsync();

            return trade.Summary;
        }

        //Entries Update

        public async Task<(Cell updatedCell, TradeElement? summary)> UpdateCellContent(string componentId, string newContent, string changeNote)
        {
            if (!int.TryParse(componentId, out var parsedId))
            {
                throw new ArgumentException($"The EntryId '{componentId}' is not a valid integer.", nameof(componentId));
            }

            var cell = await dataContext.AllEntries.SingleOrDefaultAsync(t => t.Id == parsedId);
            if (cell == null)
            {
                throw new InvalidOperationException($"Entry with ID {componentId} not found.");
            }

            cell.SetFollowupContent(newContent, changeNote);

            TradeElement? newSummary = null;
            if (cell.IsRelevantForOverview)
            {
                await dataContext.Entry(cell)
                    .Reference(c => c.TradeCompositeRef)
                    .LoadAsync();

                var trade = cell.TradeCompositeRef;

                newSummary = RecalculateSummary(trade);
            }

            await dataContext.SaveChangesAsync();

            return (cell, newSummary);
        }     


        #region Helpers
        private TradeElement RecalculateSummary(TradeComposite trade)
        {
            TradeElement summary = TradeElementCRUDs.GetInterimSummary(trade);
            trade.Summary = summary;
            return summary;
        }

        private async Task<TradeComposite> GetTradeCompositeAsync(string tradeId, bool loadEntriesHistory = false)
        {
            if (!int.TryParse(tradeId, out var parsedId))
            {
                throw new ArgumentException($"The tradeId '{tradeId}' is not a valid integer.", nameof(tradeId));
            }

            return await GetTradeCompositeAsync(parsedId, loadEntriesHistory);
        }

        private async Task<TradeComposite> GetTradeCompositeAsync(int tradeId, bool loadEntriesHistory = false)
        {
            var trade = await dataContext.AllTradeComposites
                                            .Where(t => t.Id == tradeId)
                                            .Include(t => t.TradeElements)
                                                .ThenInclude(te => te.Entries)
                                            .SingleOrDefaultAsync();

            if (trade == null)
            {
                throw new InvalidOperationException($"Trade with ID {tradeId} not found.");
            }

            return trade!;
        } 
        #endregion
    }
}
