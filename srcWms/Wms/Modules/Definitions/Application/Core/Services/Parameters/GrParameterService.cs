using AutoMapper;
using Wms.Application.Common;
using Wms.Application.Definitions.Dtos.Parameters;
using Wms.Domain.Entities.Definitions;

namespace Wms.Application.Definitions.Services.Parameters;

/// <summary>
/// `_old/reference/verii_wms_api.Infrastructure/Services/Parameter/GrParameterService.cs` use-case karşılığıdır.
/// </summary>
public sealed class GrParameterService
    : ParameterCrudService<GrParameter, GrParameterDto, CreateGrParameterDto, UpdateGrParameterDto>,
      IGrParameterService
{
    public GrParameterService(
        IRepository<GrParameter> repository,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILocalizationService localizationService)
        : base(repository, unitOfWork, mapper, localizationService)
    {
    }
}
