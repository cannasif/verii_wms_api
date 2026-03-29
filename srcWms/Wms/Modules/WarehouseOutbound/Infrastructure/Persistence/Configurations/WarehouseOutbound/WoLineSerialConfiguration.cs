using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wms.Domain.Entities.WarehouseOutbound;
using Wms.Infrastructure.Persistence.Configurations.Common;

namespace Wms.Infrastructure.Persistence.Configurations.WarehouseOutbound;

public sealed class WoLineSerialConfiguration : BaseLineSerialEntityConfiguration<WoLineSerial>
{
    protected override void ConfigureEntity(EntityTypeBuilder<WoLineSerial> builder)
    {
        builder.ToTable("RII_WO_LINE_SERIAL");

        builder.Property(x => x.LineId).IsRequired();

        builder.HasIndex(x => x.LineId).HasDatabaseName("IX_WoLineSerial_LineId");
        builder.HasIndex(x => x.IsDeleted).HasDatabaseName("IX_WoLineSerial_IsDeleted");

        builder.HasOne(x => x.Line)
            .WithMany(x => x.SerialLines)
            .HasForeignKey(x => x.LineId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
