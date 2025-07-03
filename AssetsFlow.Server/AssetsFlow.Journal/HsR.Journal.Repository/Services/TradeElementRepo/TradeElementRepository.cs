using AutoMapper;
using HsR.Journal.Entities;
using HsR.Journal.Entities.Factory;
using HsR.Journal.Entities.TradeJournal;
using HsR.Journal.Repository.Services.Base;
using HsR.Journal.Services;
using Microsoft.EntityFrameworkCore;

namespace HsR.Journal.DataContext
{
    public class TradeElementRepository(TradingJournalDataContext dataContext) : JournalRepositoryBase(dataContext), ITradeElementRepository
    {
        private static readonly IMapper mapper = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<DataElement, DataElement>()
                .ForMember(dest => dest.Id, opt => opt.Ignore()) // Ignore ID to prevent conflicts
                .ForMember(dest => dest.TradeElementRef, opt => opt.Ignore()) // Avoid cloning old reference
                .ForMember(dest => dest.CompositeRef, opt => opt.Ignore());
        }).CreateMapper();

        public async Task<InterimTradeElement> AddInterimPositionAsync(int tradeId, bool isAdd)
        {
            var trade = await GetTradeCompositeAsync(tradeId);
            
            if (TradeElementsFactory.GetNewElement(trade, isAdd ? TradeActionType.Add : TradeActionType.Reduce) is not InterimTradeElement tradeInput)
            {
                throw new InvalidOperationException("TradeElementsFactory returned an invalid type for InterimTradeElement.");
            }

            trade.TradeElements.Add(tradeInput);
            await _dataContext.SaveChangesAsync();
            return tradeInput;
        }

        public async Task<InterimTradeElement> AddInterimEvalutationAsync(int tradeId)
        {
            var trade = await GetTradeCompositeAsync(tradeId);

            if (TradeElementsFactory.GetNewElement(trade, TradeActionType.Evaluation) is not InterimTradeElement tradeOverview)
            {
                throw new InvalidOperationException("TradeElementsFactory returned an invalid type for Evaluation.");
            }

            if (trade.Summary != null)
            {
                var clonedEntries = trade.Summary.Entries
                    .Select(entry =>
                    {
                        var clonedEntry = mapper.Map<DataElement>(entry);

                        clonedEntry.Id = 0;
                        clonedEntry.History = null;
                        clonedEntry.ContentWrapper = null;
                        clonedEntry.IsRelevantForTradeOverview = false;
                        clonedEntry.UserId = trade.UserId;

                        string content = entry.Content ?? "";
                        clonedEntry.SetFollowupContent(content, "");

                        clonedEntry.UpdateParentRefs(tradeOverview);
                        return clonedEntry;
                    })
                    .ToList();

                tradeOverview.Entries.AddRange(clonedEntries);
            }
            await _dataContext.SaveChangesAsync();

            return tradeOverview;
        }

        public async Task<TradeComposite> RemoveInterimPositionAsync(int tradeInputId)
        {
            var tradeInputToRemove = await GetTradeElementAsync(tradeInputId);
            if (tradeInputToRemove == null)
            {
                throw new ArgumentException($"The trade input (Id '{tradeInputId}') to remove is null.", nameof(tradeInputId));
            }

            var trade = await GetTradeCompositeAsync(tradeInputToRemove.CompositeFK);
            if (trade == null)
            {
                throw new InvalidOperationException($"TradeComposite with Id '{tradeInputToRemove.CompositeFK}' not found.");
            }

            trade.TradeElements.Remove(tradeInputToRemove);

            if (trade.IsTradeActive()) 
            {
                RefreshSummary(trade);
            }
            else
            {
                if (trade.Summary != null)
                {
                    _dataContext.Entry(trade.Summary).State = EntityState.Deleted;
                    trade.Summary = null;
                }
                trade.Status = TradeStatus.AnIdea;
            }
            await _dataContext.SaveChangesAsync();
            return trade;
        }

        public async Task<UpdatedStatesCollation> UpdateActivationTimeAsync(int tradeInputId, string newTimestamp)
        {
            var tradeInput = await GetTradeElementAsync(tradeInputId);
            if (tradeInput == null)
            {
                throw new ArgumentException($"The trade input (Id '{tradeInputId}') to update is null.", nameof(tradeInputId));
            }
            
            tradeInput.Activate();
            UpdatedStatesCollation? updatedStates = new() { ActivationTimeStamp = tradeInput.TimeStamp };

            await _dataContext.SaveChangesAsync();
            return updatedStates;
        }
    }
}