using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wms.Domain.Entities.WarehouseTransfer;
using Wms.Infrastructure.Persistence.Configurations.Common;

namespace Wms.Infrastructure.Persistence.Configurations.WarehouseTransfer;

public sealed class WtLineConfiguration : BaseLineEntityConfiguration<WtLine>
{
    protected override void ConfigureEntity(EntityTypeBuilder<WtLine> builder)
    {
        builder.ToTable("RII_WT_LINE");

        builder.Property(x => x.HeaderId).IsRequired();

        builder.HasIndex(x => x.HeaderId).HasDatabaseName("IX_WtLine_HeaderId");
        builder.HasIndex(x => x.StockCode).HasDatabaseName("IX_WtLine_StockCode");
        builder.HasIndex(x => x.ErpOrderNo).HasDatabaseName("IX_WtLine_ErpOrderNo");
        builder.HasIndex(x => x.IsDeleted).HasDatabaseName("IX_WtLine_IsDeleted");

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
