using AutoMapper;
using Wms.Application.Common;
using Wms.Application.Definitions.Dtos.Parameters;
using Wms.Domain.Entities.Definitions;

namespace Wms.Application.Definitions.Services.Parameters;

public sealed class PrParameterService
    : ParameterCrudService<PrParameter, PrParameterDto, CreatePrParameterDto, UpdatePrParameterDto>,
      IPrParameterService
{
    public PrParameterService(
        IRepository<PrParameter> repository,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILocalizationService localizationService)
        : base(repository, unitOfWork, mapper, localizationService)
    {
    }
}
