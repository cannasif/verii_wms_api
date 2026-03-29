using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WMS_WEBAPI.Models;

namespace WMS_WEBAPI.Data.Configuration
{
    public class MobileUserGroupMatchConfiguration : BaseEntityConfiguration<MobileUserGroupMatch>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<MobileUserGroupMatch> builder)
        {
            // Table name
            builder.ToTable("RII_MOBIL_USER_GROUP_MATCH");

            builder.Property(x => x.UserId)
                .IsRequired()
                .HasColumnName("UserId");

            builder.Property(x => x.GroupCode)
                .IsRequired()
                .HasColumnName("GroupCode");


            // Relationships
            builder.HasOne(x => x.Users)
                .WithMany()
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(x => x.MobilePageGroups)
                .WithMany(x => x.UserGroupMatches)
                .UsingEntity(j => j.ToTable("RII_MOBIL_USER_PAGE_GROUP_MATCH"));

        }
    }
}