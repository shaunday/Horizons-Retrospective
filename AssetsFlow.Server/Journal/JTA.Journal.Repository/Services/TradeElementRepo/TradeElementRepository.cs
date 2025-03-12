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
                tradeOverview.Entries.AddRange(trade.Summary.Entries);
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

        public async Task<UpdatedStatesCollation> CloseTradeAsync(string tradeId, string closingPrice)
        {
            var trade = await GetTradeCompositeAsync(tradeId);
            if (trade.Summary != null)
            {
                _dataContext.Entry(trade.Summary).State = EntityState.Deleted;
            }
            else
            {
                throw new InvalidOperationException("Trade summary is missing.");
            }
            TradeCompositeOperations.CloseTrade(trade, closingPrice);

            await _dataContext.SaveChangesAsync();

            UpdatedStatesCollation? newStates = new() { Summary = trade.Summary };
            return newStates;
        }
    }

}