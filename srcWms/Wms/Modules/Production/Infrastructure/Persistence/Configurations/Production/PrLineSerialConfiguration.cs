using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wms.Domain.Entities.Production;
using Wms.Infrastructure.Persistence.Configurations.Common;

namespace Wms.Infrastructure.Persistence.Configurations.Production;

public sealed class PrLineSerialConfiguration : BaseLineSerialEntityConfiguration<PrLineSerial>
{
    protected override void ConfigureEntity(EntityTypeBuilder<PrLineSerial> builder)
    {
        builder.ToTable("RII_PR_LINE_SERIAL");
        builder.Property(x => x.LineId).IsRequired();
        builder.HasOne(x => x.Line).WithMany(x => x.SerialLines).HasForeignKey(x => x.LineId).OnDelete(DeleteBehavior.Restrict);
    }
}
