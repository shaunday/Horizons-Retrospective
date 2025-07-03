using HsR.Journal.Entities;
using HsR.Journal.Entities.TradeJournal;
using HsR.Journal.Services;

namespace HsR.Journal.DataContext
{
    public interface IDataElementRepository
    {
        Task<(DataElement updatedCell, UpdatedStatesCollation updatedStates)> UpdateCellContentAsync(int componentId, string newContent, string changeNote);
    }

}