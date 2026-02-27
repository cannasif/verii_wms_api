using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WMS_WEBAPI.Models;

namespace WMS_WEBAPI.Data.Configuration
{
    public class PtHeaderConfiguration : BaseHeaderEntityConfiguration<PtHeader>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<PtHeader> builder)
        {
            builder.ToTable("RII_PT_HEADER");

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
                .HasDatabaseName("IX_PtHeader_BranchCode");

            builder.HasIndex(x => x.ProjectCode)
                .HasDatabaseName("IX_PtHeader_ProjectCode");

            builder.HasIndex(x => x.PlannedDate)
                .HasDatabaseName("IX_PtHeader_PlannedDate");

            builder.HasIndex(x => x.CustomerCode)
                .HasDatabaseName("IX_PtHeader_CustomerCode");

            builder.HasIndex(x => x.YearCode)
                .HasDatabaseName("IX_PtHeader_YearCode");

            builder.HasIndex(x => x.IsDeleted)
                .HasDatabaseName("IX_PtHeader_IsDeleted");

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