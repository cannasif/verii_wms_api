using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wms.Domain.Entities.Production;
using Wms.Infrastructure.Persistence.Configurations.Common;

namespace Wms.Infrastructure.Persistence.Configurations.Production;

public sealed class PrOrderAssignmentConfiguration : BaseEntityConfiguration<PrOrderAssignment>
{
    protected override void ConfigureEntity(EntityTypeBuilder<PrOrderAssignment> builder)
    {
        builder.ToTable("RII_PR_ORDER_ASSIGNMENT");
        builder.Property(x => x.AssignmentType).HasMaxLength(20).IsRequired();
        builder.Property(x => x.Note).HasMaxLength(250);
        builder.HasIndex(x => new { x.OrderId, x.IsActive }).HasDatabaseName("IX_PrOrderAssignment_OrderId_IsActive");
        builder.HasOne(x => x.Order).WithMany(x => x.Assignments).HasForeignKey(x => x.OrderId).OnDelete(DeleteBehavior.Restrict);
    }
}
