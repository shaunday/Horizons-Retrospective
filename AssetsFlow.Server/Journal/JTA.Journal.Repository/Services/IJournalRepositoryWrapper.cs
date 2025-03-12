using HsR.Common;
using HsR.Journal.Entities;
using HsR.Journal.Entities.TradeJournal;
using HsR.Journal.Services;
using System.Data;

namespace HsR.Journal.DataContext
{
    public interface IJournalRepositoryWrapper
    {
        //composites
        Task<(IEnumerable<TradeComposite>, PaginationMetadata)> GetAllTradeCompositesAsync(int pageNumber = 1, int pageSize = 10);
        Task<(IEnumerable<TradeComposite>, PaginationMetadata)> GetFilteredTradesAsync(TradesFilterModel filter, int pageNumber = 1, int pageSize = 10);
        Task<TradeComposite> AddTradeCompositeAsync();

        //interim elements
        Task<(InterimTradeElement newEntry, UpdatedStatesCollation? updatedStates)> AddInterimPositionAsync(string tradeId, bool isAdd);
        Task<UpdatedStatesCollation> RemoveInterimPositionAsync(string tradeInputId);
        Task<InterimTradeElement> AddInterimEvalutationAsync(string tradeId);

        //components
        Task<(DataElement updatedCell, UpdatedStatesCollation updatedStates)> UpdateCellContentAsync(string componentId, string newContent, string changeNote);

        //closure
        Task<UpdatedStatesCollation> CloseTradeAsync(string tradeId, string closingPrice);

        //data
        Task<List<string>?> GetAllSavedSectors();
    }
}
