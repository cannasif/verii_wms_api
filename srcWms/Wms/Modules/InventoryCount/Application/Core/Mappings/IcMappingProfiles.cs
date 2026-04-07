using AutoMapper;
using Wms.Application.InventoryCount.Dtos;
using Wms.Domain.Entities.InventoryCount;

namespace Wms.Application.InventoryCount.Mappings;

public sealed class IcHeaderMappingProfile : Profile
{
    public IcHeaderMappingProfile()
    {
        CreateMap<IcHeader, IcHeaderDto>();
        CreateMap<CreateIcHeaderDto, IcHeader>()
            .ForMember(dest => dest.BranchCode, opt => opt.MapFrom(src => string.IsNullOrWhiteSpace(src.BranchCode) ? "0" : src.BranchCode));
        CreateMap<UpdateIcHeaderDto, IcHeader>()
            .ForMember(dest => dest.BranchCode, opt => opt.Condition(src => !string.IsNullOrWhiteSpace(src.BranchCode)))
            .ForMember(dest => dest.BranchCode, opt => opt.MapFrom(src => src.BranchCode));
    }
}

public sealed class IcChildMappingProfile : Profile
{
    public IcChildMappingProfile()
    {
        CreateMap<IcImportLine, IcImportLineDto>();
        CreateMap<CreateIcImportLineDto, IcImportLine>();
        CreateMap<UpdateIcImportLineDto, IcImportLine>();

        CreateMap<IcRoute, IcRouteDto>()
            .ForMember(dest => dest.YapKod, opt => opt.MapFrom(src => src.Description ?? string.Empty))
            .ForMember(dest => dest.Priority, opt => opt.MapFrom(src => 0))
            .ForMember(dest => dest.ScannedBarcode, opt => opt.MapFrom(src => src.Barcode ?? string.Empty))
            .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.NewQuantity));
        CreateMap<CreateIcRouteDto, IcRoute>()
            .ForMember(dest => dest.Barcode, opt => opt.MapFrom(src => src.Barcode))
            .ForMember(dest => dest.NewQuantity, opt => opt.MapFrom(src => src.NewQuantity))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description));
        CreateMap<UpdateIcRouteDto, IcRoute>()
            .ForMember(dest => dest.Barcode, opt => opt.MapFrom(src => src.Barcode))
            .ForMember(dest => dest.NewQuantity, opt => opt.MapFrom(src => src.NewQuantity));

        CreateMap<IcTerminalLine, IcTerminalLineDto>();
        CreateMap<CreateIcTerminalLineDto, IcTerminalLine>();
        CreateMap<UpdateIcTerminalLineDto, IcTerminalLine>();

        CreateMap<IcScope, IcScopeDto>();
        CreateMap<CreateIcScopeDto, IcScope>();
        CreateMap<UpdateIcScopeDto, IcScope>();
        CreateMap<IcLine, IcLineDto>();
        CreateMap<CreateIcLineDto, IcLine>();
        CreateMap<UpdateIcLineDto, IcLine>();
        CreateMap<IcCountEntry, IcCountEntryDto>();
        CreateMap<CreateIcCountEntryDto, IcCountEntry>();
        CreateMap<IcAdjustment, IcAdjustmentDto>();
    }
}
