using AutoMapper;
using Wms.Application.Definitions.Dtos.Parameters;
using Wms.Domain.Entities.Definitions;

namespace Wms.Application.Definitions.Mappings;

public sealed class PParameterMappingProfile : Profile
{
    public PParameterMappingProfile()
    {
        CreateMap<PParameter, PParameterDto>();
        CreateMap<CreatePParameterDto, PParameter>();
        CreateMap<UpdatePParameterDto, PParameter>()
            .ForAllMembers(options => options.Condition((_, _, sourceMember) => sourceMember != null));
    }
}
