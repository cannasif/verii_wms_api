using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wms.Domain.Entities.ProductionTransfer;
using Wms.Infrastructure.Persistence.Configurations.Common;

namespace Wms.Infrastructure.Persistence.Configurations.ProductionTransfer;

public sealed class PtTerminalLineConfiguration : BaseEntityConfiguration<PtTerminalLine>
{
    protected override void ConfigureEntity(EntityTypeBuilder<PtTerminalLine> builder)
    {
        builder.ToTable("RII_PT_TERMINAL_LINE");
        builder.Property(x => x.HeaderId).IsRequired();
        builder.Property(x => x.TerminalUserId).IsRequired();
        builder.HasOne(x => x.Header).WithMany().HasForeignKey(x => x.HeaderId).OnDelete(DeleteBehavior.Restrict);
        builder.HasIndex(x => x.HeaderId).HasDatabaseName("IX_PtTerminalLine_HeaderId");
        builder.HasIndex(x => x.TerminalUserId).HasDatabaseName("IX_PtTerminalLine_TerminalUserId");
    }
}
