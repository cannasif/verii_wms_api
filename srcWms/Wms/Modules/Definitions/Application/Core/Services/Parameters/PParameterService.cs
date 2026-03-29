using AutoMapper;
using Wms.Application.Common;
using Wms.Application.Definitions.Dtos.Parameters;
using Wms.Domain.Entities.Definitions;

namespace Wms.Application.Definitions.Services.Parameters;

public sealed class PParameterService
    : ParameterCrudService<PParameter, PParameterDto, CreatePParameterDto, UpdatePParameterDto>,
      IPParameterService
{
    public PParameterService(
        IRepository<PParameter> repository,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILocalizationService localizationService)
        : base(repository, unitOfWork, mapper, localizationService)
    {
    }
}
