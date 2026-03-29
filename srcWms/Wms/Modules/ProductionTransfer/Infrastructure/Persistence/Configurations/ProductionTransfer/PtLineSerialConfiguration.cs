using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wms.Domain.Entities.ProductionTransfer;
using Wms.Infrastructure.Persistence.Configurations.Common;

namespace Wms.Infrastructure.Persistence.Configurations.ProductionTransfer;

public sealed class PtLineSerialConfiguration : BaseLineSerialEntityConfiguration<PtLineSerial>
{
    protected override void ConfigureEntity(EntityTypeBuilder<PtLineSerial> builder)
    {
        builder.ToTable("RII_PT_LINE_SERIAL");
        builder.Property(x => x.LineId).IsRequired();
        builder.HasOne(x => x.Line).WithMany(x => x.SerialLines).HasForeignKey(x => x.LineId).OnDelete(DeleteBehavior.Restrict);
    }
}
