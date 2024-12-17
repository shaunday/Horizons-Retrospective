using DayJT.Journal.DataEntities.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DayJT.Journal.Repository.Configurations
{
    public class CellConfiguration : IEntityTypeConfiguration<DataElement>
    {
        public void Configure(EntityTypeBuilder<DataElement> builder)
        {
            builder.OwnsOne(c => c.ContentWrapper);
            builder.Navigation(c => c.ContentWrapper).AutoInclude();

            builder.OwnsMany(t => t.History)
                .WithOwner(h => h.CellRef);

            builder.HasOne(c => c.TradeCompositeRef)
                .WithMany()
                .HasForeignKey(c => c.TradeCompositeFK)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            // Enum to string conversions
            builder.Property(c => c.ComponentType)
                .HasConversion<string>();

            builder.Property(c => c.CostRelevance)
                .HasConversion<string>();

            builder.Property(c => c.PriceRelevance)
                .HasConversion<string>();
        }
    }
}
