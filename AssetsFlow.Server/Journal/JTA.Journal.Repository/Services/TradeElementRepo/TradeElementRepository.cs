using AutoMapper;
using HsR.Journal.Entities;
using HsR.Journal.Entities.Factory;
using HsR.Journal.Entities.TradeJournal;
using HsR.Journal.Repository.Services.Base;
using HsR.Journal.Services;
using Microsoft.EntityFrameworkCore;

namespace HsR.Journal.DataContext
{
    public class TradeElementRepository(TradingJournalDataContext dataContext) 
                                            : JournalRepositoryBase(dataContext), ITradeElementRepository
    {
        private static readonly IMapper mapper = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<DataElement, DataElement>()
                .ForMember(dest => dest.Id, opt => opt.Ignore()) // Ignore ID to prevent conflicts
                .ForMember(dest => dest.TradeElementRef, opt => opt.Ignore()) // Avoid cloning old reference
                .ForMember(dest => dest.CompositeRef, opt => opt.Ignore());
        }).CreateMapper();

        public async Task<(InterimTradeElement newEntry, UpdatedStatesCollation? updatedStates)> AddInterimPositionAsync(string tradeId, bool isAdd)
        {
            var trade = await GetTradeCompositeAsync(tradeId);

            if (TradeElementsFactory.GetNewElement(trade, isAdd ? TradeActionType.Add : TradeActionType.Reduce)
                is not InterimTradeElement tradeInput)
            {
                throw new InvalidOperationException("TradeElementsFactory returned an invalid type for InterimTradeElement.");
            }

            trade.TradeElements.Add(tradeInput);

            UpdatedStatesCollation? newStates = null;
            if (trade.Status == TradeStatus.Open)
            {
                newStates = new()
                {
                    Summary = RefreshSummary(trade)
                };
            }

            await _dataContext.SaveChangesAsync();
            return (tradeInput, newStates);
        }

        public async Task<InterimTradeElement> AddInterimEvalutationAsync(string tradeId)
        {
            var trade = await GetTradeCompositeAsync(tradeId);

            if (TradeElementsFactory.GetNewElement(trade, TradeActionType.Evaluation)
                is not InterimTradeElement tradeOverview)
            {
                throw new InvalidOperationException("TradeElementsFactory returned an invalid type for Evaluation.");
            }

            if (trade.Summary != null)
            {
                var clonedEntries = trade.Summary.Entries
                    .Select(entry =>
                    {
                        var clonedEntry = mapper.Map<DataElement>(entry);
                        clonedEntry.UpdateParentRefs(tradeOverview);
                        return clonedEntry;
                    })
                    .ToList();

                tradeOverview.Entries.AddRange(clonedEntries);
            }

            return tradeOverview;
        }

        public async Task<UpdatedStatesCollation> RemoveInterimPositionAsync(string tradeInputId)
        {
            if (!int.TryParse(tradeInputId, out var parsedId))
            {
                throw new ArgumentException($"The element Id '{tradeInputId}' is not a valid integer.", nameof(tradeInputId));
            }

            var tradeInputToRemove = await _dataContext.TradeElements.FindAsync(parsedId);
            if (tradeInputToRemove == null)
            {
                throw new ArgumentException($"The trade input (Id '{tradeInputId}') to remove is null.", nameof(tradeInputId));
            }

            var trade = await GetTradeCompositeAsync(tradeInputToRemove.CompositeFK);
            if (trade == null)
            {
                throw new InvalidOperationException($"TradeComposite with Id '{tradeInputToRemove.CompositeFK}' not found.");
            }

            _dataContext.TradeElements.Remove(tradeInputToRemove);

            UpdatedStatesCollation? updatedStates = new();
            if (trade.IsTradeActive()) 
            {
                updatedStates.Summary = RefreshSummary(trade);
            }
            else
            {
                trade.Status = TradeStatus.AnIdea;
                updatedStates.TradeStatus = trade.Status;
            }

            await _dataContext.SaveChangesAsync();
            return updatedStates;
        }
    }

}