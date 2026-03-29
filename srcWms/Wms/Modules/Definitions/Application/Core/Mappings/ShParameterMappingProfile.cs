using AutoMapper;
using Wms.Application.Definitions.Dtos.Parameters;
using Wms.Domain.Entities.Definitions;

namespace Wms.Application.Definitions.Mappings;

public sealed class ShParameterMappingProfile : Profile
{
    public ShParameterMappingProfile()
    {
        CreateMap<ShParameter, ShParameterDto>();
        CreateMap<CreateShParameterDto, ShParameter>();
        CreateMap<UpdateShParameterDto, ShParameter>()
            .ForAllMembers(options => options.Condition((_, _, sourceMember) => sourceMember != null));
    }
}
