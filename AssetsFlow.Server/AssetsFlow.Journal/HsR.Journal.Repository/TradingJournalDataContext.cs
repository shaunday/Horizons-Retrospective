using HsR.Journal.Repository.Configurations;
using HsR.Journal.Entities;
using Microsoft.EntityFrameworkCore;
using HsR.Common.Db;

namespace HsR.Journal.DataContext
{
    public class TradingJournalDataContext(DbContextOptions<TradingJournalDataContext> options) : BaseDbContext(options)
    {
        public DbSet<TradeComposite> TradeComposites { get; set; } = null!;
        public DbSet<TradeElement> TradeElements { get; set; } = null!; 
        public DbSet<DataElement> Entries { get; set; } = null!;
        
        public DbSet<UserData> UserData { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(TradingJournalDataContext).Assembly);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            int saveRes = await base.SaveChangesAsync(cancellationToken);
            //ChangeTracker.Clear(); // Clear tracker after saving //todo - reconsider
            return saveRes;
        }
    }
}
