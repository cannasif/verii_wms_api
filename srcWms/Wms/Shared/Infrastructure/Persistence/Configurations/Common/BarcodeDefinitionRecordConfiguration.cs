using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wms.Domain.Entities.Common;

namespace Wms.Infrastructure.Persistence.Configurations.Common;

public sealed class BarcodeDefinitionRecordConfiguration : BaseEntityConfiguration<BarcodeDefinitionRecord>
{
    protected override void ConfigureEntity(EntityTypeBuilder<BarcodeDefinitionRecord> builder)
    {
        builder.ToTable("RII_BARCODE_DEFINITION");

        builder.Property(x => x.ModuleKey)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.DisplayName)
            .HasMaxLength(150)
            .IsRequired();

        builder.Property(x => x.DefinitionType)
            .HasMaxLength(50)
            .IsRequired()
            .HasDefaultValue("pattern");

        builder.Property(x => x.SegmentPattern)
            .HasMaxLength(250)
            .IsRequired(false);

        builder.Property(x => x.RequiredSegments)
            .HasMaxLength(250)
            .IsRequired(false);

        builder.Property(x => x.HintText)
            .HasMaxLength(250)
            .IsRequired(false);

        builder.Property(x => x.IsActive)
            .IsRequired()
            .HasDefaultValue(true);

        builder.Property(x => x.AllowMirrorLookup)
            .IsRequired()
            .HasDefaultValue(true);

        builder.HasIndex(x => new { x.BranchCode, x.ModuleKey })
            .IsUnique()
            .HasDatabaseName("UX_RII_BARCODE_DEFINITION_BRANCH_MODULE");
    }
}
