using HsR.Journal.Repository.Services.Base;
using Microsoft.EntityFrameworkCore;

namespace HsR.Journal.DataContext
{
    public class UserDataRepository(TradingJournalDataContext dataContext) : 
                                                        JournalRepositoryBase(dataContext), IUserDataRepository
    {
        public async Task<List<string>?> GetAllSavedSectors()
        {
            var userData = await _dataContext.UserData.AsNoTracking().SingleOrDefaultAsync();
            return userData?.SavedSectors?.OrderBy(s => s).ToList();
        }
    }
}