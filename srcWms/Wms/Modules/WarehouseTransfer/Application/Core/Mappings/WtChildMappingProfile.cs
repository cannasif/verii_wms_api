using AutoMapper;
using Wms.Application.WarehouseTransfer.Dtos;
using Wms.Domain.Entities.WarehouseTransfer;

namespace Wms.Application.WarehouseTransfer.Mappings;

public sealed class WtChildMappingProfile : Profile
{
    public WtChildMappingProfile()
    {
        CreateMap<WtLine, WtLineDto>();
        CreateMap<CreateWtLineDto, WtLine>();
        CreateMap<UpdateWtLineDto, WtLine>();

        CreateMap<WtImportLine, WtImportLineDto>();
        CreateMap<CreateWtImportLineDto, WtImportLine>();
        CreateMap<UpdateWtImportLineDto, WtImportLine>();

        CreateMap<WtRoute, WtRouteDto>();
        CreateMap<CreateWtRouteDto, WtRoute>();
        CreateMap<UpdateWtRouteDto, WtRoute>();

        CreateMap<WtLineSerial, WtLineSerialDto>();
        CreateMap<CreateWtLineSerialDto, WtLineSerial>();
        CreateMap<UpdateWtLineSerialDto, WtLineSerial>();

        CreateMap<WtTerminalLine, WtTerminalLineDto>();
        CreateMap<CreateWtTerminalLineDto, WtTerminalLine>();
        CreateMap<UpdateWtTerminalLineDto, WtTerminalLine>();
    }
}
