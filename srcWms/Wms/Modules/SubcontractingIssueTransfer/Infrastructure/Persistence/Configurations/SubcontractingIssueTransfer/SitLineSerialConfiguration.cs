using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wms.Domain.Entities.SubcontractingIssueTransfer;
using Wms.Infrastructure.Persistence.Configurations.Common;

namespace Wms.Infrastructure.Persistence.Configurations.SubcontractingIssueTransfer;

public sealed class SitLineSerialConfiguration : BaseLineSerialEntityConfiguration<SitLineSerial>
{
    protected override void ConfigureEntity(EntityTypeBuilder<SitLineSerial> builder)
    {
        builder.ToTable("RII_SIT_LINE_SERIAL");
        builder.Property(x => x.LineId).IsRequired();
        builder.HasOne(x => x.Line).WithMany(x => x.SerialLines).HasForeignKey(x => x.LineId).OnDelete(DeleteBehavior.Restrict);
    }
}
