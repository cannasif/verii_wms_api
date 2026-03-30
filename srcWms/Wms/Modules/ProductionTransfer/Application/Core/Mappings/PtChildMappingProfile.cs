using AutoMapper;
using Wms.Application.ProductionTransfer.Dtos;
using Wms.Domain.Entities.ProductionTransfer;

namespace Wms.Application.ProductionTransfer.Mappings;

public sealed class PtChildMappingProfile : Profile
{
    public PtChildMappingProfile()
    {
        CreateMap<PtLine, PtLineDto>();
        CreateMap<CreatePtLineDto, PtLine>();
        CreateMap<CreatePtLineWithKeyDto, PtLine>();
        CreateMap<UpdatePtLineDto, PtLine>();

        CreateMap<PtImportLine, PtImportLineDto>();
        CreateMap<CreatePtImportLineDto, PtImportLine>();
        CreateMap<CreatePtImportLineWithKeysDto, PtImportLine>();
        CreateMap<UpdatePtImportLineDto, PtImportLine>();

        CreateMap<PtRoute, PtRouteDto>();
        CreateMap<CreatePtRouteDto, PtRoute>();
        CreateMap<CreatePtRouteWithLineKeyDto, PtRoute>();
        CreateMap<UpdatePtRouteDto, PtRoute>();

        CreateMap<PtLineSerial, PtLineSerialDto>();
        CreateMap<CreatePtLineSerialDto, PtLineSerial>();
        CreateMap<CreatePtLineSerialWithLineKeyDto, PtLineSerial>();
        CreateMap<UpdatePtLineSerialDto, PtLineSerial>();

        CreateMap<PtTerminalLine, PtTerminalLineDto>();
        CreateMap<CreatePtTerminalLineDto, PtTerminalLine>();
        CreateMap<CreatePtTerminalLineWithUserDto, PtTerminalLine>();
        CreateMap<UpdatePtTerminalLineDto, PtTerminalLine>();
    }
}
