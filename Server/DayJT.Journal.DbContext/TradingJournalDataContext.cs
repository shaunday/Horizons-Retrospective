using DayJT.Journal.Data;
using Microsoft.EntityFrameworkCore;

namespace DayJT.Journal.DataContext.Services
{
    public class TradingJournalDataContext : DbContext
    {
        #region Data
        public DbSet<TradeComposite> AllTradeComposites { get; set; } 

        public DbSet<TradeElement> AllTradeElements { get; set; }

        public DbSet<Cell> AllTradeInfoCells { get; set; } 

        public DbSet<CellContent> AllCellContents { get; set; } 
        #endregion

        public TradingJournalDataContext(DbContextOptions<TradingJournalDataContext> options) : base(options) { } //allow service configuration 

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<TradeComposite>()
                .HasMany(t => t.TradeElements)
                .WithOne(t => t.TradeCompositeRef)
                .HasForeignKey(t => t.TradeCompositeRefId)
                .IsRequired(); //force FK

            modelBuilder.Entity<TradeElement>()
               .HasMany(t => t.Entries)
               .WithOne(t => t.TradeElementRef)
               .HasForeignKey(t => t.TradeElementRefId)
               .IsRequired(); //force FK

            modelBuilder.Entity<Cell>()
                .HasMany(t => t.History)
                .WithOne(t => t.CellRef)
                .HasForeignKey(t => t.CellRefId)
                .IsRequired(false); //force FK

        }

    }
}
