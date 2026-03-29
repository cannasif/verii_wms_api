using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wms.Domain.Entities.ProductionTransfer;
using Wms.Infrastructure.Persistence.Configurations.Common;

namespace Wms.Infrastructure.Persistence.Configurations.ProductionTransfer;

public sealed class PtHeaderConfiguration : BaseHeaderEntityConfiguration<PtHeader>
{
    protected override void ConfigureEntity(EntityTypeBuilder<PtHeader> builder)
    {
        builder.ToTable("RII_PT_HEADER");
        builder.Property(x => x.CustomerCode).HasMaxLength(20);
        builder.Property(x => x.SourceWarehouse).HasMaxLength(20);
        builder.Property(x => x.TargetWarehouse).HasMaxLength(20);
        builder.HasMany(x => x.Lines).WithOne(x => x.Header).HasForeignKey(x => x.HeaderId).OnDelete(DeleteBehavior.Restrict);
        builder.HasMany(x => x.ImportLines).WithOne(x => x.Header).HasForeignKey(x => x.HeaderId).OnDelete(DeleteBehavior.Restrict);
    }
}
