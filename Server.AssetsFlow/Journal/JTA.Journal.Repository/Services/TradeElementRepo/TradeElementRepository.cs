using HsR.Journal.Entities;
using HsR.Journal.Entities.Factory;
using Microsoft.EntityFrameworkCore;

namespace HsR.Journal.DataContext
{
    public class TradeElementRepository(TradingJournalDataContext dataContext) : JournalRepositoryBase(dataContext), ITradeElementRepository
    {
        public async Task<(TradeElement newEntry, TradeElement summary)> AddInterimPositionAsync(string tradeId, bool isAdd)
        {
            var trade = await GetTradeCompositeAsync(tradeId);
            TradeElement tradeInput = TradeElementCRUDs.CreateInterimTradeElement(trade, isAdd);
            trade.TradeElements.Add(tradeInput);

            TradeElement summary = TradeCompositeUpdates.RecreateSummary(trade);

            await _dataContext.SaveChangesAsync();
            return (tradeInput, summary);
        }

        public async Task<TradeElement> RemoveInterimPositionAsync(string tradeId, string tradeInputId)
        {
            var trade = await GetTradeCompositeAsync(tradeId);
            if (trade.TradeElements.Count <= 1)
            {
                throw new InvalidOperationException($"No entries to remove on trade ID {tradeId} .");
            }

            TradeElementCRUDs.RemoveInterimInputById(trade, tradeInputId);
            TradeElement summary = TradeElementCRUDs.GetInterimSummary(trade);
            trade.Summary = summary;

            await _dataContext.SaveChangesAsync();
            return summary;
        }

        public async Task<TradeElement> CloseTradeAsync(string tradeId, string closingPrice)
        {
            var trade = await GetTradeCompositeAsync(tradeId);
            TradeCompositeUpdates.CloseTrade(trade, closingPrice);

            await _dataContext.SaveChangesAsync();
            return trade.Summary!;
        }

        private async Task<TradeComposite> GetTradeCompositeAsync(string tradeId)
        {
            if (!int.TryParse(tradeId, out var parsedId))
            {
                throw new ArgumentException($"The tradeId '{tradeId}' is not a valid integer.", nameof(tradeId));
            }

            var trade = await _dataContext.TradeComposites
                                            .Where(t => t.Id == parsedId)
                                            .SingleOrDefaultAsync() ?? throw new InvalidOperationException($"Trade with ID {tradeId} not found.");
            return trade!;
        }
    }

}