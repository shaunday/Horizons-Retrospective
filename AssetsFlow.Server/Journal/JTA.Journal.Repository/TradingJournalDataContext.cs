using HsR.Journal.Repository.Configurations;
using HsR.Journal.Entities;
using Microsoft.EntityFrameworkCore;

namespace HsR.Journal.DataContext
{
    public class TradingJournalDataContext(DbContextOptions<TradingJournalDataContext> options) : DbContext(options)
    {
        public DbSet<TradeComposite> TradeComposites { get; set; } = null!;
        public DbSet<TradeAction> TradeElements { get; set; } = null!; 
        public DbSet<DataElement> Entries { get; set; } = null!;
        
        public DbSet<UserData> UserData { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new UserDataConfig());

            modelBuilder.ApplyConfiguration(new TradeCompositeConfig());
            modelBuilder.ApplyConfiguration(new TradeElementsConfig());
            modelBuilder.ApplyConfiguration(new DataElementConfig());
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            int saveRes = await base.SaveChangesAsync(cancellationToken);
            //ChangeTracker.Clear(); // Clear tracker after saving //todo - reconsider
            return saveRes;
        }
    }
}
