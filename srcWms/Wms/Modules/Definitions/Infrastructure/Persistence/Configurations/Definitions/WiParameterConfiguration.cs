using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wms.Domain.Entities.Definitions;
using Wms.Infrastructure.Persistence.Configurations.Common;

namespace Wms.Infrastructure.Persistence.Configurations.Definitions;

public sealed class WiParameterConfiguration : BaseParameterConfiguration<WiParameter>
{
    protected override void ConfigureEntity(EntityTypeBuilder<WiParameter> builder)
    {
        builder.ToTable("RII_WI_PARAMETER");
    }
}
