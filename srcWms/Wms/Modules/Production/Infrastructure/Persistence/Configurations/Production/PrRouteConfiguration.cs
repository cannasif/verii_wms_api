using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wms.Domain.Entities.Production;
using Wms.Infrastructure.Persistence.Configurations.Common;

namespace Wms.Infrastructure.Persistence.Configurations.Production;

public sealed class PrRouteConfiguration : BaseRouteEntityConfiguration<PrRoute>
{
    protected override void ConfigureEntity(EntityTypeBuilder<PrRoute> builder)
    {
        builder.ToTable("RII_PR_ROUTE");
        builder.Property(x => x.ImportLineId).IsRequired();
        builder.HasOne(x => x.ImportLine).WithMany(x => x.Routes).HasForeignKey(x => x.ImportLineId).OnDelete(DeleteBehavior.Restrict);
    }
}
