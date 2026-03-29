using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wms.Domain.Entities.SubcontractingIssueTransfer;
using Wms.Infrastructure.Persistence.Configurations.Common;

namespace Wms.Infrastructure.Persistence.Configurations.SubcontractingIssueTransfer;

public sealed class SitImportLineConfiguration : BaseImportLineEntityConfiguration<SitImportLine>
{
    protected override void ConfigureEntity(EntityTypeBuilder<SitImportLine> builder)
    {
        builder.ToTable("RII_SIT_IMPORT_LINE");
        builder.Property(x => x.HeaderId).IsRequired();
        builder.Property(x => x.LineId);
        builder.HasOne(x => x.Header).WithMany(x => x.ImportLines).HasForeignKey(x => x.HeaderId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.Line).WithMany(x => x.ImportLines).HasForeignKey(x => x.LineId).OnDelete(DeleteBehavior.Restrict);
    }
}
