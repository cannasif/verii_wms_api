using AutoMapper;
using Wms.Application.Common;
using Wms.Application.Definitions.Dtos.Parameters;
using Wms.Domain.Entities.Definitions;

namespace Wms.Application.Definitions.Services.Parameters;

public sealed class WtParameterService
    : ParameterCrudService<WtParameter, WtParameterDto, CreateWtParameterDto, UpdateWtParameterDto>,
      IWtParameterService
{
    public WtParameterService(
        IRepository<WtParameter> repository,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILocalizationService localizationService)
        : base(repository, unitOfWork, mapper, localizationService)
    {
    }
}
