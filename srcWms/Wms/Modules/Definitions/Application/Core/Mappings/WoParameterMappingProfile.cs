using AutoMapper;
using Wms.Application.Definitions.Dtos.Parameters;
using Wms.Domain.Entities.Definitions;

namespace Wms.Application.Definitions.Mappings;

public sealed class WoParameterMappingProfile : Profile
{
    public WoParameterMappingProfile()
    {
        CreateMap<WoParameter, WoParameterDto>();
        CreateMap<CreateWoParameterDto, WoParameter>();
        CreateMap<UpdateWoParameterDto, WoParameter>()
            .ForAllMembers(options => options.Condition((_, _, sourceMember) => sourceMember != null));
    }
}
