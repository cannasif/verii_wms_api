using AutoMapper;
using Wms.Application.Common;
using Wms.Application.Definitions.Dtos.Parameters;
using Wms.Domain.Entities.Definitions;

namespace Wms.Application.Definitions.Services.Parameters;

public sealed class WiParameterService
    : ParameterCrudService<WiParameter, WiParameterDto, CreateWiParameterDto, UpdateWiParameterDto>,
      IWiParameterService
{
    public WiParameterService(
        IRepository<WiParameter> repository,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILocalizationService localizationService)
        : base(repository, unitOfWork, mapper, localizationService)
    {
    }
}
