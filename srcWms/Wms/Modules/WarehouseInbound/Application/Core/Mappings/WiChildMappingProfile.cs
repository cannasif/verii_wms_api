using AutoMapper;
using Wms.Application.WarehouseInbound.Dtos;
using Wms.Domain.Entities.WarehouseInbound;

namespace Wms.Application.WarehouseInbound.Mappings;

public sealed class WiChildMappingProfile : Profile
{
    public WiChildMappingProfile()
    {
        CreateMap<WiLine, WiLineDto>();
        CreateMap<CreateWiLineDto, WiLine>();
        CreateMap<UpdateWiLineDto, WiLine>();

        CreateMap<WiImportLine, WiImportLineDto>();
        CreateMap<CreateWiImportLineDto, WiImportLine>();
        CreateMap<UpdateWiImportLineDto, WiImportLine>();

        CreateMap<WiRoute, WiRouteDto>();
        CreateMap<CreateWiRouteDto, WiRoute>();
        CreateMap<UpdateWiRouteDto, WiRoute>();

        CreateMap<WiLineSerial, WiLineSerialDto>();
        CreateMap<CreateWiLineSerialDto, WiLineSerial>();
        CreateMap<UpdateWiLineSerialDto, WiLineSerial>();

        CreateMap<WiTerminalLine, WiTerminalLineDto>();
        CreateMap<CreateWiTerminalLineDto, WiTerminalLine>();
        CreateMap<UpdateWiTerminalLineDto, WiTerminalLine>();
    }
}
