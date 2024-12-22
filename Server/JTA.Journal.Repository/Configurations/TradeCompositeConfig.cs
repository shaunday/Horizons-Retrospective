using JTA.Journal.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JTA.Journal.Repository.Configurations
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

            // Enum to string conversion
            builder.Property(te => te.Status)
                .HasConversion<string>();
        }
    }
}
