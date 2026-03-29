using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wms.Domain.Entities.SubcontractingReceiptTransfer;
using Wms.Infrastructure.Persistence.Configurations.Common;

namespace Wms.Infrastructure.Persistence.Configurations.SubcontractingReceiptTransfer;

public sealed class SrtTerminalLineConfiguration : BaseEntityConfiguration<SrtTerminalLine>
{
    protected override void ConfigureEntity(EntityTypeBuilder<SrtTerminalLine> builder)
    {
        builder.ToTable("RII_SRT_TERMINAL_LINE");
        builder.Property(x => x.HeaderId).IsRequired();
        builder.Property(x => x.TerminalUserId).IsRequired();
        builder.HasOne(x => x.Header).WithMany().HasForeignKey(x => x.HeaderId).OnDelete(DeleteBehavior.Restrict);
        builder.HasIndex(x => x.HeaderId).HasDatabaseName("IX_SrtTerminalLine_HeaderId");
        builder.HasIndex(x => x.TerminalUserId).HasDatabaseName("IX_SrtTerminalLine_TerminalUserId");
    }
}
