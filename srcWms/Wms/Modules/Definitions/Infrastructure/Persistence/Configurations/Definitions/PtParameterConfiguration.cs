using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wms.Domain.Entities.Definitions;
using Wms.Infrastructure.Persistence.Configurations.Common;

namespace Wms.Infrastructure.Persistence.Configurations.Definitions;

public sealed class PtParameterConfiguration : BaseParameterConfiguration<PtParameter>
{
    protected override void ConfigureEntity(EntityTypeBuilder<PtParameter> builder)
    {
        builder.ToTable("RII_PT_PARAMETER");
    }
}
