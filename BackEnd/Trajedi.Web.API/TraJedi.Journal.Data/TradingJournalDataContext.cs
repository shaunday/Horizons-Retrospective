using Microsoft.EntityFrameworkCore;

namespace TraJedi.Journal.Data
{
    public class TradingJournalDataContext : DbContext
    {
        #region Data
        public DbSet<TradeConstruct> OverallTrades { get; set; } = null!;

        public DbSet<TradeInputModel>? TradeInputs { get; set; } = null!;

        public DbSet<InputComponentModel>? TradeInputComponents { get; set; } = null!;

        public DbSet<ContentModel>? ContentModels { get; set; } = null!;
        #endregion

        public TradingJournalDataContext(DbContextOptions<TradingJournalDataContext> options) : base(options) { } //allow service configuration 

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<TradeConstruct>(entity =>
            {
                entity.HasMany(t => t.TradeInputs).WithOne(t => t.TradeConstruct);
            });

            modelBuilder.Entity<InputComponentModel>(entity =>
            {
                entity.HasMany(t => t.History).WithOne(t => t.InputComponentModel);
            });
        }

    }
}
