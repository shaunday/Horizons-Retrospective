using Microsoft.EntityFrameworkCore;

namespace TraJedi.Journal.Data
{
    public class TradingJournalDataContext : DbContext
    {
        #region Data
        public DbSet<TradePositionComposite> OverallTrades { get; set; } = null!;

        public DbSet<TradeInfoSingleLine> TradeInputs { get; set; } = null!;

        public DbSet<Cell> TradeInputComponents { get; set; } = null!;

        public DbSet<CellContent> ContentModels { get; set; } = null!;
        #endregion

        public TradingJournalDataContext(DbContextOptions<TradingJournalDataContext> options) : base(options) { } //allow service configuration 

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<TradePositionComposite>(entity =>
            {
                entity.HasMany(t => t.TradeComponents).WithOne(t => t.TradePositionComposite);
            });

            modelBuilder.Entity<Cell>(entity =>
            {
                entity.HasMany(t => t.History).WithOne(t => t.Cell);
            });
        }

    }
}
