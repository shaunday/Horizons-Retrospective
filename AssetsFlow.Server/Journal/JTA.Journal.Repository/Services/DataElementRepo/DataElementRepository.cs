using HsR.Journal.Entities;
using HsR.Journal.Entities.Factory;
using HsR.Journal.Repository.Services.Base;
using Microsoft.EntityFrameworkCore;

namespace HsR.Journal.DataContext
{
    public class DataElementRepository(TradingJournalDataContext dataContext) : JournalRepositoryBase(dataContext), IDataElementRepository
    {
        public async Task<(DataElement updatedCell, TradeElement? summary)> UpdateCellContentAsync(string componentId, string newContent, string changeNote)
        {
            if (!int.TryParse(componentId, out var parsedId))
            {
                throw new ArgumentException($"The EntryId '{componentId}' is not a valid integer.", nameof(componentId));
            }

            var cell = await _dataContext.Entries
                                .Include(t => t.History)
                                .SingleOrDefaultAsync(t => t.Id == parsedId) ?? throw new InvalidOperationException($"Entry with ID {componentId} not found.");

            cell.SetFollowupContent(newContent, changeNote);

            TradeElement? newSummary = null;
            if (cell.IsCostRelevant)
            {
                await _dataContext.Entry(cell)
                    .Reference(c => c.CompositeRef)
                    .LoadAsync();

                var trade = cell.CompositeRef;
                newSummary = RefreshSummary(trade);
            }

            await _dataContext.SaveChangesAsync();
            return (cell, newSummary);
        }
    }

}