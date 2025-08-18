using HsR.Common;
using HsR.Journal.Entities;
using HsR.Journal.Repository;

namespace HsR.Journal.DataContext
{
    public interface IUserDataRepository
    {
        Task<UserData> GetOrCreateUserDataAsync(Guid userId);
        Task SaveSectorAsync(Guid userId, string sector);
        Task<List<string>> GetSavedSectorsAsync(Guid userId);
    } 
}
