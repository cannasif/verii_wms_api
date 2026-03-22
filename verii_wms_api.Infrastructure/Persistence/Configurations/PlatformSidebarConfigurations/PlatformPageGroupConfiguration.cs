using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WMS_WEBAPI.Models;

namespace WMS_WEBAPI.Data.Configuration
{
    public class PlatformPageGroupConfiguration : BaseEntityConfiguration<PlatformPageGroup>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<PlatformPageGroup> builder)
        {
            // Table name
            builder.ToTable("PlatformPageGroup");

            builder.Property(x => x.GroupCode)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnName("GroupCode");

            builder.HasIndex(x => x.GroupCode)
                .HasDatabaseName("IX_PlatformPageGroup_GroupCode");

            builder.HasIndex(x => x.IsDeleted)
                .HasDatabaseName("IX_PlatformPageGroup_IsDeleted");

            builder.HasQueryFilter(x => !x.IsDeleted);
        }
    }
}
