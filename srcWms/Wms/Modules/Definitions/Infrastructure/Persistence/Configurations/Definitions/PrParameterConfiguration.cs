using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wms.Domain.Entities.Definitions;
using Wms.Infrastructure.Persistence.Configurations.Common;

namespace Wms.Infrastructure.Persistence.Configurations.Definitions;

public sealed class PrParameterConfiguration : BaseParameterConfiguration<PrParameter>
{
    protected override void ConfigureEntity(EntityTypeBuilder<PrParameter> builder)
    {
        builder.ToTable("RII_PR_PARAMETER");
    }
}
