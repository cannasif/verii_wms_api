using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wms.Domain.Entities.Production;
using Wms.Infrastructure.Persistence.Configurations.Common;

namespace Wms.Infrastructure.Persistence.Configurations.Production;

public sealed class PrOrderConfiguration : BaseEntityConfiguration<PrOrder>
{
    protected override void ConfigureEntity(EntityTypeBuilder<PrOrder> builder)
    {
        builder.ToTable("RII_PR_ORDER");
        builder.Property(x => x.OrderNo).HasMaxLength(30).IsRequired();
        builder.Property(x => x.Description).HasMaxLength(500);
        builder.Property(x => x.OrderType).HasMaxLength(20).IsRequired();
        builder.Property(x => x.Status).HasMaxLength(20).IsRequired();
        builder.Property(x => x.ProducedStockCode).HasMaxLength(50).IsRequired();
        builder.Property(x => x.ProducedYapKod).HasMaxLength(50);
        builder.Property(x => x.PlannedQuantity).HasColumnType("decimal(18,6)").IsRequired();
        builder.Property(x => x.StartedQuantity).HasColumnType("decimal(18,6)");
        builder.Property(x => x.CompletedQuantity).HasColumnType("decimal(18,6)");
        builder.Property(x => x.ScrapQuantity).HasColumnType("decimal(18,6)");
        builder.Property(x => x.SourceWarehouseCode).HasMaxLength(20);
        builder.Property(x => x.TargetWarehouseCode).HasMaxLength(20);
        builder.Property(x => x.BlockedReason).HasMaxLength(250);
        builder.Property(x => x.StatusNote).HasMaxLength(500);
        builder.HasIndex(x => new { x.HeaderId, x.OrderNo }).HasDatabaseName("IX_PrOrder_HeaderId_OrderNo").IsUnique();
        builder.HasIndex(x => new { x.HeaderId, x.Status }).HasDatabaseName("IX_PrOrder_HeaderId_Status");
        builder.HasOne(x => x.Header).WithMany(x => x.Orders).HasForeignKey(x => x.HeaderId).OnDelete(DeleteBehavior.Restrict);
        builder.HasMany(x => x.Assignments).WithOne(x => x.Order).HasForeignKey(x => x.OrderId).OnDelete(DeleteBehavior.Restrict);
        builder.HasMany(x => x.Outputs).WithOne(x => x.Order).HasForeignKey(x => x.OrderId).OnDelete(DeleteBehavior.Restrict);
        builder.HasMany(x => x.Consumptions).WithOne(x => x.Order).HasForeignKey(x => x.OrderId).OnDelete(DeleteBehavior.Restrict);
        builder.HasMany(x => x.Operations).WithOne(x => x.Order).HasForeignKey(x => x.OrderId).OnDelete(DeleteBehavior.Restrict);
        builder.HasMany(x => x.DependenciesAsPredecessor).WithOne(x => x.PredecessorOrder).HasForeignKey(x => x.PredecessorOrderId).OnDelete(DeleteBehavior.Restrict);
        builder.HasMany(x => x.DependenciesAsSuccessor).WithOne(x => x.SuccessorOrder).HasForeignKey(x => x.SuccessorOrderId).OnDelete(DeleteBehavior.Restrict);
    }
}
