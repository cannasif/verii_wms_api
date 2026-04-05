using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wms.Domain.Entities.WarehouseTransfer;
using Wms.Infrastructure.Persistence.Configurations.Common;

namespace Wms.Infrastructure.Persistence.Configurations.WarehouseTransfer;

public sealed class WtHeaderConfiguration : BaseHeaderEntityConfiguration<WtHeader>
{
    protected override void ConfigureEntity(EntityTypeBuilder<WtHeader> builder)
    {
        builder.ToTable("RII_WT_HEADER");

        builder.Property(x => x.BranchCode).HasMaxLength(10).IsRequired();
        builder.Property(x => x.CustomerCode).HasMaxLength(20);
        builder.Property(x => x.CustomerId).IsRequired(false);
        builder.Property(x => x.SourceWarehouse).HasMaxLength(20);
        builder.Property(x => x.SourceWarehouseId).IsRequired(false);
        builder.Property(x => x.TargetWarehouse).HasMaxLength(20);
        builder.Property(x => x.TargetWarehouseId).IsRequired(false);
        builder.Property(x => x.ElectronicWaybill).IsRequired();
        builder.Property(x => x.ShipmentId);

        builder.HasIndex(x => x.BranchCode).HasDatabaseName("IX_TrHeader_BranchCode");
        builder.HasIndex(x => x.ProjectCode).HasDatabaseName("IX_TrHeader_ProjectCode");
        builder.HasIndex(x => x.PlannedDate).HasDatabaseName("IX_TrHeader_PlannedDate");
        builder.HasIndex(x => x.CustomerCode).HasDatabaseName("IX_TrHeader_CustomerCode");
        builder.HasIndex(x => x.CustomerId).HasDatabaseName("IX_TrHeader_CustomerId");
        builder.HasIndex(x => x.SourceWarehouseId).HasDatabaseName("IX_TrHeader_SourceWarehouseId");
        builder.HasIndex(x => x.TargetWarehouseId).HasDatabaseName("IX_TrHeader_TargetWarehouseId");
        builder.HasIndex(x => x.IsDeleted).HasDatabaseName("IX_TrHeader_IsDeleted");
        builder.HasIndex(x => x.YearCode).HasDatabaseName("IX_TrHeader_YearCode");

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
