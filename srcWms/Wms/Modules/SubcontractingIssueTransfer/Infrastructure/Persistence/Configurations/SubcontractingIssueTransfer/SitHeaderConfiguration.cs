using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wms.Domain.Entities.SubcontractingIssueTransfer;
using Wms.Infrastructure.Persistence.Configurations.Common;

namespace Wms.Infrastructure.Persistence.Configurations.SubcontractingIssueTransfer;

public sealed class SitHeaderConfiguration : BaseHeaderEntityConfiguration<SitHeader>
{
    protected override void ConfigureEntity(EntityTypeBuilder<SitHeader> builder)
    {
        builder.ToTable("RII_SIT_HEADER");
        builder.Property(x => x.CustomerCode).HasMaxLength(20);
        builder.Property(x => x.SourceWarehouse).HasMaxLength(20);
        builder.Property(x => x.TargetWarehouse).HasMaxLength(20);
        builder.Property(x => x.Type).IsRequired();
        builder.HasMany(x => x.Lines).WithOne(x => x.Header).HasForeignKey(x => x.HeaderId).OnDelete(DeleteBehavior.Restrict);
        builder.HasMany(x => x.ImportLines).WithOne(x => x.Header).HasForeignKey(x => x.HeaderId).OnDelete(DeleteBehavior.Restrict);
    }
}
