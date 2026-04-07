using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wms.Domain.Entities.SubcontractingReceiptTransfer;
using Wms.Infrastructure.Persistence.Configurations.Common;

namespace Wms.Infrastructure.Persistence.Configurations.SubcontractingReceiptTransfer;

public sealed class SrtHeaderConfiguration : BaseHeaderEntityConfiguration<SrtHeader>
{
    protected override void ConfigureEntity(EntityTypeBuilder<SrtHeader> builder)
    {
        builder.ToTable("RII_SRT_HEADER");
        builder.Property(x => x.CustomerCode).HasMaxLength(20);
        builder.Property(x => x.CustomerId).IsRequired(false);
        builder.Property(x => x.SourceWarehouse).HasMaxLength(20);
        builder.Property(x => x.SourceWarehouseId).IsRequired(false);
        builder.Property(x => x.TargetWarehouse).HasMaxLength(20);
        builder.Property(x => x.TargetWarehouseId).IsRequired(false);
        builder.HasIndex(x => x.CustomerId).HasDatabaseName("IX_SrtHeader_CustomerId");
        builder.HasIndex(x => x.SourceWarehouseId).HasDatabaseName("IX_SrtHeader_SourceWarehouseId");
        builder.HasIndex(x => x.TargetWarehouseId).HasDatabaseName("IX_SrtHeader_TargetWarehouseId");
        builder.HasMany(x => x.Lines).WithOne(x => x.Header).HasForeignKey(x => x.HeaderId).OnDelete(DeleteBehavior.Restrict);
        builder.HasMany(x => x.ImportLines).WithOne(x => x.Header).HasForeignKey(x => x.HeaderId).OnDelete(DeleteBehavior.Restrict);
    }
}
