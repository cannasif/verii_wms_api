using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wms.Domain.Entities.Production;
using Wms.Infrastructure.Persistence.Configurations.Common;

namespace Wms.Infrastructure.Persistence.Configurations.Production;

public sealed class PrOperationEventConfiguration : BaseEntityConfiguration<PrOperationEvent>
{
    protected override void ConfigureEntity(EntityTypeBuilder<PrOperationEvent> builder)
    {
        builder.ToTable("RII_PR_OPERATION_EVENT");
        builder.Property(x => x.EventType).HasMaxLength(30).IsRequired();
        builder.Property(x => x.EventReasonCode).HasMaxLength(50);
        builder.Property(x => x.EventNote).HasMaxLength(500);
        builder.HasIndex(x => new { x.OrderId, x.EventAt }).HasDatabaseName("IX_PrOperationEvent_OrderId_EventAt");
        builder.HasOne(x => x.Operation).WithMany(x => x.Events).HasForeignKey(x => x.OperationId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.Order).WithMany().HasForeignKey(x => x.OrderId).OnDelete(DeleteBehavior.Restrict);
    }
}
