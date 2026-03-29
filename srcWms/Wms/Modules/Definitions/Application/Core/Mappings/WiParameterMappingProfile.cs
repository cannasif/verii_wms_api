using AutoMapper;
using Wms.Application.Definitions.Dtos.Parameters;
using Wms.Domain.Entities.Definitions;

namespace Wms.Application.Definitions.Mappings;

public sealed class WiParameterMappingProfile : Profile
{
    public WiParameterMappingProfile()
    {
        CreateMap<WiParameter, WiParameterDto>();
        CreateMap<CreateWiParameterDto, WiParameter>();
        CreateMap<UpdateWiParameterDto, WiParameter>()
            .ForAllMembers(options => options.Condition((_, _, sourceMember) => sourceMember != null));
    }
}
