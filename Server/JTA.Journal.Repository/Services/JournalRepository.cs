﻿using JTA.Journal.Entities;
using JTA.Journal.DataContext.Services;
using Microsoft.EntityFrameworkCore;

namespace JTA.Journal.Repository.Services
{
    public partial class JournalRepository(TradingJournalDataContext journalDbContext) : IJournalRepository
    {
        private readonly TradingJournalDataContext dataContext = journalDbContext ?? throw new ArgumentNullException(nameof(journalDbContext));

        //Trade Composite 

        public async Task<(IEnumerable<TradeComposite>, PaginationMetadata)> GetAllTradeCompositesAsync(int pageNumber = 1, int pageSize = 10)
        {
            var trades = await dataContext.TradeComposites
                                            .AsNoTracking()
                                            .OrderBy(t => t.Id)  // Order by the actual ID to ensure correct sequential order
                                            .Skip((pageNumber - 1) * pageSize)
                                            .Take(pageSize)
                                            .ToListAsync();

            //todo monitor performance .AsSplitQuery() ? or other solutions

            // Ensure there are trades
            if (trades == null || trades.Count == 0)
            {
                throw new InvalidOperationException("Could not get any trades.");
            }

            // Get total count of records
            var totalCount = await dataContext.TradeComposites.CountAsync();

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
            TradeComposite trade = new();

            TradeElement originElement = new(trade, TradeActionType.Origin);
            trade.TradeElements.Add(originElement);

            dataContext.TradeComposites.Add(trade);
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
            TradeElement tradeInput = new(trade, isAdd ? TradeActionType.AddPosition : TradeActionType.ReducePosition);

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

        public async Task<(DataElement updatedCell, TradeElement? summary)> UpdateCellContentAsync(string componentId, string newContent, string changeNote)
        {
            if (!int.TryParse(componentId, out var parsedId))
            {
                throw new ArgumentException($"The EntryId '{componentId}' is not a valid integer.", nameof(componentId));
            }

            var cell = await dataContext.Entries
                                .Include(t => t.History)
                                .SingleOrDefaultAsync(t => t.Id == parsedId) ?? throw new InvalidOperationException($"Entry with ID {componentId} not found.");

            cell.SetFollowupContent(newContent, changeNote);

            TradeElement? newSummary = null;
            if (cell.IsRelevantForOverview)
            {
                await dataContext.Entry(cell)
                    .Reference(c => c.CompositeRef)
                    .LoadAsync();

                var trade = cell.CompositeRef;

                newSummary = RecalculateSummary(trade);
            }

            await dataContext.SaveChangesAsync();

            return (cell, newSummary);
        }


        #region Helpers
        private static TradeElement RecalculateSummary(TradeComposite trade)
        {
            TradeElement summary = TradeElementCRUDs.GetInterimSummary(trade);
            trade.Summary = summary;
            return summary;
        }

        private async Task<TradeComposite> GetTradeCompositeAsync(string tradeId)
        {
            if (!int.TryParse(tradeId, out var parsedId))
            {
                throw new ArgumentException($"The tradeId '{tradeId}' is not a valid integer.", nameof(tradeId));
            }

            var trade = await dataContext.TradeComposites
                                            .Where(t => t.Id == parsedId)
                                            .SingleOrDefaultAsync() ?? throw new InvalidOperationException($"Trade with ID {tradeId} not found.");
            return trade!;
        }
        #endregion
    }
}
