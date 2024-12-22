using JTA.Journal.DataContext.Services;
using Microsoft.EntityFrameworkCore;

namespace JTA.Journal.Repository.Services
{
    public partial class JournalRepository : IJournalRepository
    {
        //Gen. Data

        public async Task<List<string>?> GetAllSavedSectors()
        {
            var journalData = await dataContext.JournalData.AsNoTracking().SingleOrDefaultAsync();
            return journalData?.SavedSectors?.OrderBy(s => s).ToList();
        }
    }
}