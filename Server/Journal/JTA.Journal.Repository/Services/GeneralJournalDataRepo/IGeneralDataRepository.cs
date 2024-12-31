using JTA.Common;
using JTA.Journal.Entities;
using JTA.Journal.Repository;

namespace JTA.Journal.DataContext
{
    public interface IGeneralDataRepository
    {
        Task<List<string>?> GetAllSavedSectors();
    } 
}
