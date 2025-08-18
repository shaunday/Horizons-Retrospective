using HsR.Journal.Entities;
using HsR.Journal.Entities.Factory;
using HsR.Journal.Entities.TradeJournal;
using HsR.Journal.Repository.Services.Base;
using HsR.Journal.Services;
using Microsoft.EntityFrameworkCore;

namespace HsR.Journal.DataContext
{
    public class DataElementRepository(TradingJournalDataContext dataContext, IUserDataRepository userDataRepository) 
                                                                    : JournalRepositoryBase(dataContext), IDataElementRepository
    {
        private readonly IUserDataRepository _userDataRepository = userDataRepository;

        public async Task<(DataElement updatedCell, UpdatedStatesCollation updatedStates)> 
                                                        UpdateCellContentAsync(int componentId, UpdateDataComponentRequest request)
        {
            var cell = await _dataContext.Entries
                .Include(e => e.History)
                .FirstOrDefaultAsync(e => e.Id == componentId)
                ?? throw new InvalidOperationException($"Entry with ID {componentId} not found.");

            cell.SetFollowupContent(request.Content, request.Info);

            UpdatedStatesCollation updatedStates =  await HandleTradeStatusUpdates(cell, request.DenyActivation);

            if (cell.SectorRelevance)
            {
                var userId = cell.UserId;
                await _userDataRepository.SaveSectorAsync(userId, request.Content);
                var userData = await _userDataRepository.GetOrCreateUserDataAsync(userId);
                updatedStates.SavedSectors = userData.SavedSectors;
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

        private async Task<UpdatedStatesCollation> HandleTradeStatusUpdates(DataElement cell, bool ignoreActivation)
        {
            UpdatedStatesCollation updatedStates = new();
            await _dataContext.Entry(cell).Reference(c => c.TradeElementRef).LoadAsync();

            if (cell.TradeElementRef is InterimTradeElement interim && interim.IsAllRequiredFields()
                                                                            && interim.TradeActionType != TradeActionType.Origin)
            {
                if (!ignoreActivation)
                {
                    interim.Activate();
                    updatedStates.ActivationTimeStamp = interim.TimeStamp;
                }

                await LoadCompositeRefAsync(cell);

                if (interim.TradeActionType == TradeActionType.Add)
                {
                    cell.CompositeRef?.SetStatus(TradeStatus.Open);
                }

                if (cell.CompositeRef != null && cell.IsCostRelevant())
                {
                    RefreshSummary(cell.CompositeRef);
                }

                updatedStates.TradeInfo = cell.CompositeRef;
            }

            return updatedStates;
        }
    }
}
