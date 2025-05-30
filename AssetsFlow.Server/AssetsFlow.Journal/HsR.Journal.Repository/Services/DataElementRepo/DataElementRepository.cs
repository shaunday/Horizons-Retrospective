using HsR.Journal.Entities;
using HsR.Journal.Entities.Factory;
using HsR.Journal.Entities.TradeJournal;
using HsR.Journal.Repository.Services.Base;
using HsR.Journal.Services;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace HsR.Journal.DataContext
{
    public class DataElementRepository(TradingJournalDataContext dataContext) : JournalRepositoryBase(dataContext), IDataElementRepository
    {
        public async Task<(DataElement updatedCell, UpdatedStatesCollation updatedStates)>
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

            UpdatedStatesCollation updatedStates = new();

            var interimIfActive = await HandleActivationAsync(cell);
            if (interimIfActive != null)
            {
                updatedStates.ActivationTimeStamp = interimIfActive.TimeStamp;
                updatedStates.TradeInfo = cell.CompositeRef;

                if (cell.IsCostRelevant())
                {
                    RefreshSummary(cell.CompositeRef);
                }
            }

            if (cell.SectorRelevance)
            {
                var userData = await _dataContext.UserData.FirstOrDefaultAsync(u => u.Id == 1);
                userData?.SavedSectors?.Add(newContent);
                updatedStates.SavedSectors = userData?.SavedSectors;
            }

            await _dataContext.SaveChangesAsync();
            return (cell, updatedStates);
        }

        private async Task LoadCompositeRefAsync(DataElement cell)
        {
            if (cell.CompositeRef == null)
            {
                await _dataContext.Entry(cell).Reference(c => c.CompositeRef).LoadAsync();
            }
        }

        private async Task<InterimTradeElement?> HandleActivationAsync(DataElement cell)
        {
            await _dataContext.Entry(cell).Reference(c => c.TradeElementRef).LoadAsync();

            if (cell.TradeElementRef is InterimTradeElement interim && !interim.IsActive && interim.AllowActivation())
            {
                interim.Activate();

                if (interim.TradeActionType == TradeActionType.Add)
                {
                    await LoadCompositeRefAsync(cell);
                    cell.CompositeRef?.Activate();
                }
                return interim;
            }

            Log.Logger.Warning($"Activation failed or incorrect cast for Entry ID {cell.Id}.");
            return null;
        }
    }
}
