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
        builder.Property(x => x.CustomerId).IsRequired(false);
        builder.Property(x => x.StockCode).HasMaxLength(35);
        builder.Property(x => x.StockId).IsRequired(false);
        builder.Property(x => x.YapKod).HasMaxLength(50);
        builder.Property(x => x.YapKodId).IsRequired(false);
        builder.Property(x => x.Quantity).HasColumnType("decimal(18,6)");
        builder.Property(x => x.SourceWarehouse).HasMaxLength(20);
        builder.Property(x => x.SourceWarehouseId).IsRequired(false);
        builder.Property(x => x.TargetWarehouse).HasMaxLength(20);
        builder.Property(x => x.TargetWarehouseId).IsRequired(false);
        builder.HasIndex(x => x.CustomerId).HasDatabaseName("IX_PrHeader_CustomerId");
        builder.HasIndex(x => x.StockId).HasDatabaseName("IX_PrHeader_StockId");
        builder.HasIndex(x => x.YapKodId).HasDatabaseName("IX_PrHeader_YapKodId");
        builder.HasIndex(x => x.SourceWarehouseId).HasDatabaseName("IX_PrHeader_SourceWarehouseId");
        builder.HasIndex(x => x.TargetWarehouseId).HasDatabaseName("IX_PrHeader_TargetWarehouseId");
        builder.HasMany(x => x.Lines).WithOne(x => x.Header).HasForeignKey(x => x.HeaderId).OnDelete(DeleteBehavior.Restrict);
        builder.HasMany(x => x.ImportLines).WithOne(x => x.Header).HasForeignKey(x => x.HeaderId).OnDelete(DeleteBehavior.Restrict);
    }
}
