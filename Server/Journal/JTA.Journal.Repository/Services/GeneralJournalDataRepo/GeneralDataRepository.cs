using Microsoft.EntityFrameworkCore;

namespace JTA.Journal.DataContext
{
    public partial class GeneralDataRepository(TradingJournalDataContext dataContext) : JournalRepositoryBase(dataContext), IGeneralDataRepository
    {
        public async Task<List<string>?> GetAllSavedSectors()
        {
            var journalData = await _dataContext.JournalData.AsNoTracking().SingleOrDefaultAsync();
            return journalData?.SavedSectors?.OrderBy(s => s).ToList();
        }
    }
}