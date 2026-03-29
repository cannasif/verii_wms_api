using AutoMapper;
using Wms.Application.Common;
using Wms.Application.Definitions.Dtos.Parameters;
using Wms.Domain.Entities.Definitions;

namespace Wms.Application.Definitions.Services.Parameters;

public sealed class SrtParameterService
    : ParameterCrudService<SrtParameter, SrtParameterDto, CreateSrtParameterDto, UpdateSrtParameterDto>,
      ISrtParameterService
{
    public SrtParameterService(
        IRepository<SrtParameter> repository,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILocalizationService localizationService)
        : base(repository, unitOfWork, mapper, localizationService)
    {
    }
}
