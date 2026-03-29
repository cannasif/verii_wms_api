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

            builder.HasMany(x => x.UserGroupMatches)
                .WithMany(x => x.MobilePageGroups);

        }
    }
}
