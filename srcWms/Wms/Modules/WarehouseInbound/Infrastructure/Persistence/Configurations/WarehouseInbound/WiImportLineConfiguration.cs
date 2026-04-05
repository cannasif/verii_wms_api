using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wms.Domain.Entities.WarehouseInbound;
using Wms.Infrastructure.Persistence.Configurations.Common;

namespace Wms.Infrastructure.Persistence.Configurations.WarehouseInbound;

public sealed class WiImportLineConfiguration : BaseImportLineEntityConfiguration<WiImportLine>
{
    protected override void ConfigureEntity(EntityTypeBuilder<WiImportLine> builder)
    {
        builder.ToTable("RII_WI_IMPORT_LINE");

        builder.Property(x => x.HeaderId).IsRequired();
        builder.Property(x => x.LineId);

        builder.HasIndex(x => x.HeaderId).HasDatabaseName("IX_WiImportLine_HeaderId");
        builder.HasIndex(x => x.LineId).HasDatabaseName("IX_WiImportLine_LineId");
        builder.HasIndex(x => x.StockCode).HasDatabaseName("IX_WiImportLine_StockCode");
        builder.HasIndex(x => x.StockId).HasDatabaseName("IX_WiImportLine_StockId");
        builder.HasIndex(x => x.IsDeleted).HasDatabaseName("IX_WiImportLine_IsDeleted");

        builder.HasOne(x => x.Header)
            .WithMany(x => x.ImportLines)
            .HasForeignKey(x => x.HeaderId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Line)
            .WithMany(x => x.ImportLines)
            .HasForeignKey(x => x.LineId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
