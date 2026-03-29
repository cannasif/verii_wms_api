using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wms.Domain.Entities.Definitions;
using Wms.Infrastructure.Persistence.Configurations.Common;

namespace Wms.Infrastructure.Persistence.Configurations.Definitions;

public sealed class PParameterConfiguration : BaseParameterConfiguration<PParameter>
{
    protected override void ConfigureEntity(EntityTypeBuilder<PParameter> builder)
    {
        builder.ToTable("RII_P_PARAMETER");
    }
}
