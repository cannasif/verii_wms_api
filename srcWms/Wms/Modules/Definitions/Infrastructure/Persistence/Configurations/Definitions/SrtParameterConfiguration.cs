using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wms.Domain.Entities.Definitions;
using Wms.Infrastructure.Persistence.Configurations.Common;

namespace Wms.Infrastructure.Persistence.Configurations.Definitions;

public sealed class SrtParameterConfiguration : BaseParameterConfiguration<SrtParameter>
{
    protected override void ConfigureEntity(EntityTypeBuilder<SrtParameter> builder)
    {
        builder.ToTable("RII_SRT_PARAMETER");
    }
}
