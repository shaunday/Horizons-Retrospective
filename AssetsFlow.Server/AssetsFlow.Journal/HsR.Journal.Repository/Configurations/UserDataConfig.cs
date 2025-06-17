using HsR.Journal.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HsR.Journal.Repository.Configurations
{
    public class UserDataConfig : IEntityTypeConfiguration<UserData>
    {
        public void Configure(EntityTypeBuilder<UserData> builder)
        {
            builder.HasKey(u => u.Id);

            builder
               .Property(e => e.SavedSectors)
               .HasConversion(new JsonCollectionConverter())
               .HasColumnType("jsonb")  // store as json in PostgreSQL
               .Metadata.SetValueComparer(ValueComparers.StringCollectionComparer);
        }
    }
}
