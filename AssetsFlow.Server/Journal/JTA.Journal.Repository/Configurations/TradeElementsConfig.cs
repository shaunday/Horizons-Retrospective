using HsR.Journal.Entities;
using HsR.Journal.Entities.TradeJournal;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

namespace HsR.Journal.Repository.Configurations
{
    public class TradeElementsConfig : IEntityTypeConfiguration<TradeElement>
    {
        public void Configure(EntityTypeBuilder<TradeElement> builder)
        {
            // Configuring the inheritance hierarchy
            builder
                .HasDiscriminator<string>("TradeElementType")
                .HasValue<TradeSummary>("Summary")
                .HasValue<InterimTradeElement>("Action");

            builder.HasMany(t => t.Entries)
                .WithOne(t => t.TradeElementRef)
                .HasForeignKey(t => t.TradeElementFK)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.Navigation(te => te.Entries).AutoInclude();

            // Enum to string conversion
            builder.Property(te => te.TradeActionType)
                .HasConversion<string>();
        }
    }




}
