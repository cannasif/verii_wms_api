using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WMS_WEBAPI.Models;

namespace WMS_WEBAPI.Data.Configuration
{
    public class WoHeaderConfiguration : BaseHeaderEntityConfiguration<WoHeader>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<WoHeader> builder)
        {
            builder.ToTable("RII_WO_HEADER");

            builder.Property(x => x.BranchCode).HasMaxLength(10).IsRequired();
            builder.Property(x => x.ProjectCode).HasMaxLength(20);
            builder.Property(x => x.DocumentType).HasMaxLength(10).IsRequired();
            builder.Property(x => x.OutboundType).HasMaxLength(10).IsRequired();
            builder.Property(x => x.AccountCode).HasMaxLength(20);
            builder.Property(x => x.CustomerCode).HasMaxLength(20);
            builder.Property(x => x.SourceWarehouse).HasMaxLength(20);
            builder.Property(x => x.TargetWarehouse).HasMaxLength(20);
            builder.Property(x => x.YearCode).HasMaxLength(4).IsRequired();
            builder.Property(x => x.Description1).HasMaxLength(50);
            builder.Property(x => x.Description2).HasMaxLength(100);
            builder.Property(x => x.Type).IsRequired();

            builder.HasIndex(x => x.BranchCode).HasDatabaseName("IX_WoHeader_BranchCode");
            builder.HasIndex(x => x.PlannedDate).HasDatabaseName("IX_WoHeader_PlannedDate");
            builder.HasIndex(x => x.AccountCode).HasDatabaseName("IX_WoHeader_AccountCode");
            builder.HasIndex(x => x.CustomerCode).HasDatabaseName("IX_WoHeader_CustomerCode");
            builder.HasIndex(x => x.YearCode).HasDatabaseName("IX_WoHeader_YearCode");
            builder.HasIndex(x => x.IsDeleted).HasDatabaseName("IX_WoHeader_IsDeleted");

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