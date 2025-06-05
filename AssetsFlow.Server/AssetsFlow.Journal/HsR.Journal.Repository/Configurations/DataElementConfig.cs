using HsR.Journal.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

namespace HsR.Journal.Repository.Configurations
{
    public class DataElementConfig : IEntityTypeConfiguration<DataElement>
    {
        public void Configure(EntityTypeBuilder<DataElement> builder)
        {
            builder.OwnsOne(c => c.ContentWrapper)
                .WithOwner();
            builder.Navigation(c => c.ContentWrapper).AutoInclude();

            builder.OwnsMany(t => t.History)
                .WithOwner();
            builder.Navigation(c => c.History).AutoInclude();

            builder.HasOne(c => c.CompositeRef)
                .WithMany()
                .HasForeignKey(c => c.CompositeFK)
                .IsRequired();

            // Enum to string conversions
            builder.Property(c => c.ComponentType)
                .HasConversion<string>();

            builder.Property(c => c.TotalCostRelevance)
                .HasConversion<string>();

            builder.Property(c => c.UnitPriceRelevance)
            .HasConversion<string>();

            builder
                .Property(e => e.Restrictions)
                .HasConversion(new JsonCollectionConverter())
                .HasColumnType("jsonb")  // store as json in PostgreSQL
                .Metadata.SetValueComparer(ValueComparers.StringCollectionComparer);
        }
    }
}
