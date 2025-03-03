using HsR.Journal.Entities;
using HsR.Journal.Entities.Factory;
using HsR.Journal.Entities.TradeJournal;
using HsR.Journal.Repository.Services.Base;
using Microsoft.EntityFrameworkCore;

namespace HsR.Journal.DataContext
{
    public class TradeElementRepository(TradingJournalDataContext dataContext) 
                                            : JournalRepositoryBase(dataContext), ITradeElementRepository
    {
        public async Task<(InterimTradeElement newEntry, TradeSummary? summary)> AddInterimPositionAsync(string tradeId, bool isAdd)
        {
            var trade = await GetTradeCompositeAsync(tradeId);

            if (TradeElementsFactory.GetNewElement(trade, isAdd ? TradeActionType.Add : TradeActionType.Reduce)
                is not InterimTradeElement tradeInput)
            {
                throw new InvalidOperationException("TradeElementsFactory returned an invalid type for InterimTradeElement.");
            }

            trade.TradeElements.Add(tradeInput);

            TradeSummary? newSummary = null;
            if (trade.Status == TradeStatus.Open)
            {
                newSummary = RefreshSummary(trade);
            }

            await _dataContext.SaveChangesAsync();
            return (tradeInput, newSummary);
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


        public async Task<TradeSummary?> RemoveInterimPositionAsync(string tradeInputId)
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

            TradeSummary? summary = null;
            if (trade.IsTradeActive()) 
            {
                summary = RefreshSummary(trade);
            }
            else
            {
                trade.Status = TradeStatus.AnIdea;
            }

            await _dataContext.SaveChangesAsync();
            return summary;
        }


        public async Task<InterimTradeElement> ActivateTradeElement(string tradeEleId)
        {
            var tradeEle = await GetTradeElementAsync(tradeEleId, loadComposite: true);

            bool okForActivate = tradeEle.AllowActivation();
            if (okForActivate)
            {
                tradeEle.Activate();
                tradeEle.CompositeRef.Activate();
                await _dataContext.SaveChangesAsync();
            }

            return tradeEle;
        }

    }

}