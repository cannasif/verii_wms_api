using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wms.Domain.Entities.GoodsReceipt;
using Wms.Infrastructure.Persistence.Configurations.Common;

namespace Wms.Infrastructure.Persistence.Configurations.GoodsReceipt;

public sealed class GrTerminalLineConfiguration : BaseEntityConfiguration<GrTerminalLine>
{
    protected override void ConfigureEntity(EntityTypeBuilder<GrTerminalLine> builder)
    {
        builder.ToTable("RII_GR_TERMINAL_LINE");
        builder.Property(x => x.HeaderId).IsRequired();
        builder.Property(x => x.TerminalUserId).IsRequired();
        builder.HasIndex(x => x.HeaderId).HasDatabaseName("IX_GrTerminalLine_HeaderId");
        builder.HasIndex(x => x.TerminalUserId).HasDatabaseName("IX_GrTerminalLine_TerminalUserId");
        builder.HasIndex(x => x.IsDeleted).HasDatabaseName("IX_GrTerminalLine_IsDeleted");
        builder.HasOne(x => x.Header).WithMany().HasForeignKey(x => x.HeaderId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.User).WithMany().HasForeignKey(x => x.TerminalUserId).OnDelete(DeleteBehavior.Restrict);
    }
}
