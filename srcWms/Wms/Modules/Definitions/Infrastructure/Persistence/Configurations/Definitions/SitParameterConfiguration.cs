using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wms.Domain.Entities.Definitions;
using Wms.Infrastructure.Persistence.Configurations.Common;

namespace Wms.Infrastructure.Persistence.Configurations.Definitions;

public sealed class SitParameterConfiguration : BaseParameterConfiguration<SitParameter>
{
    protected override void ConfigureEntity(EntityTypeBuilder<SitParameter> builder)
    {
        builder.ToTable("RII_SIT_PARAMETER");
    }
}
