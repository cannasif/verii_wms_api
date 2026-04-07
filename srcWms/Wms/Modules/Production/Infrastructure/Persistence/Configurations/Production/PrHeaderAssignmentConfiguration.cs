using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wms.Domain.Entities.Production;
using Wms.Infrastructure.Persistence.Configurations.Common;

namespace Wms.Infrastructure.Persistence.Configurations.Production;

public sealed class PrHeaderAssignmentConfiguration : BaseEntityConfiguration<PrHeaderAssignment>
{
    protected override void ConfigureEntity(EntityTypeBuilder<PrHeaderAssignment> builder)
    {
        builder.ToTable("RII_PR_HEADER_ASSIGNMENT");
        builder.Property(x => x.AssignmentType).HasMaxLength(20).IsRequired();
        builder.Property(x => x.AssignedAt).IsRequired();
        builder.HasIndex(x => new { x.HeaderId, x.IsActive }).HasDatabaseName("IX_PrHeaderAssignment_HeaderId_IsActive");
        builder.HasOne(x => x.Header).WithMany(x => x.Assignments).HasForeignKey(x => x.HeaderId).OnDelete(DeleteBehavior.Restrict);
    }
}
