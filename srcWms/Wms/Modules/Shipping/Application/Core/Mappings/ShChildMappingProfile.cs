using AutoMapper;
using Wms.Application.Shipping.Dtos;
using Wms.Domain.Entities.Shipping;

namespace Wms.Application.Shipping.Mappings;

public sealed class ShChildMappingProfile : Profile
{
    public ShChildMappingProfile()
    {
        CreateMap<ShLine, ShLineDto>();
        CreateMap<CreateShLineDto, ShLine>();
        CreateMap<CreateShLineWithKeyDto, ShLine>();
        CreateMap<UpdateShLineDto, ShLine>();

        CreateMap<ShImportLine, ShImportLineDto>();
        CreateMap<CreateShImportLineDto, ShImportLine>();
        CreateMap<CreateShImportLineWithKeysDto, ShImportLine>();
        CreateMap<UpdateShImportLineDto, ShImportLine>();

        CreateMap<ShRoute, ShRouteDto>();
        CreateMap<CreateShRouteDto, ShRoute>();
        CreateMap<CreateShRouteWithLineKeyDto, ShRoute>();
        CreateMap<UpdateShRouteDto, ShRoute>();

        CreateMap<ShLineSerial, ShLineSerialDto>();
        CreateMap<CreateShLineSerialDto, ShLineSerial>();
        CreateMap<CreateShLineSerialWithLineKeyDto, ShLineSerial>();
        CreateMap<UpdateShLineSerialDto, ShLineSerial>();

        CreateMap<ShTerminalLine, ShTerminalLineDto>();
        CreateMap<CreateShTerminalLineDto, ShTerminalLine>();
        CreateMap<CreateShTerminalLineWithUserDto, ShTerminalLine>();
        CreateMap<UpdateShTerminalLineDto, ShTerminalLine>();
    }
}
