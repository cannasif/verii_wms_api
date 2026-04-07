using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wms.Domain.Entities.ProductionTransfer;
using Wms.Infrastructure.Persistence.Configurations.Common;

namespace Wms.Infrastructure.Persistence.Configurations.ProductionTransfer;

public sealed class PtHeaderConfiguration : BaseHeaderEntityConfiguration<PtHeader>
{
    protected override void ConfigureEntity(EntityTypeBuilder<PtHeader> builder)
    {
        builder.ToTable("RII_PT_HEADER");
        builder.Property(x => x.CustomerCode).HasMaxLength(20);
        builder.Property(x => x.CustomerId).IsRequired(false);
        builder.Property(x => x.SourceWarehouse).HasMaxLength(20);
        builder.Property(x => x.SourceWarehouseId).IsRequired(false);
        builder.Property(x => x.TargetWarehouse).HasMaxLength(20);
        builder.Property(x => x.TargetWarehouseId).IsRequired(false);
        builder.Property(x => x.TransferPurpose).HasMaxLength(30).IsRequired();
        builder.HasIndex(x => x.CustomerId).HasDatabaseName("IX_PtHeader_CustomerId");
        builder.HasIndex(x => x.SourceWarehouseId).HasDatabaseName("IX_PtHeader_SourceWarehouseId");
        builder.HasIndex(x => x.TargetWarehouseId).HasDatabaseName("IX_PtHeader_TargetWarehouseId");
        builder.HasIndex(x => x.ProductionHeaderId).HasDatabaseName("IX_PtHeader_ProductionHeaderId");
        builder.HasIndex(x => x.ProductionOrderId).HasDatabaseName("IX_PtHeader_ProductionOrderId");
        builder.HasOne(x => x.ProductionHeader).WithMany().HasForeignKey(x => x.ProductionHeaderId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.ProductionOrder).WithMany(x => x.ProductionTransfers).HasForeignKey(x => x.ProductionOrderId).OnDelete(DeleteBehavior.Restrict);
        builder.HasMany(x => x.Lines).WithOne(x => x.Header).HasForeignKey(x => x.HeaderId).OnDelete(DeleteBehavior.Restrict);
        builder.HasMany(x => x.ImportLines).WithOne(x => x.Header).HasForeignKey(x => x.HeaderId).OnDelete(DeleteBehavior.Restrict);
    }
}
