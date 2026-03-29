using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wms.Domain.Entities.ProductionTransfer;
using Wms.Infrastructure.Persistence.Configurations.Common;

namespace Wms.Infrastructure.Persistence.Configurations.ProductionTransfer;

public sealed class PtLineConfiguration : BaseLineEntityConfiguration<PtLine>
{
    protected override void ConfigureEntity(EntityTypeBuilder<PtLine> builder)
    {
        builder.ToTable("RII_PT_LINE");
        builder.Property(x => x.HeaderId).IsRequired();
        builder.HasOne(x => x.Header).WithMany(x => x.Lines).HasForeignKey(x => x.HeaderId).OnDelete(DeleteBehavior.Restrict);
        builder.HasMany(x => x.ImportLines).WithOne(x => x.Line).HasForeignKey(x => x.LineId).OnDelete(DeleteBehavior.Restrict);
    }
}
