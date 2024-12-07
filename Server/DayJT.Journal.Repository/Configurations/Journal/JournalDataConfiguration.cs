using DayJT.Journal.DataEntities.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DayJT.Journal.Repository.Configurations
{
    public class JournalDataConfiguration : IEntityTypeConfiguration<JournalData>
    {
        public void Configure(EntityTypeBuilder<JournalData> builder)
        {
            builder.HasNoKey();

            builder.Property(e => e.SavedSectors)
                .HasColumnType("jsonb");

            builder.Property(e => e.SavedBrokers)
                .HasColumnType("jsonb");
        }
    }
}
