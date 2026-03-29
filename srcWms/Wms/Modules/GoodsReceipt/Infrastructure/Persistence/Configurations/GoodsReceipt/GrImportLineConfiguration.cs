using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wms.Domain.Entities.GoodsReceipt;
using Wms.Infrastructure.Persistence.Configurations.Common;

namespace Wms.Infrastructure.Persistence.Configurations.GoodsReceipt;

public sealed class GrImportLineConfiguration : BaseImportLineEntityConfiguration<GrImportLine>
{
    protected override void ConfigureEntity(EntityTypeBuilder<GrImportLine> builder)
    {
        builder.ToTable("RII_GR_IMPORT_LINE");
        builder.Property(x => x.LineId).HasColumnName("LineId");
        builder.Property(x => x.HeaderId).IsRequired().HasColumnName("HeaderId");
        builder.HasOne(x => x.Line).WithMany(x => x.ImportLines).HasForeignKey(x => x.LineId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.Header).WithMany(x => x.ImportLines).HasForeignKey(x => x.HeaderId).OnDelete(DeleteBehavior.Restrict).IsRequired();
    }
}
