using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wms.Domain.Entities.ProductionTransfer;
using Wms.Infrastructure.Persistence.Configurations.Common;

namespace Wms.Infrastructure.Persistence.Configurations.ProductionTransfer;

public sealed class PtRouteConfiguration : BaseRouteEntityConfiguration<PtRoute>
{
    protected override void ConfigureEntity(EntityTypeBuilder<PtRoute> builder)
    {
        builder.ToTable("RII_PT_ROUTE");
        builder.Property(x => x.ImportLineId).IsRequired();
        builder.HasOne(x => x.ImportLine).WithMany(x => x.Routes).HasForeignKey(x => x.ImportLineId).OnDelete(DeleteBehavior.Restrict);
    }
}
