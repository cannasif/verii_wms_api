using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wms.Domain.Entities.GoodsReceipt;
using Wms.Infrastructure.Persistence.Configurations.Common;

namespace Wms.Infrastructure.Persistence.Configurations.GoodsReceipt;

public sealed class GrImportDocumentConfiguration : BaseEntityConfiguration<GrImportDocument>
{
    protected override void ConfigureEntity(EntityTypeBuilder<GrImportDocument> builder)
    {
        builder.ToTable("RII_GR_IMPORT_DOCUMENT");
        builder.Property(x => x.HeaderId).IsRequired().HasColumnName("HeaderId");
        builder.Property(x => x.Base64).IsRequired().HasColumnName("Base64");
        builder.Property(x => x.ImageUrl).HasColumnName("ImageUrl");
        builder.Property(x => x.FileName).HasColumnName("FileName");
        builder.HasOne(x => x.Header).WithMany().HasForeignKey(x => x.HeaderId).OnDelete(DeleteBehavior.Restrict).HasConstraintName("FK_GrImportDocument_GrHeader");
        builder.HasIndex(x => x.HeaderId).HasDatabaseName("IX_GrImportDocument_HeaderId");
        builder.HasIndex(x => x.IsDeleted).HasDatabaseName("IX_GrImportDocument_IsDeleted");
        builder.HasQueryFilter(x => !x.IsDeleted);
    }
}
