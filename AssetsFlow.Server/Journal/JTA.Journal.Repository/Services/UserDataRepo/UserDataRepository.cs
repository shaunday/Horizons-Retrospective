using HsR.Journal.Repository.Services.Base;
using Microsoft.EntityFrameworkCore;

namespace HsR.Journal.DataContext
{
    public partial class UserDataRepository(TradingJournalDataContext dataContext) : 
                                                        JournalRepositoryBase(dataContext), IUserDataRepository
    {
        public async Task<List<string>?> GetAllSavedSectors()
        {
            var journalData = await _dataContext.JournalData.AsNoTracking().SingleOrDefaultAsync();
            return journalData?.SavedSectors?.OrderBy(s => s).ToList();
        }
    }
}