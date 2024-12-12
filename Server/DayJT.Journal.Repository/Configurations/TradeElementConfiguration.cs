using DayJT.Journal.DataEntities.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DayJT.Journal.Repository.Configurations
{
    public class TradeElementConfiguration : IEntityTypeConfiguration<TradeElement>
    {
        public void Configure(EntityTypeBuilder<TradeElement> builder)
        {
            builder.HasMany(t => t.Entries)
                .WithOne(t => t.TradeElementRef)
                .HasForeignKey(t => t.TradeElementFK)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.Navigation(te => te.Entries).AutoInclude();
        }
    }
}
