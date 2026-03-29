using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wms.Domain.Entities.Shipping;
using Wms.Infrastructure.Persistence.Configurations.Common;

namespace Wms.Infrastructure.Persistence.Configurations.Shipping;

public sealed class ShLineSerialConfiguration : BaseLineSerialEntityConfiguration<ShLineSerial>
{
    protected override void ConfigureEntity(EntityTypeBuilder<ShLineSerial> builder)
    {
        builder.ToTable("RII_SH_LINE_SERIAL");

        builder.Property(x => x.LineId).IsRequired();

        builder.HasIndex(x => x.LineId).HasDatabaseName("IX_ShLineSerial_LineId");
        builder.HasIndex(x => x.IsDeleted).HasDatabaseName("IX_ShLineSerial_IsDeleted");

        builder.HasOne(x => x.Line)
            .WithMany(x => x.SerialLines)
            .HasForeignKey(x => x.LineId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
