using AutoMapper;
using Wms.Application.WarehouseOutbound.Dtos;
using Wms.Domain.Entities.WarehouseOutbound;

namespace Wms.Application.WarehouseOutbound.Mappings;

public sealed class WoHeaderMappingProfile : Profile
{
    public WoHeaderMappingProfile()
    {
        CreateMap<WoHeader, WoHeaderDto>();
        CreateMap<CreateWoHeaderDto, WoHeader>()
            .ForMember(dest => dest.BranchCode, opt => opt.MapFrom(src => string.IsNullOrWhiteSpace(src.BranchCode) ? "0" : src.BranchCode));
        CreateMap<UpdateWoHeaderDto, WoHeader>()
            .ForMember(dest => dest.BranchCode, opt => opt.MapFrom(src => string.IsNullOrWhiteSpace(src.BranchCode) ? "0" : src.BranchCode));
    }
}
