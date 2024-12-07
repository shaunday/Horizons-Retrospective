using DayJT.Journal.DataEntities.Entities;
using DayJT.Journal.Repository.Configurations;
using DayJTrading.Journal.Data;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace DayJT.Journal.Repository
{
    public class TradingJournalDataContext(DbContextOptions<TradingJournalDataContext> options) : DbContext(options)
    {
        public DbSet<TradeComposite> TradeComposites { get; set; } = null!;

        public DbSet<TradeElement> TradeElements { get; set; } = null!; //must have this to allow principality of TradeElement in the r/ship with cell

        public DbSet<Cell> Entries { get; set; } = null!;

        public DbSet<JournalData> JournalData { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new JournalDataConfiguration());
            modelBuilder.ApplyConfiguration(new TradeCompositeConfiguration());
            modelBuilder.ApplyConfiguration(new TradeElementConfiguration());
            modelBuilder.ApplyConfiguration(new CellConfiguration());
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
