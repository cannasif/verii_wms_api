using AutoMapper;
using Wms.Application.Definitions.Dtos.Parameters;
using Wms.Domain.Entities.Definitions;

namespace Wms.Application.Definitions.Mappings;

public sealed class PtParameterMappingProfile : Profile
{
    public PtParameterMappingProfile()
    {
        CreateMap<PtParameter, PtParameterDto>();
        CreateMap<CreatePtParameterDto, PtParameter>();
        CreateMap<UpdatePtParameterDto, PtParameter>()
            .ForAllMembers(options => options.Condition((_, _, sourceMember) => sourceMember != null));
    }
}
