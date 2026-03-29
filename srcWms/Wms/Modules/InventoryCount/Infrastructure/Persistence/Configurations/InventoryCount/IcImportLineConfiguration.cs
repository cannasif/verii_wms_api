using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wms.Domain.Entities.InventoryCount;
using Wms.Infrastructure.Persistence.Configurations.Common;

namespace Wms.Infrastructure.Persistence.Configurations.InventoryCount;

public sealed class IcImportLineConfiguration : BaseImportLineEntityConfiguration<IcImportLine>
{
    protected override void ConfigureEntity(EntityTypeBuilder<IcImportLine> builder)
    {
        builder.ToTable("RII_IC_IMPORT_LINE");
        builder.Property(x => x.HeaderId).IsRequired();
        builder.Property(x => x.StockCode).HasMaxLength(35).IsRequired();
        builder.Property(x => x.YapKod).HasMaxLength(35);
        builder.Property(x => x.Description1).HasMaxLength(30);
        builder.Property(x => x.Description2).HasMaxLength(50);
        builder.Property(x => x.Description).HasMaxLength(255);
        builder.HasOne(x => x.Header).WithMany(x => x.ImportLines).HasForeignKey(x => x.HeaderId).OnDelete(DeleteBehavior.Restrict);
        builder.HasMany(x => x.Routes).WithOne(x => x.ImportLine).HasForeignKey(x => x.ImportLineId).OnDelete(DeleteBehavior.Restrict);
    }
}
