using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wms.Domain.Entities.Shipping;
using Wms.Infrastructure.Persistence.Configurations.Common;

namespace Wms.Infrastructure.Persistence.Configurations.Shipping;

public sealed class ShLineConfiguration : BaseLineEntityConfiguration<ShLine>
{
    protected override void ConfigureEntity(EntityTypeBuilder<ShLine> builder)
    {
        builder.ToTable("RII_SH_LINE");

        builder.Property(x => x.HeaderId).IsRequired();

        builder.HasIndex(x => x.HeaderId).HasDatabaseName("IX_ShLine_HeaderId");
        builder.HasIndex(x => x.StockCode).HasDatabaseName("IX_ShLine_StockCode");
        builder.HasIndex(x => x.StockId).HasDatabaseName("IX_ShLine_StockId");
        builder.HasIndex(x => x.ErpOrderNo).HasDatabaseName("IX_ShLine_ErpOrderNo");
        builder.HasIndex(x => x.IsDeleted).HasDatabaseName("IX_ShLine_IsDeleted");

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
