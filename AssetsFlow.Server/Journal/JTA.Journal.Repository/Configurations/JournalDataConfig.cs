using HsR.Journal.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HsR.Journal.Repository.Configurations
{
    public class JournalDataConfig : IEntityTypeConfiguration<UserData>
    {
        public void Configure(EntityTypeBuilder<UserData> builder)
        {
            builder.HasNoKey();

            builder.Property(e => e.SavedSectors)
                .HasColumnType("jsonb");

            builder.Property(e => e.SavedBrokers)
                .HasColumnType("jsonb");
        }
    }
}
