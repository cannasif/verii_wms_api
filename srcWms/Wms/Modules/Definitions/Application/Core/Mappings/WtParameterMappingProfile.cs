using AutoMapper;
using Wms.Application.Definitions.Dtos.Parameters;
using Wms.Domain.Entities.Definitions;

namespace Wms.Application.Definitions.Mappings;

public sealed class WtParameterMappingProfile : Profile
{
    public WtParameterMappingProfile()
    {
        CreateMap<WtParameter, WtParameterDto>();
        CreateMap<CreateWtParameterDto, WtParameter>();
        CreateMap<UpdateWtParameterDto, WtParameter>()
            .ForAllMembers(options => options.Condition((_, _, sourceMember) => sourceMember != null));
    }
}
