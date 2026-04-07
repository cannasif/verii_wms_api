using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wms.Domain.Entities.Production;
using Wms.Infrastructure.Persistence.Configurations.Common;

namespace Wms.Infrastructure.Persistence.Configurations.Production;

public sealed class PrOperationLineConfiguration : BaseEntityConfiguration<PrOperationLine>
{
    protected override void ConfigureEntity(EntityTypeBuilder<PrOperationLine> builder)
    {
        builder.ToTable("RII_PR_OPERATION_LINE");
        builder.Property(x => x.LineRole).HasMaxLength(20).IsRequired();
        builder.Property(x => x.StockCode).HasMaxLength(50).IsRequired();
        builder.Property(x => x.YapKod).HasMaxLength(50);
        builder.Property(x => x.Quantity).HasColumnType("decimal(18,6)").IsRequired();
        builder.Property(x => x.Unit).HasMaxLength(20);
        builder.Property(x => x.SerialNo1).HasMaxLength(100);
        builder.Property(x => x.SerialNo2).HasMaxLength(100);
        builder.Property(x => x.SerialNo3).HasMaxLength(100);
        builder.Property(x => x.SerialNo4).HasMaxLength(100);
        builder.Property(x => x.LotNo).HasMaxLength(100);
        builder.Property(x => x.BatchNo).HasMaxLength(100);
        builder.Property(x => x.SourceWarehouseCode).HasMaxLength(20);
        builder.Property(x => x.TargetWarehouseCode).HasMaxLength(20);
        builder.Property(x => x.SourceCellCode).HasMaxLength(50);
        builder.Property(x => x.TargetCellCode).HasMaxLength(50);
        builder.Property(x => x.ScannedBarcode).HasMaxLength(100);
        builder.HasIndex(x => x.OrderId).HasDatabaseName("IX_PrOperationLine_OrderId");
        builder.HasOne(x => x.Operation).WithMany(x => x.Lines).HasForeignKey(x => x.OperationId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.Order).WithMany().HasForeignKey(x => x.OrderId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.OrderOutput).WithMany(x => x.OperationLines).HasForeignKey(x => x.OrderOutputId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.OrderConsumption).WithMany(x => x.OperationLines).HasForeignKey(x => x.OrderConsumptionId).OnDelete(DeleteBehavior.Restrict);
    }
}
