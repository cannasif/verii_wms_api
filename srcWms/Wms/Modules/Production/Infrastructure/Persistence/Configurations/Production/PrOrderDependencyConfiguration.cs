using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wms.Domain.Entities.Production;
using Wms.Infrastructure.Persistence.Configurations.Common;

namespace Wms.Infrastructure.Persistence.Configurations.Production;

public sealed class PrOrderDependencyConfiguration : BaseEntityConfiguration<PrOrderDependency>
{
    protected override void ConfigureEntity(EntityTypeBuilder<PrOrderDependency> builder)
    {
        builder.ToTable("RII_PR_ORDER_DEPENDENCY");
        builder.Property(x => x.DependencyType).HasMaxLength(20).IsRequired();
        builder.Property(x => x.ConditionNote).HasMaxLength(250);
        builder.HasIndex(x => new { x.PredecessorOrderId, x.SuccessorOrderId }).HasDatabaseName("IX_PrOrderDependency_Predecessor_Successor").IsUnique();
        builder.HasIndex(x => x.HeaderId).HasDatabaseName("IX_PrOrderDependency_HeaderId");
        builder.HasOne(x => x.Header).WithMany().HasForeignKey(x => x.HeaderId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.PredecessorOrder).WithMany(x => x.DependenciesAsPredecessor).HasForeignKey(x => x.PredecessorOrderId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.SuccessorOrder).WithMany(x => x.DependenciesAsSuccessor).HasForeignKey(x => x.SuccessorOrderId).OnDelete(DeleteBehavior.Restrict);
    }
}
