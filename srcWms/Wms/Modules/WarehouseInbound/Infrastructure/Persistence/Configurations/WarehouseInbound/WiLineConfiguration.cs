using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wms.Domain.Entities.WarehouseInbound;
using Wms.Infrastructure.Persistence.Configurations.Common;

namespace Wms.Infrastructure.Persistence.Configurations.WarehouseInbound;

public sealed class WiLineConfiguration : BaseLineEntityConfiguration<WiLine>
{
    protected override void ConfigureEntity(EntityTypeBuilder<WiLine> builder)
    {
        builder.ToTable("RII_WI_LINE");

        builder.Property(x => x.HeaderId).IsRequired();

        builder.HasIndex(x => x.HeaderId).HasDatabaseName("IX_WiLine_HeaderId");
        builder.HasIndex(x => x.StockCode).HasDatabaseName("IX_WiLine_StockCode");
        builder.HasIndex(x => x.ErpOrderNo).HasDatabaseName("IX_WiLine_ErpOrderNo");
        builder.HasIndex(x => x.IsDeleted).HasDatabaseName("IX_WiLine_IsDeleted");

        builder.HasOne(x => x.Header)
            .WithMany(x => x.Lines)
            .HasForeignKey(x => x.HeaderId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(x => x.ImportLines)
            .WithOne(x => x.Line)
            .HasForeignKey(x => x.LineId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
