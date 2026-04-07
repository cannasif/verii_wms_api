using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wms.Domain.Entities.Production;
using Wms.Infrastructure.Persistence.Configurations.Common;

namespace Wms.Infrastructure.Persistence.Configurations.Production;

public sealed class PrOrderConsumptionConfiguration : BaseEntityConfiguration<PrOrderConsumption>
{
    protected override void ConfigureEntity(EntityTypeBuilder<PrOrderConsumption> builder)
    {
        builder.ToTable("RII_PR_ORDER_CONSUMPTION");
        builder.Property(x => x.StockCode).HasMaxLength(50).IsRequired();
        builder.Property(x => x.YapKod).HasMaxLength(50);
        builder.Property(x => x.Unit).HasMaxLength(20);
        builder.Property(x => x.PlannedQuantity).HasColumnType("decimal(18,6)").IsRequired();
        builder.Property(x => x.ConsumedQuantity).HasColumnType("decimal(18,6)");
        builder.Property(x => x.TrackingMode).HasMaxLength(20).IsRequired();
        builder.Property(x => x.SerialEntryMode).HasMaxLength(20).IsRequired();
        builder.Property(x => x.SourceWarehouseCode).HasMaxLength(20);
        builder.Property(x => x.SourceCellCode).HasMaxLength(50);
        builder.Property(x => x.Status).HasMaxLength(20).IsRequired();
        builder.HasIndex(x => x.OrderId).HasDatabaseName("IX_PrOrderConsumption_OrderId");
        builder.HasOne(x => x.Order).WithMany(x => x.Consumptions).HasForeignKey(x => x.OrderId).OnDelete(DeleteBehavior.Restrict);
    }
}
