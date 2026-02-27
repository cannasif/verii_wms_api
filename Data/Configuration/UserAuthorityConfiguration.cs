using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WMS_WEBAPI.Models;

namespace WMS_WEBAPI.Data.Configuration
{
    public class UserAuthorityConfiguration : BaseEntityConfiguration<UserAuthority>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<UserAuthority> builder)
        {
            // Table name
            builder.ToTable("RII_USER_AUTHORITY");

            // Properties
            builder.Property(x => x.Title)
                .IsRequired()
                .HasMaxLength(30)
                .HasColumnName("Title");


            // Query filter for soft delete
            builder.HasQueryFilter(x => !x.IsDeleted);

            // Seed data
            builder.HasData(
                new UserAuthority { Id = 1, Title = "user", CreatedDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
                new UserAuthority { Id = 2, Title = "admin", CreatedDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
                new UserAuthority { Id = 3, Title = "superadmin", CreatedDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) }
            );
        }
    }
}