using HsR.Common;
using HsR.Journal.Entities;
using HsR.Journal.Repository;

namespace HsR.Journal.DataContext
{
    public interface IGeneralDataRepository
    {
        Task<List<string>?> GetAllSavedSectors();
    } 
}
