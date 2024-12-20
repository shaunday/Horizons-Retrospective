using DayJT.Journal.DataEntities.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DayJT.Journal.Repository.Configurations
{
    public class DataElementConfig : IEntityTypeConfiguration<DataElement>
    {
        public void Configure(EntityTypeBuilder<DataElement> builder)
        {
            builder.OwnsOne(c => c.ContentWrapper)
                .WithOwner()
                .HasForeignKey(h => h.DataElementFK);
            builder.Navigation(c => c.ContentWrapper).AutoInclude();

            builder.OwnsMany(t => t.History)
                .WithOwner()
                .HasForeignKey(h => h.DataElementFK);

            builder.HasOne(c => c.CompositeRef)
                .WithMany()
                .HasForeignKey(c => c.CompositeFK)
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
