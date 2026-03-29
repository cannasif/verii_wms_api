using AutoMapper;
using Wms.Application.Common;
using Wms.Application.Definitions.Dtos.Parameters;
using Wms.Domain.Entities.Definitions;

namespace Wms.Application.Definitions.Services.Parameters;

public sealed class ShParameterService
    : ParameterCrudService<ShParameter, ShParameterDto, CreateShParameterDto, UpdateShParameterDto>,
      IShParameterService
{
    public ShParameterService(
        IRepository<ShParameter> repository,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILocalizationService localizationService)
        : base(repository, unitOfWork, mapper, localizationService)
    {
    }
}
