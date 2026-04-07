using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wms.Domain.Entities.ProductionTransfer;
using Wms.Infrastructure.Persistence.Configurations.Common;

namespace Wms.Infrastructure.Persistence.Configurations.ProductionTransfer;

public sealed class PtLineConfiguration : BaseLineEntityConfiguration<PtLine>
{
    protected override void ConfigureEntity(EntityTypeBuilder<PtLine> builder)
    {
        builder.ToTable("RII_PT_LINE");
        builder.Property(x => x.HeaderId).IsRequired();
        builder.Property(x => x.LineRole).HasMaxLength(20);
        builder.Property(x => x.RequiredQuantityFromProduction).HasColumnType("decimal(18,6)");
        builder.HasOne(x => x.Header).WithMany(x => x.Lines).HasForeignKey(x => x.HeaderId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.ProductionOrder).WithMany().HasForeignKey(x => x.ProductionOrderId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.ProductionOrderOutput).WithMany().HasForeignKey(x => x.ProductionOrderOutputId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.ProductionOrderConsumption).WithMany(x => x.ProductionTransferLines).HasForeignKey(x => x.ProductionOrderConsumptionId).OnDelete(DeleteBehavior.Restrict);
        builder.HasMany(x => x.ImportLines).WithOne(x => x.Line).HasForeignKey(x => x.LineId).OnDelete(DeleteBehavior.Restrict);
    }
}
