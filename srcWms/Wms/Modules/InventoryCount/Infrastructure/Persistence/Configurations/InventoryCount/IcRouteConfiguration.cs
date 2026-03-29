using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wms.Domain.Entities.InventoryCount;
using Wms.Infrastructure.Persistence.Configurations.Common;

namespace Wms.Infrastructure.Persistence.Configurations.InventoryCount;

public sealed class IcRouteConfiguration : BaseRouteEntityConfiguration<IcRoute>
{
    protected override void ConfigureEntity(EntityTypeBuilder<IcRoute> builder)
    {
        builder.ToTable("RII_IC_ROUTE");
        builder.Property(x => x.ImportLineId).IsRequired();
        builder.Property(x => x.Barcode).HasMaxLength(50);
        builder.Property(x => x.OldQuantity).HasColumnType("decimal(18,6)").IsRequired();
        builder.Property(x => x.NewQuantity).HasColumnType("decimal(18,6)").IsRequired();
        builder.Property(x => x.SourceCellCode).HasMaxLength(20);
        builder.Property(x => x.TargetCellCode).HasMaxLength(20);
        builder.Property(x => x.Description).HasMaxLength(100);
        builder.HasOne(x => x.ImportLine).WithMany(x => x.Routes).HasForeignKey(x => x.ImportLineId).OnDelete(DeleteBehavior.Restrict);
    }
}
