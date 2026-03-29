using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wms.Domain.Entities.WarehouseInbound;
using Wms.Infrastructure.Persistence.Configurations.Common;

namespace Wms.Infrastructure.Persistence.Configurations.WarehouseInbound;

public sealed class WiTerminalLineConfiguration : BaseEntityConfiguration<WiTerminalLine>
{
    protected override void ConfigureEntity(EntityTypeBuilder<WiTerminalLine> builder)
    {
        builder.ToTable("RII_WI_TERMINAL_LINE");

        builder.Property(x => x.HeaderId).IsRequired();
        builder.Property(x => x.TerminalUserId).IsRequired();

        builder.HasOne(x => x.Header)
            .WithMany()
            .HasForeignKey(x => x.HeaderId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => x.HeaderId).HasDatabaseName("IX_WiTerminalLine_HeaderId");
        builder.HasIndex(x => x.TerminalUserId).HasDatabaseName("IX_WiTerminalLine_TerminalUserId");
    }
}
