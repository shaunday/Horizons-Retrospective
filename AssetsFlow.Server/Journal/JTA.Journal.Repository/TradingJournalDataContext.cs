using HsR.Journal.Repository.Configurations;
using HsR.Journal.Entities;
using Microsoft.EntityFrameworkCore;

namespace HsR.Journal.DataContext
{
    public class TradingJournalDataContext(DbContextOptions<TradingJournalDataContext> options) : DbContext(options)
    {
        public DbSet<TradeComposite> TradeComposites { get; set; } = null!;
        public DbSet<TradeElement> TradeElements { get; set; } = null!;  //must have this to allow principality of TradeElement in the r/ship with cell
        public DbSet<DataElement> Entries { get; set; } = null!;
        
        public DbSet<UserData> JournalData { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new UserDataConfig());

            modelBuilder.ApplyConfiguration(new TradeCompositeConfig());
            modelBuilder.ApplyConfiguration(new TradeElementConfig());
            modelBuilder.ApplyConfiguration(new DataElementConfig());
        }

        //override save changes to clear tracker after saving
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            int saveRes = await base.SaveChangesAsync(cancellationToken);
            ChangeTracker.Clear(); // Clear tracker after saving
            return saveRes;
        }
    }
}
