using HsR.Journal.Entities;
using HsR.Journal.Entities.Factory;
using HsR.Journal.Repository.Services.Base;
using Microsoft.EntityFrameworkCore;

namespace HsR.Journal.DataContext
{
    public class TradeElementRepository(TradingJournalDataContext dataContext) 
                                            : JournalRepositoryBase(dataContext), ITradeElementRepository
    {
        public async Task<(TradeElement newEntry, TradeElement summary)> AddInterimPositionAsync(string tradeId, bool isAdd)
        {
            var trade = await GetTradeCompositeAsync(tradeId);
            TradeElement tradeInput = TradeElementCRUDs.CreateInterimTradeElement(trade, isAdd);
            trade.TradeElements.Add(tradeInput);

            if (trade.Status == TradeStatus.AnIdea)
            {
                trade.Status = TradeStatus.Open;
            }

            TradeElement newSummary = RefreshSummary(trade);

            await _dataContext.SaveChangesAsync();
            return (tradeInput, newSummary);
        }

        public async Task<TradeElement> RemoveInterimPositionAsync(string tradeId, string tradeInputId)
        {
            var trade = await GetTradeCompositeAsync(tradeId);
            if (trade.TradeElements.Count <= 1)
            {
                throw new InvalidOperationException($"No entries to remove on trade ID {tradeId} .");
            }

            TradeElementCRUDs.RemoveInterimInputById(trade, tradeInputId);
            TradeElement newSummary = RefreshSummary(trade); 

            await _dataContext.SaveChangesAsync();
            return newSummary;
        }

        public async Task<TradeElement> ActivateTradeElement(string tradeEleId)
        {
            var tradeEle = await GetTradeElementAsync(tradeEleId);

            bool okForActivate = !tradeEle.AllowActivation();
            
            if (okForActivate)
            {
                tradeEle.Activate();
                await _dataContext.SaveChangesAsync();
            }

            return tradeEle;
        }

    }

}