using DayJT.Journal.DataEntities.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DayJT.Journal.Repository.Configurations
{
    public class CellConfiguration : IEntityTypeConfiguration<Cell>
    {
        public void Configure(EntityTypeBuilder<Cell> builder)
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
        }
    }
}
