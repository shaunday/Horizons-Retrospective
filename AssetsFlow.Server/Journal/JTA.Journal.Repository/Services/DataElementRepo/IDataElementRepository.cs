using HsR.Journal.Entities;
using HsR.Journal.Entities.TradeJournal;

namespace HsR.Journal.DataContext
{
    public interface IDataElementRepository
    {
        Task<(DataElement updatedCell, TradeSummary? summary, DateTime? newTimeStamp)> 
                                        UpdateCellContentAsync(string componentId, string newContent, string changeNote);
    }

}