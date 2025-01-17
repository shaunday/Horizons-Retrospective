﻿using HsR.Common;
using HsR.Journal.Entities;

namespace HsR.Journal.DataContext
{
    public interface IJournalRepository
    {
        //composites
        Task<(IEnumerable<TradeComposite>, PaginationMetadata)> GetAllTradeCompositesAsync(int pageNumber = 1, int pageSize = 10);
        Task<TradeComposite> AddTradeCompositeAsync();
        Task<(IEnumerable<TradeComposite>, PaginationMetadata)> GetFilteredTradesAsync(TradesFilterModel filter, int pageNumber = 1, int pageSize = 10);

        //interim elements
        Task<(TradeElement newEntry, TradeElement? summary)> AddInterimPositionAsync(string tradeId, bool isAdd);
        Task<TradeElement?> RemoveInterimPositionAsync(string tradeId, string tradeInputId);

        //components
        Task<(DataElement updatedCell, TradeElement? summary)> UpdateCellContentAsync(string componentId, string newContent, string changeNote);

        //closure
        Task<TradeElement> CloseTradeAsync(string tradeId, string closingPrice);

        //data
        Task<List<string>?> GetAllSavedSectors();
    }
}
