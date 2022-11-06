using Microsoft.EntityFrameworkCore;

namespace TraJedi.Journal.Data
{
    public class TradingJournalDataContext : DbContext
    {
        #region Data
        public DbSet<TradeModel> OverallTrades { get; set; }

        public DbSet<TradeInputModel> TradeInputs { get; set; }

        public DbSet<InputComponentModel> TradeInputComponents { get; set; }

        public DbSet<ContentModel> ContentModels { get; set; } 
        #endregion

        public TradingJournalDataContext(DbContextOptions<TradingJournalDataContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<TradeModel>(entity =>
            {
                entity.HasMany(t => t.TradeInputs).WithOne(t => t.ParentTrade);
            });
        }

    }
}
