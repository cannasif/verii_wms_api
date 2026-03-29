using AutoMapper;
using Wms.Application.Definitions.Dtos.Parameters;
using Wms.Domain.Entities.Definitions;

namespace Wms.Application.Definitions.Mappings;

/// <summary>
/// `_old` GrParameter DTO/entity mapping davranışını yeni yapıda karşılar.
/// </summary>
public sealed class GrParameterMappingProfile : Profile
{
    public GrParameterMappingProfile()
    {
        CreateMap<GrParameter, GrParameterDto>();
        CreateMap<CreateGrParameterDto, GrParameter>();

        CreateMap<UpdateGrParameterDto, GrParameter>()
            .ForAllMembers(options => options.Condition((_, _, sourceMember) => sourceMember != null));
    }
}
