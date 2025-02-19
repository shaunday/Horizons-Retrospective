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
            InterimTradeElement tradeInput = TradeElementsFactory.GetNewInterimTradeElement(trade, isAdd);
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
            InterimTradeElement tradeOverview = TradeElementsFactory.GetNewEvalutationElement(trade);

            if (trade.Summary != null)
            {
                tradeOverview.Entries.AddRange(trade.Summary.Entries);
            }

            return tradeOverview;
        }

        public async Task<TradeSummary?> RemoveInterimPositionAsync(string tradeId, string tradeInputId)
        {
            if (!int.TryParse(tradeInputId, out var parsedId))
            {
                throw new ArgumentException($"The element Id '{tradeInputId}' is not a valid integer.", nameof(tradeInputId));
            }

            var trade = await GetTradeCompositeAsync(tradeId);

            var tradeInputToRemove = trade.TradeElements.Where(t => t.Id == parsedId).SingleOrDefault();
            if (tradeInputToRemove != null)
            {
                trade.TradeElements.Remove(tradeInputToRemove); //trade is being tracked so change will be commited on save
            }
            else
            {
                throw new ArgumentException($"The trade input (Id '{tradeInputId}') to remove is null.", nameof(tradeInputId));

            }

            //check if any positions are still active
            TradeSummary? summary = null;
            if (trade.IsTadeActive())
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