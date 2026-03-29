using AutoMapper;
using Wms.Application.Definitions.Dtos.Parameters;
using Wms.Domain.Entities.Definitions;

namespace Wms.Application.Definitions.Mappings;

public sealed class PrParameterMappingProfile : Profile
{
    public PrParameterMappingProfile()
    {
        CreateMap<PrParameter, PrParameterDto>();
        CreateMap<CreatePrParameterDto, PrParameter>();
        CreateMap<UpdatePrParameterDto, PrParameter>()
            .ForAllMembers(options => options.Condition((_, _, sourceMember) => sourceMember != null));
    }
}
