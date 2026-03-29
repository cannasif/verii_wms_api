using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wms.Domain.Entities.InventoryCount;
using Wms.Infrastructure.Persistence.Configurations.Common;

namespace Wms.Infrastructure.Persistence.Configurations.InventoryCount;

public sealed class IcTerminalLineConfiguration : BaseEntityConfiguration<IcTerminalLine>
{
    protected override void ConfigureEntity(EntityTypeBuilder<IcTerminalLine> builder)
    {
        builder.ToTable("RII_IC_TERMINAL_LINE");
        builder.Property(x => x.HeaderId).IsRequired();
        builder.Property(x => x.TerminalUserId).IsRequired();
        builder.HasOne(x => x.Header).WithMany(x => x.TerminalLines).HasForeignKey(x => x.HeaderId).OnDelete(DeleteBehavior.Restrict);
        builder.HasIndex(x => x.HeaderId).HasDatabaseName("IX_IcTerminalLine_HeaderId");
        builder.HasIndex(x => x.TerminalUserId).HasDatabaseName("IX_IcTerminalLine_TerminalUserId");
    }
}
