using AutoMapper;
using Wms.Application.Definitions.Dtos.Parameters;
using Wms.Domain.Entities.Definitions;

namespace Wms.Application.Definitions.Mappings;

public sealed class SitParameterMappingProfile : Profile
{
    public SitParameterMappingProfile()
    {
        CreateMap<SitParameter, SitParameterDto>();
        CreateMap<CreateSitParameterDto, SitParameter>();
        CreateMap<UpdateSitParameterDto, SitParameter>()
            .ForAllMembers(options => options.Condition((_, _, sourceMember) => sourceMember != null));
    }
}
