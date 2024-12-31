using JTA.Journal.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JTA.Journal.Repository.Configurations
{
    public class TradeElementConfig : IEntityTypeConfiguration<TradeElement>
    {
        public void Configure(EntityTypeBuilder<TradeElement> builder)
        {
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
