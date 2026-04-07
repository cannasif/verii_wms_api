using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wms.Domain.Entities.SubcontractingIssueTransfer;
using Wms.Infrastructure.Persistence.Configurations.Common;

namespace Wms.Infrastructure.Persistence.Configurations.SubcontractingIssueTransfer;

public sealed class SitHeaderConfiguration : BaseHeaderEntityConfiguration<SitHeader>
{
    protected override void ConfigureEntity(EntityTypeBuilder<SitHeader> builder)
    {
        builder.ToTable("RII_SIT_HEADER");
        builder.Property(x => x.CustomerCode).HasMaxLength(20);
        builder.Property(x => x.CustomerId).IsRequired(false);
        builder.Property(x => x.SourceWarehouse).HasMaxLength(20);
        builder.Property(x => x.SourceWarehouseId).IsRequired(false);
        builder.Property(x => x.TargetWarehouse).HasMaxLength(20);
        builder.Property(x => x.TargetWarehouseId).IsRequired(false);
        builder.HasIndex(x => x.CustomerId).HasDatabaseName("IX_SitHeader_CustomerId");
        builder.HasIndex(x => x.SourceWarehouseId).HasDatabaseName("IX_SitHeader_SourceWarehouseId");
        builder.HasIndex(x => x.TargetWarehouseId).HasDatabaseName("IX_SitHeader_TargetWarehouseId");
        builder.HasMany(x => x.Lines).WithOne(x => x.Header).HasForeignKey(x => x.HeaderId).OnDelete(DeleteBehavior.Restrict);
        builder.HasMany(x => x.ImportLines).WithOne(x => x.Header).HasForeignKey(x => x.HeaderId).OnDelete(DeleteBehavior.Restrict);
    }
}
