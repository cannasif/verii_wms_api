using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wms.Domain.Entities.ServiceAllocation;
using Wms.Infrastructure.Persistence.Configurations.Common;

namespace Wms.Infrastructure.Persistence.Configurations.ServiceAllocation;

public sealed class BusinessDocumentLinkConfiguration : BaseEntityConfiguration<BusinessDocumentLink>
{
    protected override void ConfigureEntity(EntityTypeBuilder<BusinessDocumentLink> builder)
    {
        builder.ToTable("RII_SA_DOCUMENT_LINK");

        builder.Property(x => x.BusinessEntityType).HasConversion<int>().IsRequired();
        builder.Property(x => x.BusinessEntityId).IsRequired();
        builder.Property(x => x.DocumentModule).HasConversion<int>().IsRequired();
        builder.Property(x => x.DocumentHeaderId).IsRequired();
        builder.Property(x => x.LinkPurpose).HasConversion<int>().IsRequired();
        builder.Property(x => x.Note).HasMaxLength(250);
        builder.Property(x => x.LinkedAt).IsRequired();

        builder.HasIndex(x => new { x.BusinessEntityType, x.BusinessEntityId }).HasDatabaseName("IX_DocumentLink_BusinessEntity");
        builder.HasIndex(x => new { x.DocumentModule, x.DocumentHeaderId }).HasDatabaseName("IX_DocumentLink_DocumentHeader");
        builder.HasIndex(x => x.ServiceCaseId).HasDatabaseName("IX_DocumentLink_ServiceCaseId");
        builder.HasIndex(x => x.OrderAllocationLineId).HasDatabaseName("IX_DocumentLink_OrderAllocationLineId");
        builder.HasIndex(x => x.IsDeleted).HasDatabaseName("IX_DocumentLink_IsDeleted");

        builder.HasOne(x => x.ServiceCase)
            .WithMany(x => x.DocumentLinks)
            .HasForeignKey(x => x.ServiceCaseId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.OrderAllocationLine)
            .WithMany(x => x.DocumentLinks)
            .HasForeignKey(x => x.OrderAllocationLineId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
