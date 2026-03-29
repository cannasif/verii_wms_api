using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wms.Domain.Entities.Production;
using Wms.Infrastructure.Persistence.Configurations.Common;

namespace Wms.Infrastructure.Persistence.Configurations.Production;

public sealed class PrHeaderConfiguration : BaseHeaderEntityConfiguration<PrHeader>
{
    protected override void ConfigureEntity(EntityTypeBuilder<PrHeader> builder)
    {
        builder.ToTable("RII_PR_HEADER");
        builder.Property(x => x.CustomerCode).HasMaxLength(20);
        builder.Property(x => x.StockCode).HasMaxLength(35);
        builder.Property(x => x.YapKod).HasMaxLength(50);
        builder.Property(x => x.Quantity).HasColumnType("decimal(18,6)");
        builder.Property(x => x.SourceWarehouse).HasMaxLength(20);
        builder.Property(x => x.TargetWarehouse).HasMaxLength(20);
        builder.HasMany(x => x.Lines).WithOne(x => x.Header).HasForeignKey(x => x.HeaderId).OnDelete(DeleteBehavior.Restrict);
        builder.HasMany(x => x.ImportLines).WithOne(x => x.Header).HasForeignKey(x => x.HeaderId).OnDelete(DeleteBehavior.Restrict);
    }
}
