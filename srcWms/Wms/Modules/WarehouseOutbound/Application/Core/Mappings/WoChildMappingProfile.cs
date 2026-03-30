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
        CreateMap<CreateWoLineWithKeyDto, WoLine>();
        CreateMap<UpdateWoLineDto, WoLine>();

        CreateMap<WoImportLine, WoImportLineDto>();
        CreateMap<CreateWoImportLineDto, WoImportLine>();
        CreateMap<CreateWoImportLineWithKeysDto, WoImportLine>();
        CreateMap<UpdateWoImportLineDto, WoImportLine>();

        CreateMap<WoRoute, WoRouteDto>();
        CreateMap<CreateWoRouteDto, WoRoute>();
        CreateMap<CreateWoRouteWithLineKeyDto, WoRoute>();
        CreateMap<UpdateWoRouteDto, WoRoute>();

        CreateMap<WoLineSerial, WoLineSerialDto>();
        CreateMap<CreateWoLineSerialDto, WoLineSerial>();
        CreateMap<CreateWoLineSerialWithLineKeyDto, WoLineSerial>();
        CreateMap<UpdateWoLineSerialDto, WoLineSerial>();

        CreateMap<WoTerminalLine, WoTerminalLineDto>();
        CreateMap<CreateWoTerminalLineDto, WoTerminalLine>();
        CreateMap<CreateWoTerminalLineWithUserDto, WoTerminalLine>();
        CreateMap<UpdateWoTerminalLineDto, WoTerminalLine>();
    }
}
