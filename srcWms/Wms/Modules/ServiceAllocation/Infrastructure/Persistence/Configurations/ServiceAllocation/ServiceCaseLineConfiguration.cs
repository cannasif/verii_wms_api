using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wms.Domain.Entities.ServiceAllocation;
using Wms.Infrastructure.Persistence.Configurations.Common;

namespace Wms.Infrastructure.Persistence.Configurations.ServiceAllocation;

public sealed class ServiceCaseLineConfiguration : BaseEntityConfiguration<ServiceCaseLine>
{
    protected override void ConfigureEntity(EntityTypeBuilder<ServiceCaseLine> builder)
    {
        builder.ToTable("RII_SA_SERVICE_CASE_LINE");

        builder.Property(x => x.ServiceCaseId).IsRequired();
        builder.Property(x => x.LineType).HasConversion<int>().IsRequired();
        builder.Property(x => x.ProcessType).HasConversion<int>().IsRequired();
        builder.Property(x => x.StockCode).HasMaxLength(35);
        builder.Property(x => x.Quantity).HasColumnType("decimal(18,6)").IsRequired();
        builder.Property(x => x.Unit).HasMaxLength(10);
        builder.Property(x => x.ErpOrderNo).HasMaxLength(50);
        builder.Property(x => x.ErpOrderId).HasMaxLength(30);
        builder.Property(x => x.Description).HasMaxLength(250);

        builder.HasIndex(x => x.ServiceCaseId).HasDatabaseName("IX_ServiceCaseLine_ServiceCaseId");
        builder.HasIndex(x => x.ErpOrderId).HasDatabaseName("IX_ServiceCaseLine_ErpOrderId");
        builder.HasIndex(x => x.StockCode).HasDatabaseName("IX_ServiceCaseLine_StockCode");
        builder.HasIndex(x => x.IsDeleted).HasDatabaseName("IX_ServiceCaseLine_IsDeleted");
    }
}
