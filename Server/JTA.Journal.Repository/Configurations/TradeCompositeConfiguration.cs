using DayJT.Journal.DataEntities.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DayJT.Journal.Repository.Configurations
{
    public class TradeCompositeConfiguration : IEntityTypeConfiguration<TradeComposite>
    {
        public void Configure(EntityTypeBuilder<TradeComposite> builder)
        {
            builder.HasMany(t => t.TradeElements)
                .WithOne(t => t.TradeCompositeRef)
                .HasForeignKey(t => t.TradeCompositeFK)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.Navigation(tc => tc.TradeElements).AutoInclude();
        }
    }
}
