using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wms.Domain.Entities.ProductionTransfer;
using Wms.Infrastructure.Persistence.Configurations.Common;

namespace Wms.Infrastructure.Persistence.Configurations.ProductionTransfer;

public sealed class PtImportLineConfiguration : BaseImportLineEntityConfiguration<PtImportLine>
{
    protected override void ConfigureEntity(EntityTypeBuilder<PtImportLine> builder)
    {
        builder.ToTable("RII_PT_IMPORT_LINE");
        builder.Property(x => x.HeaderId).IsRequired();
        builder.Property(x => x.LineId);
        builder.HasOne(x => x.Header).WithMany(x => x.ImportLines).HasForeignKey(x => x.HeaderId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.Line).WithMany(x => x.ImportLines).HasForeignKey(x => x.LineId).OnDelete(DeleteBehavior.Restrict);
    }
}
