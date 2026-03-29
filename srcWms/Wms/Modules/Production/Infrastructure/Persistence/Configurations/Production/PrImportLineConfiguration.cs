using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wms.Domain.Entities.Production;
using Wms.Infrastructure.Persistence.Configurations.Common;

namespace Wms.Infrastructure.Persistence.Configurations.Production;

public sealed class PrImportLineConfiguration : BaseImportLineEntityConfiguration<PrImportLine>
{
    protected override void ConfigureEntity(EntityTypeBuilder<PrImportLine> builder)
    {
        builder.ToTable("RII_PR_IMPORT_LINE");
        builder.Property(x => x.HeaderId).IsRequired();
        builder.Property(x => x.LineId);
        builder.HasOne(x => x.Header).WithMany(x => x.ImportLines).HasForeignKey(x => x.HeaderId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.Line).WithMany(x => x.ImportLines).HasForeignKey(x => x.LineId).OnDelete(DeleteBehavior.Restrict);
    }
}
