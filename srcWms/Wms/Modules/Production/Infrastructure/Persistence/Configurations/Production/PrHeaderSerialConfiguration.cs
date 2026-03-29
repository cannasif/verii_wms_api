using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wms.Domain.Entities.Production;
using Wms.Infrastructure.Persistence.Configurations.Common;

namespace Wms.Infrastructure.Persistence.Configurations.Production;

public sealed class PrHeaderSerialConfiguration : BaseEntityConfiguration<PrHeaderSerial>
{
    protected override void ConfigureEntity(EntityTypeBuilder<PrHeaderSerial> builder)
    {
        builder.ToTable("RII_PR_HEADER_SERIAL");
        builder.Property(x => x.SerialNo).HasMaxLength(100);
        builder.Property(x => x.SerialNo2).HasMaxLength(100);
        builder.Property(x => x.SerialNo3).HasMaxLength(100);
        builder.Property(x => x.SerialNo4).HasMaxLength(100);
        builder.Property(x => x.Amount).HasColumnType("decimal(18,6)");
        builder.HasOne(x => x.Header).WithMany().HasForeignKey(x => x.HeaderId).OnDelete(DeleteBehavior.Cascade);
        builder.HasQueryFilter(x => !x.IsDeleted);
    }
}
