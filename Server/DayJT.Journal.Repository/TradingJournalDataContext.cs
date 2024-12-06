using DayJT.Journal.Data;
using DayJTrading.Journal.Data;
using Microsoft.EntityFrameworkCore;

namespace DayJT.Journal.DataContext.Services
{
    public class TradingJournalDataContext : DbContext
    {
        public DbSet<TradeComposite> AllTradeComposites { get; set; } = null!;

        public DbSet<TradeElement> AllTradeElements { get; set; } = null!;

        public DbSet<Cell> AllEntries { get; set; } = null!;

        public DbSet<ContentRecord> AllContentRecords { get; set; } = null!;

        public DbSet<JournalData> JournalData { get; set; } = null!;

        public TradingJournalDataContext(DbContextOptions<TradingJournalDataContext> options) : base(options) { } //allow service configuration 

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<JournalData>(entity =>
            {
                entity.HasNoKey();
            });

            modelBuilder.Entity<TradeComposite>() //force eager load of TradeElements
                .Navigation(tc => tc.TradeElements)  
                .AutoInclude();  

            modelBuilder.Entity<TradeElement>() //same for entries.. no point in a trade element without entries
                .Navigation(te => te.Entries)
                .AutoInclude();  

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

            //force Cell to also have trade composite Ref/ FK
            modelBuilder.Entity<Cell>()
                .HasOne(c => c.TradeCompositeRef)
                .WithMany()
                .HasForeignKey(c => c.TradeCompositeFK)
                .IsRequired(); //force FK

        }
    }
}
