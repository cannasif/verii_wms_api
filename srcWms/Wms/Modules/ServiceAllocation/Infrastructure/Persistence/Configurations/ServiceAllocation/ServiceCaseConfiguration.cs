using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wms.Domain.Entities.ServiceAllocation;
using Wms.Infrastructure.Persistence.Configurations.Common;

namespace Wms.Infrastructure.Persistence.Configurations.ServiceAllocation;

public sealed class ServiceCaseConfiguration : BaseEntityConfiguration<ServiceCase>
{
    protected override void ConfigureEntity(EntityTypeBuilder<ServiceCase> builder)
    {
        builder.ToTable("RII_SA_SERVICE_CASE");

        builder.Property(x => x.CaseNo).HasMaxLength(50).IsRequired();
        builder.Property(x => x.CustomerCode).HasMaxLength(20).IsRequired();
        builder.Property(x => x.IncomingStockCode).HasMaxLength(35);
        builder.Property(x => x.IncomingSerialNo).HasMaxLength(100);
        builder.Property(x => x.DiagnosisNote).HasMaxLength(1000);
        builder.Property(x => x.Status).HasConversion<int>().IsRequired();
        builder.Property(x => x.ReceivedAt).IsRequired(false);
        builder.Property(x => x.ClosedAt).IsRequired(false);

        builder.HasIndex(x => x.CaseNo).IsUnique().HasDatabaseName("UX_ServiceCase_CaseNo");
        builder.HasIndex(x => x.CustomerCode).HasDatabaseName("IX_ServiceCase_CustomerCode");
        builder.HasIndex(x => x.Status).HasDatabaseName("IX_ServiceCase_Status");
        builder.HasIndex(x => x.CurrentWarehouseId).HasDatabaseName("IX_ServiceCase_CurrentWarehouseId");
        builder.HasIndex(x => x.IsDeleted).HasDatabaseName("IX_ServiceCase_IsDeleted");

        builder.HasMany(x => x.Lines)
            .WithOne(x => x.ServiceCase)
            .HasForeignKey(x => x.ServiceCaseId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(x => x.DocumentLinks)
            .WithOne(x => x.ServiceCase)
            .HasForeignKey(x => x.ServiceCaseId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
