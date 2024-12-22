using DayJT.Journal.Data;
using DayJT.Journal.DataContext.Services;
using DayJT.Journal.DataEntities.Entities;
using DayJTrading.Journal.Data;
using Microsoft.EntityFrameworkCore;

namespace DayJT.Journal.Repository.Services
{
    public partial class TradingJournalRepository : ITradingJournalRepository
    {
        //Gen. Data

        public async Task<List<string>?> GetAllSavedSectors()
        {
            var journalData = await dataContext.JournalData.AsNoTracking().SingleOrDefaultAsync();
            return journalData?.SavedSectors?.OrderBy(s => s).ToList();
        }
    }
}