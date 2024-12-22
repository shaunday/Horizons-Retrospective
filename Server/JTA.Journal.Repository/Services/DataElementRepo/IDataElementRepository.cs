using JTA.Journal.Entities;

namespace JTA.Journal.DataContext
{
    public interface IDataElementRepository
    {
        Task<(DataElement updatedCell, TradeElement? summary)> UpdateCellContentAsync(string componentId, string newContent, string changeNote);
    }

}