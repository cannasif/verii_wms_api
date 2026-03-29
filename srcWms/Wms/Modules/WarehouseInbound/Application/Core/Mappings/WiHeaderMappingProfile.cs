using AutoMapper;
using Wms.Application.WarehouseInbound.Dtos;
using Wms.Domain.Entities.WarehouseInbound;

namespace Wms.Application.WarehouseInbound.Mappings;

public sealed class WiHeaderMappingProfile : Profile
{
    public WiHeaderMappingProfile()
    {
        CreateMap<WiHeader, WiHeaderDto>();
        CreateMap<CreateWiHeaderDto, WiHeader>()
            .ForMember(dest => dest.BranchCode, opt => opt.MapFrom(src => string.IsNullOrWhiteSpace(src.BranchCode) ? "0" : src.BranchCode));
        CreateMap<UpdateWiHeaderDto, WiHeader>()
            .ForMember(dest => dest.BranchCode, opt => opt.MapFrom(src => string.IsNullOrWhiteSpace(src.BranchCode) ? "0" : src.BranchCode));
    }
}
