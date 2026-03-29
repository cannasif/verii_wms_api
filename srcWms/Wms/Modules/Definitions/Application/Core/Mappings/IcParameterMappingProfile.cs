using AutoMapper;
using Wms.Application.Definitions.Dtos.Parameters;
using Wms.Domain.Entities.Definitions;

namespace Wms.Application.Definitions.Mappings;

public sealed class IcParameterMappingProfile : Profile
{
    public IcParameterMappingProfile()
    {
        CreateMap<IcParameter, IcParameterDto>();
        CreateMap<CreateIcParameterDto, IcParameter>();
        CreateMap<UpdateIcParameterDto, IcParameter>()
            .ForAllMembers(options => options.Condition((_, _, sourceMember) => sourceMember != null));
    }
}
