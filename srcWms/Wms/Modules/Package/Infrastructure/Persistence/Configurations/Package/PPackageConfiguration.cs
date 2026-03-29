using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wms.Domain.Entities.Package;
using Wms.Domain.Entities.Package.Enums;
using Wms.Infrastructure.Persistence.Configurations.Common;

namespace Wms.Infrastructure.Persistence.Configurations.Package;

public sealed class PPackageConfiguration : BaseEntityConfiguration<PPackage>
{
    protected override void ConfigureEntity(EntityTypeBuilder<PPackage> builder)
    {
        builder.ToTable("RII_P_PACKAGE");
        builder.Property(x => x.PackingHeaderId).IsRequired();
        builder.HasOne(x => x.PackingHeader).WithMany(x => x.Packages).HasForeignKey(x => x.PackingHeaderId).OnDelete(DeleteBehavior.Restrict).HasConstraintName("FK_PPackage_PHeader");
        builder.Property(x => x.PackageNo).IsRequired().HasMaxLength(50);
        builder.Property(x => x.PackageType).IsRequired().HasMaxLength(20).HasDefaultValue(PPackageType.Box);
        builder.Property(x => x.Barcode).HasMaxLength(100);
        builder.Property(x => x.Length).HasColumnType("decimal(18,6)");
        builder.Property(x => x.Width).HasColumnType("decimal(18,6)");
        builder.Property(x => x.Height).HasColumnType("decimal(18,6)");
        builder.Property(x => x.Volume).HasColumnType("decimal(18,6)");
        builder.Property(x => x.NetWeight).HasColumnType("decimal(18,6)");
        builder.Property(x => x.TareWeight).HasColumnType("decimal(18,6)");
        builder.Property(x => x.GrossWeight).HasColumnType("decimal(18,6)");
        builder.Property(x => x.IsMixed).IsRequired().HasDefaultValue(false);
        builder.Property(x => x.Status).IsRequired().HasMaxLength(20).HasDefaultValue(PPackageStatus.Open);
        builder.HasIndex(x => x.PackingHeaderId).HasDatabaseName("IX_PPackage_PackingHeaderId");
        builder.HasIndex(x => x.PackageNo).HasDatabaseName("IX_PPackage_PackageNo");
        builder.HasIndex(x => x.Barcode).IsUnique().HasFilter("[Barcode] IS NOT NULL").HasDatabaseName("IX_PPackage_Barcode");
        builder.HasIndex(x => x.Status).HasDatabaseName("IX_PPackage_Status");
        builder.HasIndex(x => x.IsDeleted).HasDatabaseName("IX_PPackage_IsDeleted");
    }
}
