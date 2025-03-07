﻿using HsR.Common;
using HsR.Journal.Entities;
using HsR.Journal.Entities.TradeJournal;

namespace HsR.Journal.DataContext
{
    public interface IJournalRepositoryWrapper
    {
        //composites
        Task<(IEnumerable<TradeComposite>, PaginationMetadata)> GetAllTradeCompositesAsync(int pageNumber = 1, int pageSize = 10);
        Task<(IEnumerable<TradeComposite>, PaginationMetadata)> GetFilteredTradesAsync(TradesFilterModel filter, int pageNumber = 1, int pageSize = 10);
        Task<TradeComposite> AddTradeCompositeAsync();

        //interim elements
        Task<(InterimTradeElement newEntry, TradeSummary? summary)> AddInterimPositionAsync(string tradeId, bool isAdd);
        Task<InterimTradeElement> AddInterimEvalutationAsync(string tradeId);
        Task<TradeSummary?> RemoveInterimPositionAsync(string tradeInputId);
        Task<InterimTradeElement> ActivateTradeElement(string tradeEleId);

        //components
        Task<(DataElement updatedCell, TradeSummary? summary, DateTime? newTimeStamp)> UpdateCellContentAsync(string componentId, string newContent, string changeNote);

        //closure
        Task<TradeElement> CloseTradeAsync(string tradeId, string closingPrice);

        //data
        Task<List<string>?> GetAllSavedSectors();
    }
}
