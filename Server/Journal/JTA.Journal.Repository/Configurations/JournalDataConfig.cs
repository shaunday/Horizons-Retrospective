using JTA.Journal.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JTA.Journal.Repository.Configurations
{
    public class JournalDataConfig : IEntityTypeConfiguration<JournalData>
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
