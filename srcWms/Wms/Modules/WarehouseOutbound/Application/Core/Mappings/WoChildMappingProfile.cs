using AutoMapper;
using Wms.Application.WarehouseOutbound.Dtos;
using Wms.Domain.Entities.WarehouseOutbound;

namespace Wms.Application.WarehouseOutbound.Mappings;

public sealed class WoChildMappingProfile : Profile
{
    public WoChildMappingProfile()
    {
        CreateMap<WoLine, WoLineDto>();
        CreateMap<CreateWoLineDto, WoLine>();
        CreateMap<UpdateWoLineDto, WoLine>();

        CreateMap<WoImportLine, WoImportLineDto>();
        CreateMap<CreateWoImportLineDto, WoImportLine>();
        CreateMap<UpdateWoImportLineDto, WoImportLine>();

        CreateMap<WoRoute, WoRouteDto>();
        CreateMap<CreateWoRouteDto, WoRoute>();
        CreateMap<UpdateWoRouteDto, WoRoute>();

        CreateMap<WoLineSerial, WoLineSerialDto>();
        CreateMap<CreateWoLineSerialDto, WoLineSerial>();
        CreateMap<UpdateWoLineSerialDto, WoLineSerial>();

        CreateMap<WoTerminalLine, WoTerminalLineDto>();
        CreateMap<CreateWoTerminalLineDto, WoTerminalLine>();
        CreateMap<UpdateWoTerminalLineDto, WoTerminalLine>();
    }
}
