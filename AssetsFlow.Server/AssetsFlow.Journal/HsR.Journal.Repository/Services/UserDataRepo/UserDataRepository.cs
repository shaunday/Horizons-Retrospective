using HsR.Journal.Entities;
using HsR.Journal.Repository.Services.Base;
using Microsoft.EntityFrameworkCore;

namespace HsR.Journal.DataContext
{
    public class UserDataRepository(TradingJournalDataContext dataContext) : 
                                                        JournalRepositoryBase(dataContext), IUserDataRepository
    {
        public async Task<UserData> GetOrCreateUserDataAsync(Guid userId)
        {
            var userData = await _dataContext.UserData.FirstOrDefaultAsync(ud => ud.UserId == userId);
            
            if (userData == null)
            {
                userData = new UserData { UserId = userId };
                _dataContext.UserData.Add(userData);
                await _dataContext.SaveChangesAsync();
            }
            
            return userData;
        }

        public async Task SaveSectorAsync(Guid userId, string sector)
        {
            var userData = await GetOrCreateUserDataAsync(userId);
            if (userData.SavedSectors == null)
            {
                userData.SavedSectors = new List<string>();
            }
            userData.SavedSectors.Add(sector);
            await _dataContext.SaveChangesAsync();
        }

        public async Task<List<string>> GetSavedSectorsAsync(Guid userId)
        {
            var userData = await _dataContext.UserData.AsNoTracking().FirstOrDefaultAsync(ud => ud.UserId == userId);
            return userData?.SavedSectors?.OrderBy(s => s).ToList() ?? new List<string>();
        }
    }
}