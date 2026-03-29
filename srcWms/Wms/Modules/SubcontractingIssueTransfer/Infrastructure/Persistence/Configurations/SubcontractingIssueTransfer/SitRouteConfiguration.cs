using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wms.Domain.Entities.SubcontractingIssueTransfer;
using Wms.Infrastructure.Persistence.Configurations.Common;

namespace Wms.Infrastructure.Persistence.Configurations.SubcontractingIssueTransfer;

public sealed class SitRouteConfiguration : BaseRouteEntityConfiguration<SitRoute>
{
    protected override void ConfigureEntity(EntityTypeBuilder<SitRoute> builder)
    {
        builder.ToTable("RII_SIT_ROUTE");
        builder.Property(x => x.ImportLineId).IsRequired();
        builder.HasOne(x => x.ImportLine).WithMany(x => x.Routes).HasForeignKey(x => x.ImportLineId).OnDelete(DeleteBehavior.Restrict);
    }
}
