using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WMS_WEBAPI.Models;

namespace WMS_WEBAPI.Data.Configuration
{
    public class GrHeaderConfiguration : BaseHeaderEntityConfiguration<GrHeader>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<GrHeader> builder)
        {
            builder.ToTable("RII_GR_HEADER");

            builder.Property(x => x.CustomerCode)
                .IsRequired()
                .HasMaxLength(30)
                .HasColumnName("CustomerCode");

            builder.Property(x => x.ReturnCode)
                .HasDefaultValue(false)
                .HasColumnName("ReturnCode");

            builder.Property(x => x.OCRSource)
                .HasDefaultValue(false)
                .HasColumnName("OCRSource");

            builder.Property(x => x.IsPlanned)
                .HasDefaultValue(false)
                .HasColumnName("IsPlanned");

            builder.Property(x => x.Description1)
                .HasMaxLength(100)
                .HasColumnName("Description1");

            builder.Property(x => x.Description2)
                .HasMaxLength(100)
                .HasColumnName("Description2");

            builder.Property(x => x.Description3)
                .HasMaxLength(100)
                .HasColumnName("Description3");

            builder.Property(x => x.Description4)
                .HasMaxLength(100)
                .HasColumnName("Description4");

            builder.Property(x => x.Description5)
                .HasMaxLength(100)
                .HasColumnName("Description5");

            // Indexes
            builder.HasIndex(x => x.BranchCode)
                .HasDatabaseName("IX_GrHeader_BranchCode");

            builder.HasIndex(x => x.CustomerCode)
                .HasDatabaseName("IX_GrHeader_CustomerCode");

            builder.HasIndex(x => x.PlannedDate)
                .HasDatabaseName("IX_GrHeader_PlannedDate");
        }
    }
}
