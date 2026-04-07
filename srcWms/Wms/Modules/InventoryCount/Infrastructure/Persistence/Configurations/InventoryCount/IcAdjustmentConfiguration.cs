using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wms.Domain.Entities.InventoryCount;
using Wms.Infrastructure.Persistence.Configurations.Common;

namespace Wms.Infrastructure.Persistence.Configurations.InventoryCount;

public sealed class IcAdjustmentConfiguration : BaseEntityConfiguration<IcAdjustment>
{
    protected override void ConfigureEntity(EntityTypeBuilder<IcAdjustment> builder)
    {
        builder.ToTable("RII_IC_ADJUSTMENT");
        builder.Property(x => x.ExpectedQuantity).HasColumnType("decimal(18,6)").IsRequired();
        builder.Property(x => x.ApprovedCountedQuantity).HasColumnType("decimal(18,6)").IsRequired();
        builder.Property(x => x.DifferenceQuantity).HasColumnType("decimal(18,6)").IsRequired();
        builder.Property(x => x.AdjustmentType).HasMaxLength(20).IsRequired();
        builder.Property(x => x.Status).HasMaxLength(20).IsRequired();
        builder.Property(x => x.ErpReferenceNo).HasMaxLength(50);
        builder.Property(x => x.Note).HasMaxLength(250);
        builder.HasIndex(x => x.HeaderId).HasDatabaseName("IX_IcAdjustment_HeaderId");
        builder.HasIndex(x => x.LineId).HasDatabaseName("IX_IcAdjustment_LineId");
        builder.HasIndex(x => x.Status).HasDatabaseName("IX_IcAdjustment_Status");
        builder.HasOne(x => x.Header).WithMany(x => x.Adjustments).HasForeignKey(x => x.HeaderId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.Line).WithMany(x => x.Adjustments).HasForeignKey(x => x.LineId).OnDelete(DeleteBehavior.Restrict);
    }
}
