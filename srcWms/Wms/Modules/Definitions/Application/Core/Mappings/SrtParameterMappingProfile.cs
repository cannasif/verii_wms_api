using AutoMapper;
using Wms.Application.Definitions.Dtos.Parameters;
using Wms.Domain.Entities.Definitions;

namespace Wms.Application.Definitions.Mappings;

public sealed class SrtParameterMappingProfile : Profile
{
    public SrtParameterMappingProfile()
    {
        CreateMap<SrtParameter, SrtParameterDto>();
        CreateMap<CreateSrtParameterDto, SrtParameter>();
        CreateMap<UpdateSrtParameterDto, SrtParameter>()
            .ForAllMembers(options => options.Condition((_, _, sourceMember) => sourceMember != null));
    }
}
