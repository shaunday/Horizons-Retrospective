using DayJT.Journal.Data;
using DayJTrading.Journal.Data;
using Microsoft.EntityFrameworkCore;

namespace DayJT.Journal.DataContext.Services
{
    public class TradingJournalDataContext : DbContext
    {
        public DbSet<TradeComposite> AllTradeComposites { get; set; } 

        public DbSet<TradeElement> AllTradeElements { get; set; }

        public DbSet<Cell> AllEntries { get; set; } 

        public DbSet<ContentRecord> AllContentRecords { get; set; } 

        public DbSet<JournalData> JournalData { get; set; }

        public TradingJournalDataContext(DbContextOptions<TradingJournalDataContext> options) : base(options) { } //allow service configuration 

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<JournalData>(entity =>
            {
                entity.HasNoKey(); 
            });

            modelBuilder.Entity<TradeComposite>()
                .HasMany(t => t.TradeElements)
                .WithOne(t => t.TradeCompositeRef)
                .HasForeignKey(t => t.TradeCompositeFK)
                .IsRequired(); //force FK

            modelBuilder.Entity<TradeElement>()
               .HasMany(t => t.Entries)
               .WithOne(t => t.TradeElementRef)
               .HasForeignKey(t => t.TradeElementFK)
               .IsRequired(); //force FK

            modelBuilder.Entity<Cell>()
                .HasMany(t => t.History)
                .WithOne(t => t.CellRef)
                .HasForeignKey(t => t.CellFK)
                .IsRequired(); //force FK

        }
    }
}
