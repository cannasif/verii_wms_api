using AutoMapper;
using Wms.Application.SubcontractingReceiptTransfer.Dtos;
using Wms.Domain.Entities.SubcontractingReceiptTransfer;

namespace Wms.Application.SubcontractingReceiptTransfer.Mappings;

public sealed class SrtChildMappingProfile : Profile
{
    public SrtChildMappingProfile()
    {
        CreateMap<SrtLine, SrtLineDto>();
        CreateMap<CreateSrtLineDto, SrtLine>();
        CreateMap<CreateSrtLineWithKeyDto, SrtLine>();
        CreateMap<UpdateSrtLineDto, SrtLine>();

        CreateMap<SrtImportLine, SrtImportLineDto>();
        CreateMap<CreateSrtImportLineDto, SrtImportLine>();
        CreateMap<CreateSrtImportLineWithKeysDto, SrtImportLine>();
        CreateMap<UpdateSrtImportLineDto, SrtImportLine>();

        CreateMap<SrtRoute, SrtRouteDto>();
        CreateMap<CreateSrtRouteDto, SrtRoute>();
        CreateMap<CreateSrtRouteWithLineKeyDto, SrtRoute>();
        CreateMap<UpdateSrtRouteDto, SrtRoute>();

        CreateMap<SrtLineSerial, SrtLineSerialDto>();
        CreateMap<CreateSrtLineSerialDto, SrtLineSerial>();
        CreateMap<CreateSrtLineSerialWithLineKeyDto, SrtLineSerial>();
        CreateMap<UpdateSrtLineSerialDto, SrtLineSerial>();

        CreateMap<SrtTerminalLine, SrtTerminalLineDto>();
        CreateMap<CreateSrtTerminalLineDto, SrtTerminalLine>();
        CreateMap<CreateSrtTerminalLineWithUserDto, SrtTerminalLine>();
        CreateMap<UpdateSrtTerminalLineDto, SrtTerminalLine>();
    }
}
