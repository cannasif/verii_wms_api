using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WMS_WEBAPI.Models;

namespace WMS_WEBAPI.Data.Configuration
{
    public class MobilePageGroupConfiguration : BaseEntityConfiguration<MobilePageGroup>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<MobilePageGroup> builder)
        {
            // Table name
            builder.ToTable("RII_MOBIL_PAGE_GROUP");

            builder.Property(x => x.GroupName)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnName("GroupName");

            builder.Property(x => x.GroupCode)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnName("GroupCode");

            builder.Property(x => x.MenuHeaderId)
                .HasColumnName("MenuHeaderId");

            builder.Property(x => x.MenuLineId)
                .HasColumnName("MenuLineId");


            // Relationships
            builder.HasOne(x => x.MenuHeaders)
                .WithMany()
                .HasForeignKey(x => x.MenuHeaderId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.MenuLines)
                .WithMany()
                .HasForeignKey(x => x.MenuLineId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(x => x.UserGroupMatches)
                .WithMany(x => x.MobilePageGroups);

        }
    }
}