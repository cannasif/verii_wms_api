using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wms.Domain.Entities.Package;
using Wms.Infrastructure.Persistence.Configurations.Common;

namespace Wms.Infrastructure.Persistence.Configurations.Package;

public sealed class PLineConfiguration : BaseEntityConfiguration<PLine>
{
    protected override void ConfigureEntity(EntityTypeBuilder<PLine> builder)
    {
        builder.ToTable("RII_P_LINE");
        builder.Property(x => x.PackingHeaderId).IsRequired();
        builder.HasOne(x => x.PackingHeader).WithMany().HasForeignKey(x => x.PackingHeaderId).OnDelete(DeleteBehavior.Restrict).HasConstraintName("FK_PLine_PHeader");
        builder.Property(x => x.PackageId).IsRequired();
        builder.HasOne(x => x.Package).WithMany(x => x.Lines).HasForeignKey(x => x.PackageId).OnDelete(DeleteBehavior.Restrict).HasConstraintName("FK_PLine_PPackage");
        builder.Property(x => x.Barcode).HasMaxLength(50);
        builder.Property(x => x.StockCode).IsRequired().HasMaxLength(50);
        builder.Property(x => x.YapKod).HasMaxLength(50);
        builder.Property(x => x.Quantity).IsRequired().HasColumnType("decimal(18,6)");
        builder.Property(x => x.SerialNo).HasMaxLength(50);
        builder.Property(x => x.SerialNo2).HasMaxLength(50);
        builder.Property(x => x.SerialNo3).HasMaxLength(50);
        builder.Property(x => x.SerialNo4).HasMaxLength(50);
        builder.HasIndex(x => x.PackingHeaderId).HasDatabaseName("IX_PLine_PackingHeaderId");
        builder.HasIndex(x => x.PackageId).HasDatabaseName("IX_PLine_PackageId");
        builder.HasIndex(x => x.StockCode).HasDatabaseName("IX_PLine_StockCode");
        builder.HasIndex(x => x.IsDeleted).HasDatabaseName("IX_PLine_IsDeleted");
    }
}
