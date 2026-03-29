using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wms.Domain.Entities.Definitions;
using Wms.Infrastructure.Persistence.Configurations.Common;

namespace Wms.Infrastructure.Persistence.Configurations.Definitions;

public sealed class WtParameterConfiguration : BaseParameterConfiguration<WtParameter>
{
    protected override void ConfigureEntity(EntityTypeBuilder<WtParameter> builder)
    {
        builder.ToTable("RII_WT_PARAMETER");
    }
}
