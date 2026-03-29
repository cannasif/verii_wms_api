using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wms.Domain.Entities.Production;
using Wms.Infrastructure.Persistence.Configurations.Common;

namespace Wms.Infrastructure.Persistence.Configurations.Production;

public sealed class PrTerminalLineConfiguration : BaseEntityConfiguration<PrTerminalLine>
{
    protected override void ConfigureEntity(EntityTypeBuilder<PrTerminalLine> builder)
    {
        builder.ToTable("RII_PR_TERMINAL_LINE");
        builder.Property(x => x.HeaderId).IsRequired();
        builder.Property(x => x.TerminalUserId).IsRequired();
        builder.HasOne(x => x.Header).WithMany().HasForeignKey(x => x.HeaderId).OnDelete(DeleteBehavior.Restrict);
        builder.HasIndex(x => x.HeaderId).HasDatabaseName("IX_PrTerminalLine_HeaderId");
        builder.HasIndex(x => x.TerminalUserId).HasDatabaseName("IX_PrTerminalLine_TerminalUserId");
    }
}
