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

            builder.Property(x => x.MenuHeaderId)
                .IsRequired()
                .HasColumnName("MenuHeaderId");

            builder.Property(x => x.MenuLineId)
                .IsRequired()
                .HasColumnName("MenuLineId");


            // Indexes
            builder.HasIndex(x => x.GroupCode)
                .HasDatabaseName("IX_PlatformPageGroup_GroupCode");

            builder.HasIndex(x => x.MenuHeaderId)
                .HasDatabaseName("IX_PlatformPageGroup_MenuHeaderId");

            builder.HasIndex(x => x.MenuLineId)
                .HasDatabaseName("IX_PlatformPageGroup_MenuLineId");

            builder.HasIndex(x => x.IsDeleted)
                .HasDatabaseName("IX_PlatformPageGroup_IsDeleted");

            // Foreign key relationships
            builder.HasOne(x => x.MenuHeaders)
                .WithMany()
                .HasForeignKey(x => x.MenuHeaderId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.MenuLines)
                .WithMany()
                .HasForeignKey(x => x.MenuLineId)
                .OnDelete(DeleteBehavior.Restrict);

            // Query filter for soft delete
            builder.HasQueryFilter(x => !x.IsDeleted);
        }
    }
}