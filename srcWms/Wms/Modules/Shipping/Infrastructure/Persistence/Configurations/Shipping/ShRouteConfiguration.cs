using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wms.Domain.Entities.Shipping;
using Wms.Infrastructure.Persistence.Configurations.Common;

namespace Wms.Infrastructure.Persistence.Configurations.Shipping;

public sealed class ShRouteConfiguration : BaseRouteEntityConfiguration<ShRoute>
{
    protected override void ConfigureEntity(EntityTypeBuilder<ShRoute> builder)
    {
        builder.ToTable("RII_SH_ROUTE");

        builder.Property(x => x.ImportLineId).IsRequired();

        builder.HasIndex(x => x.ImportLineId).HasDatabaseName("IX_ShRoute_ImportLineId");
        builder.HasIndex(x => x.SerialNo).HasDatabaseName("IX_ShRoute_SerialNo");
        builder.HasIndex(x => x.SourceWarehouse).HasDatabaseName("IX_ShRoute_SourceWarehouse");
        builder.HasIndex(x => x.TargetWarehouse).HasDatabaseName("IX_ShRoute_TargetWarehouse");
        builder.HasIndex(x => x.IsDeleted).HasDatabaseName("IX_ShRoute_IsDeleted");

        builder.HasOne(x => x.ImportLine)
            .WithMany(x => x.Routes)
            .HasForeignKey(x => x.ImportLineId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
