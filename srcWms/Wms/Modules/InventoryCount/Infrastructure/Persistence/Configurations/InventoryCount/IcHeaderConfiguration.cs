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
        builder.Property(x => x.CountType).HasMaxLength(20).IsRequired();
        builder.Property(x => x.ScopeMode).HasMaxLength(20).IsRequired();
        builder.Property(x => x.CountMode).HasMaxLength(20).IsRequired();
        builder.Property(x => x.Status).HasMaxLength(20).IsRequired();
        builder.Property(x => x.FreezeMode).HasMaxLength(20).IsRequired();
        builder.Property(x => x.WarehouseCode).HasMaxLength(20);
        builder.Property(x => x.WarehouseId).IsRequired(false);
        builder.Property(x => x.StockCode).HasMaxLength(50);
        builder.Property(x => x.YapKod).HasMaxLength(50);
        builder.Property(x => x.RackCode).HasMaxLength(35);
        builder.Property(x => x.CellCode).HasMaxLength(35);

        builder.HasIndex(x => x.WarehouseId).HasDatabaseName("IX_IcHeader_WarehouseId");
        builder.HasIndex(x => x.StockId).HasDatabaseName("IX_IcHeader_StockId");
        builder.HasIndex(x => x.AssignedUserId).HasDatabaseName("IX_IcHeader_AssignedUserId");
        builder.HasIndex(x => x.Status).HasDatabaseName("IX_IcHeader_Status");
        builder.HasIndex(x => x.CountType).HasDatabaseName("IX_IcHeader_CountType");

        builder.HasMany(x => x.ImportLines).WithOne(x => x.Header).HasForeignKey(x => x.HeaderId).OnDelete(DeleteBehavior.Restrict);
        builder.HasMany(x => x.TerminalLines).WithOne(x => x.Header).HasForeignKey(x => x.HeaderId).OnDelete(DeleteBehavior.Restrict);
        builder.HasMany(x => x.Scopes).WithOne(x => x.Header).HasForeignKey(x => x.HeaderId).OnDelete(DeleteBehavior.Restrict);
        builder.HasMany(x => x.Lines).WithOne(x => x.Header).HasForeignKey(x => x.HeaderId).OnDelete(DeleteBehavior.Restrict);
        builder.HasMany(x => x.CountEntries).WithOne(x => x.Header).HasForeignKey(x => x.HeaderId).OnDelete(DeleteBehavior.Restrict);
        builder.HasMany(x => x.Adjustments).WithOne(x => x.Header).HasForeignKey(x => x.HeaderId).OnDelete(DeleteBehavior.Restrict);
    }
}
