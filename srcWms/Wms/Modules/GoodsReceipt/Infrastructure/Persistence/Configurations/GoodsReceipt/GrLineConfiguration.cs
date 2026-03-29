using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wms.Domain.Entities.GoodsReceipt;
using Wms.Infrastructure.Persistence.Configurations.Common;

namespace Wms.Infrastructure.Persistence.Configurations.GoodsReceipt;

public sealed class GrLineConfiguration : BaseLineEntityConfiguration<GrLine>
{
    protected override void ConfigureEntity(EntityTypeBuilder<GrLine> builder)
    {
        builder.ToTable("RII_GR_LINE");
        builder.Property(x => x.HeaderId).IsRequired().HasColumnName("HeaderId");
        builder.HasOne(x => x.Header).WithMany(x => x.Lines).HasForeignKey(x => x.HeaderId).OnDelete(DeleteBehavior.Restrict).HasConstraintName("FK_GrLine_GrHeader");
        builder.HasIndex(x => x.HeaderId).HasDatabaseName("IX_GrLine_HeaderId");
        builder.HasIndex(x => x.StockCode).HasDatabaseName("IX_GrLine_StockCode");
        builder.HasIndex(x => x.IsDeleted).HasDatabaseName("IX_GrLine_IsDeleted");
    }
}
