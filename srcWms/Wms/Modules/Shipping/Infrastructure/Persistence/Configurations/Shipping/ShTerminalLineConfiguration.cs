using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wms.Domain.Entities.Shipping;
using Wms.Infrastructure.Persistence.Configurations.Common;

namespace Wms.Infrastructure.Persistence.Configurations.Shipping;

public sealed class ShTerminalLineConfiguration : BaseEntityConfiguration<ShTerminalLine>
{
    protected override void ConfigureEntity(EntityTypeBuilder<ShTerminalLine> builder)
    {
        builder.ToTable("RII_SH_TERMINAL_LINE");

        builder.Property(x => x.HeaderId).IsRequired();
        builder.Property(x => x.TerminalUserId).IsRequired();

        builder.HasOne(x => x.Header)
            .WithMany()
            .HasForeignKey(x => x.HeaderId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => x.HeaderId).HasDatabaseName("IX_ShTerminalLine_HeaderId");
        builder.HasIndex(x => x.TerminalUserId).HasDatabaseName("IX_ShTerminalLine_TerminalUserId");
    }
}
