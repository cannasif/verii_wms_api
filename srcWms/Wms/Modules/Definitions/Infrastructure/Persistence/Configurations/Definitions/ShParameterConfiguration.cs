using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wms.Domain.Entities.Definitions;
using Wms.Infrastructure.Persistence.Configurations.Common;

namespace Wms.Infrastructure.Persistence.Configurations.Definitions;

public sealed class ShParameterConfiguration : BaseParameterConfiguration<ShParameter>
{
    protected override void ConfigureEntity(EntityTypeBuilder<ShParameter> builder)
    {
        builder.ToTable("RII_SH_PARAMETER");
    }
}
