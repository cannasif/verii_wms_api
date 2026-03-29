using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WMS_WEBAPI.Models;

namespace WMS_WEBAPI.Data.Configuration
{
    public class PlatformUserGroupMatchConfiguration : BaseEntityConfiguration<PlatformUserGroupMatch>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<PlatformUserGroupMatch> builder)
        {
            builder.ToTable("RII_PLATFORM_USER_GROUP_MATCH");

            builder.Property(x => x.UserId)
                .IsRequired()
                .HasColumnName("UserId");

            builder.Property(x => x.GroupCode)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnName("GroupCode");


            // Indexes
            builder.HasIndex(x => x.UserId)
                .HasDatabaseName("IX_PlatformUserGroupMatch_UserId");

            builder.HasIndex(x => x.GroupCode)
                .HasDatabaseName("IX_PlatformUserGroupMatch_GroupCode");

            builder.HasIndex(x => new { x.UserId, x.GroupCode })
                .IsUnique()
                .HasDatabaseName("IX_PlatformUserGroupMatch_UserId_GroupCode");

            builder.HasIndex(x => x.IsDeleted)
                .HasDatabaseName("IX_PlatformUserGroupMatch_IsDeleted");

            // Query filter for soft delete
            builder.HasQueryFilter(x => !x.IsDeleted);
        }
    }
}