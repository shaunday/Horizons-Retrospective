using DayJT.Journal.DataEntities.Entities;
using DayJT.Journal.Repository.Configurations;
using DayJTrading.Journal.Data;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace DayJT.Journal.Repository
{
    public class TradingJournalDataContext : DbContext
    {
        public TradingJournalDataContext(DbContextOptions<TradingJournalDataContext> options) : base(options) { }

        public DbSet<TradeComposite> TradeComposites { get; set; }

        public DbSet<TradeElement> TradeElements { get; set; }  //must have this to allow principality of TradeElement in the r/ship with cell

        public DbSet<DataElement> Entries { get; set; } 

        public DbSet<JournalData> JournalData { get; set; } 

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new JournalDataConfiguration());
            modelBuilder.ApplyConfiguration(new TradeCompositeConfiguration());
            modelBuilder.ApplyConfiguration(new TradeElementConfiguration());
            modelBuilder.ApplyConfiguration(new DataElementConfiguration());
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
