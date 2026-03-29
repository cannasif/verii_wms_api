using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wms.Domain.Entities.Definitions;
using Wms.Infrastructure.Persistence.Configurations.Common;

namespace Wms.Infrastructure.Persistence.Configurations.Definitions;

/// <summary>
/// `_old/reference/.../GrParameterConfiguration.cs` tablo eşlemesini taşır.
/// </summary>
public sealed class GrParameterConfiguration : BaseParameterConfiguration<GrParameter>
{
    protected override void ConfigureEntity(EntityTypeBuilder<GrParameter> builder)
    {
        builder.ToTable("RII_GR_PARAMETER");
    }
}
