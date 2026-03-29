using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WMS_WEBAPI.Models;

namespace WMS_WEBAPI.Data.Configuration
{
    public class SitHeaderConfiguration : BaseHeaderEntityConfiguration<SitHeader>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<SitHeader> builder)
        {
            builder.ToTable("RII_SIT_HEADER");

            builder.Property(x => x.BranchCode)
                .HasMaxLength(10)
                .IsRequired();

            builder.Property(x => x.ProjectCode)
                .HasMaxLength(20);


            builder.Property(x => x.DocumentType)
                .HasMaxLength(10)
                .IsRequired();

            builder.Property(x => x.CustomerCode)
                .HasMaxLength(20);

            builder.Property(x => x.SourceWarehouse)
                .HasMaxLength(20);

            builder.Property(x => x.TargetWarehouse)
                .HasMaxLength(20);


            builder.Property(x => x.YearCode)
                .HasMaxLength(4)
                .IsRequired();

            builder.Property(x => x.PriorityLevel);



            builder.HasIndex(x => x.BranchCode)
                .HasDatabaseName("IX_SitHeader_BranchCode");

            builder.HasIndex(x => x.ProjectCode)
                .HasDatabaseName("IX_SitHeader_ProjectCode");

            builder.HasIndex(x => x.PlannedDate)
                .HasDatabaseName("IX_SitHeader_PlannedDate");

            builder.HasIndex(x => x.CustomerCode)
                .HasDatabaseName("IX_SitHeader_CustomerCode");

            builder.HasIndex(x => x.YearCode)
                .HasDatabaseName("IX_SitHeader_YearCode");

            builder.HasIndex(x => x.IsDeleted)
                .HasDatabaseName("IX_SitHeader_IsDeleted");

            builder.HasMany(x => x.Lines)
                .WithOne(x => x.Header)
                .HasForeignKey(x => x.HeaderId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(x => x.ImportLines)
                .WithOne(x => x.Header)
                .HasForeignKey(x => x.HeaderId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}