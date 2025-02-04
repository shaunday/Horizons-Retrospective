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
            TradeElement tradeInput = TradeElementsFactory.GetNewInterimTradeElement(trade, isAdd);
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