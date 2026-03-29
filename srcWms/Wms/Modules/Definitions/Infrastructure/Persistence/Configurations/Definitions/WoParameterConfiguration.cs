using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wms.Domain.Entities.Definitions;
using Wms.Infrastructure.Persistence.Configurations.Common;

namespace Wms.Infrastructure.Persistence.Configurations.Definitions;

public sealed class WoParameterConfiguration : BaseParameterConfiguration<WoParameter>
{
    protected override void ConfigureEntity(EntityTypeBuilder<WoParameter> builder)
    {
        builder.ToTable("RII_WO_PARAMETER");
    }
}
