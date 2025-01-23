using HsR.Journal.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HsR.Journal.Repository.Configurations
{
    public class TradeCompositeConfig : IEntityTypeConfiguration<TradeComposite>
    {
        public void Configure(EntityTypeBuilder<TradeComposite> builder)
        {
            builder.HasMany(t => t.TradeElements)
                .WithOne(t => t.CompositeRef)
                .HasForeignKey(t => t.CompositeFK)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.Navigation(tc => tc.TradeElements).AutoInclude();

            builder.HasOne(t => t.Summary)
                .WithOne()
                .HasForeignKey<TradeElement>(te => te.CompositeFK)  
                .OnDelete(DeleteBehavior.SetNull); // Allows deletion without affecting the related entity

            // Enum to string conversion
            builder.Property(te => te.Status)
                .HasConversion<string>();
        }
    }
}
