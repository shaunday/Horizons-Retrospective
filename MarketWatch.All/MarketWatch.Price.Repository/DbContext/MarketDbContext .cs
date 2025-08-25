using MarketWatch.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace MarketWatch.Price.Repository
{
    public class MarketDbContext : DbContext
    {
        public DbSet<SecurityModel> Securities { get; set; } = null!;
        public DbSet<PriceBarModel> PriceBars { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var folder = Path.Combine(AppContext.BaseDirectory, "..", "Data");
            Directory.CreateDirectory(folder);
            optionsBuilder.UseSqlite($"Data Source={Path.Combine(folder, "marketwatch.db")}");

            optionsBuilder.LogTo(Console.WriteLine, LogLevel.Information)
                              .EnableSensitiveDataLogging();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SecurityModel>()
                .HasKey(s => s.Id);

            modelBuilder.Entity<PriceBarModel>()
                .HasKey(pb => pb.Id);

            modelBuilder.Entity<SecurityModel>()
                .HasMany(s => s.PriceBars)
                .WithOne()
                .HasForeignKey(pb => pb.SecurityFK)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
