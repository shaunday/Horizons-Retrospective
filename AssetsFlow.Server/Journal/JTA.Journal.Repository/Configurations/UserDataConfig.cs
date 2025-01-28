using HsR.Journal.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HsR.Journal.Repository.Configurations
{
    public class UserDataConfig : IEntityTypeConfiguration<UserData>
    {
        public void Configure(EntityTypeBuilder<UserData> builder)
        {
            builder.HasNoKey();

            builder
               .Property(e => e.SavedSectors)
               .HasConversion(new JsonCollectionConverter())
               .HasColumnType("jsonb");  // Make sure it's stored as json in PostgreSQL

            builder
               .Property(e => e.SavedBrokers)
               .HasConversion(new JsonCollectionConverter())
               .HasColumnType("jsonb");  // Make sure it's stored as json in PostgreSQL
        }
    }
}
