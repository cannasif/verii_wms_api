using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wms.Domain.Entities.SubcontractingIssueTransfer;
using Wms.Infrastructure.Persistence.Configurations.Common;

namespace Wms.Infrastructure.Persistence.Configurations.SubcontractingIssueTransfer;

public sealed class SitLineConfiguration : BaseLineEntityConfiguration<SitLine>
{
    protected override void ConfigureEntity(EntityTypeBuilder<SitLine> builder)
    {
        builder.ToTable("RII_SIT_LINE");
        builder.Property(x => x.HeaderId).IsRequired();
        builder.HasOne(x => x.Header).WithMany(x => x.Lines).HasForeignKey(x => x.HeaderId).OnDelete(DeleteBehavior.Restrict);
        builder.HasMany(x => x.ImportLines).WithOne(x => x.Line).HasForeignKey(x => x.LineId).OnDelete(DeleteBehavior.Restrict);
    }
}
