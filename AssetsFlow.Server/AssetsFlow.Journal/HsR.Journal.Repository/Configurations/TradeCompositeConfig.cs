using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HsR.Journal.Repository.Configurations
{
    public class TradeCompositeConfig : IEntityTypeConfiguration<TradeComposite>
    {
        public void Configure(EntityTypeBuilder<TradeComposite> builder)
        {
            builder.HasIndex(tc => tc.UserId);

            builder.HasOne(tc => tc.Summary)
                .WithOne()
                .HasForeignKey<TradeComposite>("SummaryId") //shadow prop
                .OnDelete(DeleteBehavior.NoAction);

            builder.Navigation(tc => tc.Summary).AutoInclude();

            builder.HasMany(tc => tc.TradeElements)
                .WithOne(tc => tc.CompositeRef)
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
