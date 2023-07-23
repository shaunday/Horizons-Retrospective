using Microsoft.EntityFrameworkCore;

namespace DayJT.Journal.Data
{
    public class TradingJournalDataContext : DbContext
    {
        #region Data
        public DbSet<TradePositionComposite> AllTradeComposites { get; set; } 

        public DbSet<TradeComponent> AllTradeComponents { get; set; }

        public DbSet<Cell> AllTradeInfoCells { get; set; } 

        public DbSet<CellContent> AllCellContents { get; set; } 
        #endregion

        public TradingJournalDataContext(DbContextOptions<TradingJournalDataContext> options) : base(options) { } //allow service configuration 

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<TradePositionComposite>()
                .HasMany(t => t.TradeComponents)
                .WithOne(t => t.TradePositionCompositeRef)
                .HasForeignKey(t => t.TradePositionCompositeRefId)
                .IsRequired(); //force FK

            modelBuilder.Entity<TradeComponent>()
               .HasMany(t => t.TradeActionInfoCells)
               .WithOne(t => t.TradeComponentRef)
               .HasForeignKey(t => t.TradeComponentRefId)
               .IsRequired(); //force FK

            modelBuilder.Entity<Cell>()
                .HasMany(t => t.History)
                .WithOne(t => t.CellRef)
                .HasForeignKey(t => t.CellRefId)
                .IsRequired(false); //force FK

        }

    }
}
