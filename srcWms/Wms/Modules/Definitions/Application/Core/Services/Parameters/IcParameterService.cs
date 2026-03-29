using AutoMapper;
using Wms.Application.Common;
using Wms.Application.Definitions.Dtos.Parameters;
using Wms.Domain.Entities.Definitions;

namespace Wms.Application.Definitions.Services.Parameters;

public sealed class IcParameterService
    : ParameterCrudService<IcParameter, IcParameterDto, CreateIcParameterDto, UpdateIcParameterDto>,
      IIcParameterService
{
    public IcParameterService(
        IRepository<IcParameter> repository,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILocalizationService localizationService)
        : base(repository, unitOfWork, mapper, localizationService)
    {
    }
}
