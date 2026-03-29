using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wms.Domain.Entities.SubcontractingReceiptTransfer;
using Wms.Infrastructure.Persistence.Configurations.Common;

namespace Wms.Infrastructure.Persistence.Configurations.SubcontractingReceiptTransfer;

public sealed class SrtLineSerialConfiguration : BaseLineSerialEntityConfiguration<SrtLineSerial>
{
    protected override void ConfigureEntity(EntityTypeBuilder<SrtLineSerial> builder)
    {
        builder.ToTable("RII_SRT_LINE_SERIAL");
        builder.Property(x => x.LineId).IsRequired();
        builder.HasOne(x => x.Line).WithMany(x => x.SerialLines).HasForeignKey(x => x.LineId).OnDelete(DeleteBehavior.Restrict);
    }
}
