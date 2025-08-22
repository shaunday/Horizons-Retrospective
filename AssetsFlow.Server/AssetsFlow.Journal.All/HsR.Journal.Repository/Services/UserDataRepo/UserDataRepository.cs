using HsR.Journal.Entities;
using HsR.Journal.Repository.Services.Base;
using Microsoft.EntityFrameworkCore;

namespace HsR.Journal.DataContext
{
    public class UserDataRepository : JournalRepositoryBase, IUserDataRepository
    {
        public UserDataRepository(TradingJournalDataContext dataContext)
            : base(dataContext) { }

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

        public async Task<IList<string>> GetAllAvailableSymbolsAsync(Guid userId)
        {
            var symbols = await _dataContext.TradeComposites
                .AsNoTracking()
                .Where(tc => tc.UserId == userId)
                .SelectMany(tc => tc.TradeElements)                       // flatten TradeElements
                .Where(te => te.TradeActionType == TradeActionType.Origin) // only Origin actions
                .SelectMany(te => te.Entries)                             // flatten entries
                .Where(e => e.FilterId == FilterId.Symbol)               // only Symbol entries
                .Select(e => e.Content)                                  // get the symbol value
                .Distinct()                                              // unique symbols
                .ToListAsync();

            return symbols;
        }

    }
}