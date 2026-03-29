using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wms.Domain.Entities.Definitions;

namespace Wms.Infrastructure.Persistence.Configurations.Common;

/// <summary>
/// `_old/reference/.../BaseParameterConfiguration.cs` ortak parameter kolonlarını taşır.
/// </summary>
public abstract class BaseParameterConfiguration<TEntity> : BaseEntityConfiguration<TEntity>
    where TEntity : BaseParameter
{
    public override void Configure(EntityTypeBuilder<TEntity> builder)
    {
        base.Configure(builder);

        builder.Property(p => p.AllowLessQuantityBasedOnOrder)
            .IsRequired()
            .HasDefaultValue(false)
            .HasColumnName("AllowLessQuantityBasedOnOrder");

        builder.Property(p => p.AllowMoreQuantityBasedOnOrder)
            .IsRequired()
            .HasDefaultValue(false)
            .HasColumnName("AllowMoreQuantityBasedOnOrder");

        builder.Property(p => p.RequireApprovalBeforeErp)
            .IsRequired()
            .HasDefaultValue(false)
            .HasColumnName("RequireApprovalBeforeErp");

        builder.Property(p => p.RequireAllOrderItemsCollected)
            .IsRequired()
            .HasDefaultValue(false)
            .HasColumnName("RequireAllOrderItemsCollected");
    }
}
