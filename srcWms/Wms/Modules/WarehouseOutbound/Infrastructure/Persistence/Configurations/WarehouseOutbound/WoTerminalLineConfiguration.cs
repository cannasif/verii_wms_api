using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wms.Domain.Entities.WarehouseOutbound;
using Wms.Infrastructure.Persistence.Configurations.Common;

namespace Wms.Infrastructure.Persistence.Configurations.WarehouseOutbound;

public sealed class WoTerminalLineConfiguration : BaseEntityConfiguration<WoTerminalLine>
{
    protected override void ConfigureEntity(EntityTypeBuilder<WoTerminalLine> builder)
    {
        builder.ToTable("RII_WO_TERMINAL_LINE");

        builder.Property(x => x.HeaderId).IsRequired();
        builder.Property(x => x.TerminalUserId).IsRequired();

        builder.HasIndex(x => x.HeaderId).HasDatabaseName("IX_WoTerminalLine_HeaderId");
        builder.HasIndex(x => x.TerminalUserId).HasDatabaseName("IX_WoTerminalLine_TerminalUserId");
        builder.HasIndex(x => x.IsDeleted).HasDatabaseName("IX_WoTerminalLine_IsDeleted");

        builder.HasOne(x => x.Header)
            .WithMany()
            .HasForeignKey(x => x.HeaderId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
