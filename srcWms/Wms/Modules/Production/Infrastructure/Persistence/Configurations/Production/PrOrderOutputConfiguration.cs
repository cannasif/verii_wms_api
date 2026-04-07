using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wms.Domain.Entities.Production;
using Wms.Infrastructure.Persistence.Configurations.Common;

namespace Wms.Infrastructure.Persistence.Configurations.Production;

public sealed class PrOrderOutputConfiguration : BaseEntityConfiguration<PrOrderOutput>
{
    protected override void ConfigureEntity(EntityTypeBuilder<PrOrderOutput> builder)
    {
        builder.ToTable("RII_PR_ORDER_OUTPUT");
        builder.Property(x => x.StockCode).HasMaxLength(50).IsRequired();
        builder.Property(x => x.YapKod).HasMaxLength(50);
        builder.Property(x => x.Unit).HasMaxLength(20);
        builder.Property(x => x.PlannedQuantity).HasColumnType("decimal(18,6)").IsRequired();
        builder.Property(x => x.ProducedQuantity).HasColumnType("decimal(18,6)");
        builder.Property(x => x.TrackingMode).HasMaxLength(20).IsRequired();
        builder.Property(x => x.SerialEntryMode).HasMaxLength(20).IsRequired();
        builder.Property(x => x.TargetWarehouseCode).HasMaxLength(20);
        builder.Property(x => x.TargetCellCode).HasMaxLength(50);
        builder.Property(x => x.Status).HasMaxLength(20).IsRequired();
        builder.HasIndex(x => x.OrderId).HasDatabaseName("IX_PrOrderOutput_OrderId");
        builder.HasOne(x => x.Order).WithMany(x => x.Outputs).HasForeignKey(x => x.OrderId).OnDelete(DeleteBehavior.Restrict);
    }
}
