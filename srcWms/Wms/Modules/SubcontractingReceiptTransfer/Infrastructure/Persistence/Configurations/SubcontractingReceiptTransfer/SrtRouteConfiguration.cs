using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wms.Domain.Entities.SubcontractingReceiptTransfer;
using Wms.Infrastructure.Persistence.Configurations.Common;

namespace Wms.Infrastructure.Persistence.Configurations.SubcontractingReceiptTransfer;

public sealed class SrtRouteConfiguration : BaseRouteEntityConfiguration<SrtRoute>
{
    protected override void ConfigureEntity(EntityTypeBuilder<SrtRoute> builder)
    {
        builder.ToTable("RII_SRT_ROUTE");
        builder.Property(x => x.ImportLineId).IsRequired();
        builder.HasOne(x => x.ImportLine).WithMany(x => x.Routes).HasForeignKey(x => x.ImportLineId).OnDelete(DeleteBehavior.Restrict);
    }
}
