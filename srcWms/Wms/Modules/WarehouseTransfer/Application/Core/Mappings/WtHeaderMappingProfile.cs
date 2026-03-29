using AutoMapper;
using Wms.Application.WarehouseTransfer.Dtos;
using Wms.Domain.Entities.WarehouseTransfer;

namespace Wms.Application.WarehouseTransfer.Mappings;

public sealed class WtHeaderMappingProfile : Profile
{
    public WtHeaderMappingProfile()
    {
        CreateMap<WtHeader, WtHeaderDto>();
        CreateMap<CreateWtHeaderDto, WtHeader>()
            .ForMember(dest => dest.BranchCode, opt => opt.MapFrom(src => string.IsNullOrWhiteSpace(src.BranchCode) ? "0" : src.BranchCode));
        CreateMap<UpdateWtHeaderDto, WtHeader>()
            .ForMember(dest => dest.BranchCode, opt => opt.MapFrom(src => string.IsNullOrWhiteSpace(src.BranchCode) ? "0" : src.BranchCode));
    }
}
