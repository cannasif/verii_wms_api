using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wms.Domain.Entities.Package;
using Wms.Domain.Entities.Package.Enums;
using Wms.Infrastructure.Persistence.Configurations.Common;

namespace Wms.Infrastructure.Persistence.Configurations.Package;

public sealed class PHeaderConfiguration : BaseEntityConfiguration<PHeader>
{
    protected override void ConfigureEntity(EntityTypeBuilder<PHeader> builder)
    {
        builder.ToTable("RII_P_HEADER");
        builder.Property(x => x.WarehouseCode).HasMaxLength(20);
        builder.Property(x => x.PackingNo).IsRequired().HasMaxLength(50);
        builder.Property(x => x.SourceType).HasMaxLength(30);
        builder.Property(x => x.CustomerCode).HasMaxLength(50);
        builder.Property(x => x.CustomerAddress).HasMaxLength(255);
        builder.Property(x => x.Status).IsRequired().HasMaxLength(20).HasDefaultValue(PHeaderStatus.Draft);
        builder.Property(x => x.TotalPackageCount).HasColumnType("decimal(18,6)");
        builder.Property(x => x.TotalQuantity).HasColumnType("decimal(18,6)");
        builder.Property(x => x.TotalNetWeight).HasColumnType("decimal(18,6)");
        builder.Property(x => x.TotalGrossWeight).HasColumnType("decimal(18,6)");
        builder.Property(x => x.TotalVolume).HasColumnType("decimal(18,6)");
        builder.Property(x => x.CarrierServiceType).HasMaxLength(20);
        builder.Property(x => x.TrackingNo).HasMaxLength(100);
        builder.Property(x => x.IsMatched).IsRequired().HasDefaultValue(false);
        builder.HasIndex(x => x.PackingNo).IsUnique().HasDatabaseName("IX_PHeader_PackingNo");
        builder.HasIndex(x => x.Status).HasDatabaseName("IX_PHeader_Status");
        builder.HasIndex(x => x.WarehouseCode).HasDatabaseName("IX_PHeader_WarehouseCode");
        builder.HasIndex(x => x.SourceHeaderId).HasDatabaseName("IX_PHeader_SourceHeaderId");
        builder.HasIndex(x => x.CustomerCode).HasDatabaseName("IX_PHeader_CustomerCode");
        builder.HasIndex(x => x.IsDeleted).HasDatabaseName("IX_PHeader_IsDeleted");
    }
}
