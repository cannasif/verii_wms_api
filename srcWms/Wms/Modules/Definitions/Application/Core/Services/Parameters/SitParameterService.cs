using AutoMapper;
using Wms.Application.Common;
using Wms.Application.Definitions.Dtos.Parameters;
using Wms.Domain.Entities.Definitions;

namespace Wms.Application.Definitions.Services.Parameters;

public sealed class SitParameterService
    : ParameterCrudService<SitParameter, SitParameterDto, CreateSitParameterDto, UpdateSitParameterDto>,
      ISitParameterService
{
    public SitParameterService(
        IRepository<SitParameter> repository,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILocalizationService localizationService)
        : base(repository, unitOfWork, mapper, localizationService)
    {
    }
}
