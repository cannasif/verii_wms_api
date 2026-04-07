using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wms.Domain.Entities.Production;
using Wms.Infrastructure.Persistence.Configurations.Common;

namespace Wms.Infrastructure.Persistence.Configurations.Production;

public sealed class PrOperationConfiguration : BaseEntityConfiguration<PrOperation>
{
    protected override void ConfigureEntity(EntityTypeBuilder<PrOperation> builder)
    {
        builder.ToTable("RII_PR_OPERATION");
        builder.Property(x => x.OperationNo).HasMaxLength(30);
        builder.Property(x => x.OperationType).HasMaxLength(20).IsRequired();
        builder.Property(x => x.Status).HasMaxLength(20).IsRequired();
        builder.Property(x => x.Description).HasMaxLength(500);
        builder.HasIndex(x => new { x.OrderId, x.Status }).HasDatabaseName("IX_PrOperation_OrderId_Status");
        builder.HasOne(x => x.Order).WithMany(x => x.Operations).HasForeignKey(x => x.OrderId).OnDelete(DeleteBehavior.Restrict);
        builder.HasMany(x => x.Lines).WithOne(x => x.Operation).HasForeignKey(x => x.OperationId).OnDelete(DeleteBehavior.Restrict);
        builder.HasMany(x => x.Events).WithOne(x => x.Operation).HasForeignKey(x => x.OperationId).OnDelete(DeleteBehavior.Restrict);
    }
}
