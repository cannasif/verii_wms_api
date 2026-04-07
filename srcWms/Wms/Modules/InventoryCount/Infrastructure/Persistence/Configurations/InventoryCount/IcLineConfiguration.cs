using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wms.Domain.Entities.InventoryCount;
using Wms.Infrastructure.Persistence.Configurations.Common;

namespace Wms.Infrastructure.Persistence.Configurations.InventoryCount;

public sealed class IcLineConfiguration : BaseEntityConfiguration<IcLine>
{
    protected override void ConfigureEntity(EntityTypeBuilder<IcLine> builder)
    {
        builder.ToTable("RII_IC_LINE");
        builder.Property(x => x.WarehouseCode).HasMaxLength(20);
        builder.Property(x => x.RackCode).HasMaxLength(35);
        builder.Property(x => x.CellCode).HasMaxLength(35);
        builder.Property(x => x.StockCode).HasMaxLength(50).IsRequired();
        builder.Property(x => x.YapKod).HasMaxLength(50);
        builder.Property(x => x.LotNo).HasMaxLength(50);
        builder.Property(x => x.SerialNo1).HasMaxLength(100);
        builder.Property(x => x.SerialNo2).HasMaxLength(100);
        builder.Property(x => x.BatchNo).HasMaxLength(50);
        builder.Property(x => x.Unit).HasMaxLength(20);
        builder.Property(x => x.ExpectedQuantity).HasColumnType("decimal(18,6)").IsRequired();
        builder.Property(x => x.CountedQuantity).HasColumnType("decimal(18,6)");
        builder.Property(x => x.DifferenceQuantity).HasColumnType("decimal(18,6)");
        builder.Property(x => x.CountStatus).HasMaxLength(20).IsRequired();
        builder.Property(x => x.ApprovalNote).HasMaxLength(250);
        builder.Property(x => x.DifferenceReason).HasMaxLength(250);
        builder.HasIndex(x => x.HeaderId).HasDatabaseName("IX_IcLine_HeaderId");
        builder.HasIndex(x => x.ScopeId).HasDatabaseName("IX_IcLine_ScopeId");
        builder.HasIndex(x => x.StockId).HasDatabaseName("IX_IcLine_StockId");
        builder.HasIndex(x => x.CountStatus).HasDatabaseName("IX_IcLine_CountStatus");
        builder.HasOne(x => x.Header).WithMany(x => x.Lines).HasForeignKey(x => x.HeaderId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.Scope).WithMany(x => x.Lines).HasForeignKey(x => x.ScopeId).OnDelete(DeleteBehavior.Restrict);
    }
}
