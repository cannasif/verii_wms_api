using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wms.Domain.Entities.WarehouseTransfer;
using Wms.Infrastructure.Persistence.Configurations.Common;

namespace Wms.Infrastructure.Persistence.Configurations.WarehouseTransfer;

public sealed class WtTerminalLineConfiguration : BaseEntityConfiguration<WtTerminalLine>
{
    protected override void ConfigureEntity(EntityTypeBuilder<WtTerminalLine> builder)
    {
        builder.ToTable("RII_WT_TERMINAL_LINE");
        builder.Property(x => x.HeaderId).IsRequired();
        builder.Property(x => x.TerminalUserId).IsRequired();
        builder.HasOne(x => x.Header).WithMany().HasForeignKey(x => x.HeaderId).OnDelete(DeleteBehavior.Restrict);
        builder.HasIndex(x => x.HeaderId).HasDatabaseName("IX_WtTerminalLine_HeaderId");
        builder.HasIndex(x => x.TerminalUserId).HasDatabaseName("IX_WtTerminalLine_TerminalUserId");
    }
}
