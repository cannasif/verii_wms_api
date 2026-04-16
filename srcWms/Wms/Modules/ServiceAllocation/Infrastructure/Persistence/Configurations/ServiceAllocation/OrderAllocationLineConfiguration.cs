using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wms.Domain.Entities.ServiceAllocation;
using Wms.Infrastructure.Persistence.Configurations.Common;

namespace Wms.Infrastructure.Persistence.Configurations.ServiceAllocation;

public sealed class OrderAllocationLineConfiguration : BaseEntityConfiguration<OrderAllocationLine>
{
    protected override void ConfigureEntity(EntityTypeBuilder<OrderAllocationLine> builder)
    {
        builder.ToTable("RII_SA_ALLOCATION_LINE");

        builder.Property(x => x.StockCode).HasMaxLength(35).IsRequired();
        builder.Property(x => x.StockId).IsRequired();
        builder.Property(x => x.ErpOrderNo).HasMaxLength(50).IsRequired();
        builder.Property(x => x.ErpOrderId).HasMaxLength(30).IsRequired();
        builder.Property(x => x.CustomerCode).HasMaxLength(20).IsRequired();
        builder.Property(x => x.ProcessType).HasConversion<int>().IsRequired();
        builder.Property(x => x.RequestedQuantity).HasColumnType("decimal(18,6)").IsRequired();
        builder.Property(x => x.AllocatedQuantity).HasColumnType("decimal(18,6)").IsRequired();
        builder.Property(x => x.ReservedQuantity).HasColumnType("decimal(18,6)").IsRequired();
        builder.Property(x => x.FulfilledQuantity).HasColumnType("decimal(18,6)").IsRequired();
        builder.Property(x => x.Status).HasConversion<int>().IsRequired();
        builder.Property(x => x.SourceModule).HasMaxLength(10);

        builder.HasIndex(x => x.StockCode).HasDatabaseName("IX_AllocationLine_StockCode");
        builder.HasIndex(x => x.StockId).HasDatabaseName("IX_AllocationLine_StockId");
        builder.HasIndex(x => x.ErpOrderId).HasDatabaseName("IX_AllocationLine_ErpOrderId");
        builder.HasIndex(x => new { x.StockCode, x.Status, x.PriorityNo }).HasDatabaseName("IX_AllocationLine_Queue");
        builder.HasIndex(x => x.SourceLineId).HasDatabaseName("IX_AllocationLine_SourceLineId");
        builder.HasIndex(x => x.IsDeleted).HasDatabaseName("IX_AllocationLine_IsDeleted");

        builder.HasMany(x => x.DocumentLinks)
            .WithOne(x => x.OrderAllocationLine)
            .HasForeignKey(x => x.OrderAllocationLineId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
