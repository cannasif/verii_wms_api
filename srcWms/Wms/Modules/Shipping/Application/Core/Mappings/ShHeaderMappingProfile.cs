using AutoMapper;
using Wms.Application.Shipping.Dtos;
using Wms.Domain.Entities.Shipping;

namespace Wms.Application.Shipping.Mappings;

public sealed class ShHeaderMappingProfile : Profile
{
    public ShHeaderMappingProfile()
    {
        CreateMap<ShHeader, ShHeaderDto>();
        CreateMap<CreateShHeaderDto, ShHeader>()
            .ForMember(dest => dest.BranchCode, opt => opt.MapFrom(src => string.IsNullOrWhiteSpace(src.BranchCode) ? "0" : src.BranchCode));
        CreateMap<UpdateShHeaderDto, ShHeader>()
            .ForMember(dest => dest.BranchCode, opt => opt.MapFrom(src => string.IsNullOrWhiteSpace(src.BranchCode) ? "0" : src.BranchCode));
    }
}
