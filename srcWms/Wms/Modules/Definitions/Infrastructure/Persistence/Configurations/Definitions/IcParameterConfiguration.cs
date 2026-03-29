using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wms.Domain.Entities.Definitions;
using Wms.Infrastructure.Persistence.Configurations.Common;

namespace Wms.Infrastructure.Persistence.Configurations.Definitions;

public sealed class IcParameterConfiguration : BaseParameterConfiguration<IcParameter>
{
    protected override void ConfigureEntity(EntityTypeBuilder<IcParameter> builder)
    {
        builder.ToTable("RII_IC_PARAMETER");
    }
}
