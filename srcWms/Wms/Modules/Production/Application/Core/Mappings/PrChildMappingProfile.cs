using AutoMapper;
using Wms.Application.Production.Dtos;
using Wms.Domain.Entities.Production;

namespace Wms.Application.Production.Mappings;

public sealed class PrChildMappingProfile : Profile
{
    public PrChildMappingProfile()
    {
        CreateMap<PrLine, PrLineDto>();
        CreateMap<CreatePrLineDto, PrLine>();
        CreateMap<UpdatePrLineDto, PrLine>();

        CreateMap<PrImportLine, PrImportLineDto>();
        CreateMap<CreatePrImportLineDto, PrImportLine>();
        CreateMap<UpdatePrImportLineDto, PrImportLine>();

        CreateMap<PrRoute, PrRouteDto>();
        CreateMap<CreatePrRouteDto, PrRoute>();
        CreateMap<UpdatePrRouteDto, PrRoute>();

        CreateMap<PrLineSerial, PrLineSerialDto>();
        CreateMap<CreatePrLineSerialDto, PrLineSerial>();
        CreateMap<UpdatePrLineSerialDto, PrLineSerial>();

        CreateMap<PrTerminalLine, PrTerminalLineDto>();
        CreateMap<CreatePrTerminalLineDto, PrTerminalLine>();
        CreateMap<UpdatePrTerminalLineDto, PrTerminalLine>();
    }
}
