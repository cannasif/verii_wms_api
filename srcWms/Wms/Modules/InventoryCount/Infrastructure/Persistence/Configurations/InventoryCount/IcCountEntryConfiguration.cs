using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wms.Domain.Entities.InventoryCount;
using Wms.Infrastructure.Persistence.Configurations.Common;

namespace Wms.Infrastructure.Persistence.Configurations.InventoryCount;

public sealed class IcCountEntryConfiguration : BaseEntityConfiguration<IcCountEntry>
{
    protected override void ConfigureEntity(EntityTypeBuilder<IcCountEntry> builder)
    {
        builder.ToTable("RII_IC_COUNT_ENTRY");
        builder.Property(x => x.EntryType).HasMaxLength(20).IsRequired();
        builder.Property(x => x.EnteredQuantity).HasColumnType("decimal(18,6)").IsRequired();
        builder.Property(x => x.WarehouseCode).HasMaxLength(20);
        builder.Property(x => x.RackCode).HasMaxLength(35);
        builder.Property(x => x.CellCode).HasMaxLength(35);
        builder.Property(x => x.DeviceCode).HasMaxLength(50);
        builder.Property(x => x.Note).HasMaxLength(250);
        builder.HasIndex(x => x.HeaderId).HasDatabaseName("IX_IcCountEntry_HeaderId");
        builder.HasIndex(x => x.LineId).HasDatabaseName("IX_IcCountEntry_LineId");
        builder.HasIndex(x => x.EntryType).HasDatabaseName("IX_IcCountEntry_EntryType");
        builder.HasOne(x => x.Header).WithMany(x => x.CountEntries).HasForeignKey(x => x.HeaderId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.Line).WithMany(x => x.CountEntries).HasForeignKey(x => x.LineId).OnDelete(DeleteBehavior.Restrict);
    }
}
