using AutoMapper;
using Wms.Application.Common;
using Wms.Application.Definitions.Dtos.Parameters;
using Wms.Domain.Entities.Definitions;

namespace Wms.Application.Definitions.Services.Parameters;

public sealed class PtParameterService
    : ParameterCrudService<PtParameter, PtParameterDto, CreatePtParameterDto, UpdatePtParameterDto>,
      IPtParameterService
{
    public PtParameterService(
        IRepository<PtParameter> repository,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILocalizationService localizationService)
        : base(repository, unitOfWork, mapper, localizationService)
    {
    }
}
