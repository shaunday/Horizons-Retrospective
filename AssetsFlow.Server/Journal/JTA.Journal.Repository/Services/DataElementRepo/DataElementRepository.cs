using HsR.Journal.Entities;
using HsR.Journal.Entities.Factory;
using HsR.Journal.Entities.TradeJournal;
using HsR.Journal.Repository.Services.Base;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace HsR.Journal.DataContext
{
    public class DataElementRepository(TradingJournalDataContext dataContext) : JournalRepositoryBase(dataContext), IDataElementRepository
    {
        public async Task<(DataElement updatedCell, TradeSummary? summary, DateTime? newTimeStamp)>
                                                UpdateCellContentAsync(string componentId, string newContent, string changeNote)
        {
            if (!int.TryParse(componentId, out var parsedId))
            {
                throw new ArgumentException($"The EntryId '{componentId}' is not a valid integer.", nameof(componentId));
            }

            var cell = await _dataContext.Entries
                .Include(e => e.History)
                .FirstOrDefaultAsync(e => e.Id == parsedId)
                ?? throw new InvalidOperationException($"Entry with ID {componentId} not found.");

            cell.SetFollowupContent(newContent, changeNote);

            TradeSummary? newSummary = null;
            DateTime? newTimeStamp = null;
            bool activationReq = cell.IsMustHave;

            if (cell.IsCostRelevant())
            {
                await LoadCompositeRefAsync(cell);
                newSummary = RefreshSummary(cell.CompositeRef);
            }

            if (activationReq)
            {
                newTimeStamp = await HandleActivationAsync(cell);
            }

            await _dataContext.SaveChangesAsync();
            return (cell, newSummary, newTimeStamp);
        }

        private async Task LoadCompositeRefAsync(DataElement cell)
        {
            if (cell.CompositeRef == null)
            {
                await _dataContext.Entry(cell).Reference(c => c.CompositeRef).LoadAsync();
            }
        }

        private async Task<DateTime?> HandleActivationAsync(DataElement cell)
        {
            await _dataContext.Entry(cell).Reference(c => c.TradeElementRef).LoadAsync();

            if (cell.TradeElementRef is InterimTradeElement interim && interim.AllowActivation())
            {
                interim.Activate();
                await LoadCompositeRefAsync(cell);
                cell.CompositeRef?.Activate();
                return interim.TimeStamp;
            }

            Log.Logger.Warning($"Activation failed or incorrect cast for Entry ID {cell.Id}.");
            return null;
        }
    }
}
