using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wms.Domain.Entities.InventoryCount;
using Wms.Infrastructure.Persistence.Configurations.Common;

namespace Wms.Infrastructure.Persistence.Configurations.InventoryCount;

public sealed class IcHeaderConfiguration : BaseHeaderEntityConfiguration<IcHeader>
{
    protected override void ConfigureEntity(EntityTypeBuilder<IcHeader> builder)
    {
        builder.ToTable("RII_IC_HEADER");
        builder.Property(x => x.CellCode).HasMaxLength(35);
        builder.Property(x => x.WarehouseCode).HasMaxLength(20);
        builder.Property(x => x.ProductCode).HasMaxLength(50);
        builder.Property(x => x.Type).IsRequired();
        builder.HasMany(x => x.ImportLines).WithOne(x => x.Header).HasForeignKey(x => x.HeaderId).OnDelete(DeleteBehavior.Restrict);
        builder.HasMany(x => x.TerminalLines).WithOne(x => x.Header).HasForeignKey(x => x.HeaderId).OnDelete(DeleteBehavior.Restrict);
    }
}
