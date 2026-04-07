using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wms.Domain.Entities.InventoryCount;
using Wms.Infrastructure.Persistence.Configurations.Common;

namespace Wms.Infrastructure.Persistence.Configurations.InventoryCount;

public sealed class IcScopeConfiguration : BaseEntityConfiguration<IcScope>
{
    protected override void ConfigureEntity(EntityTypeBuilder<IcScope> builder)
    {
        builder.ToTable("RII_IC_SCOPE");
        builder.Property(x => x.ScopeType).HasMaxLength(20).IsRequired();
        builder.Property(x => x.WarehouseCode).HasMaxLength(20);
        builder.Property(x => x.StockCode).HasMaxLength(50);
        builder.Property(x => x.YapKod).HasMaxLength(50);
        builder.Property(x => x.RackCode).HasMaxLength(35);
        builder.Property(x => x.CellCode).HasMaxLength(35);
        builder.HasIndex(x => x.HeaderId).HasDatabaseName("IX_IcScope_HeaderId");
        builder.HasIndex(x => x.ScopeType).HasDatabaseName("IX_IcScope_ScopeType");
        builder.HasIndex(x => x.WarehouseId).HasDatabaseName("IX_IcScope_WarehouseId");
        builder.HasIndex(x => x.StockId).HasDatabaseName("IX_IcScope_StockId");
        builder.HasOne(x => x.Header).WithMany(x => x.Scopes).HasForeignKey(x => x.HeaderId).OnDelete(DeleteBehavior.Restrict);
    }
}
