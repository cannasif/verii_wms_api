using AutoMapper;
using Wms.Application.GoodsReceipt.Dtos;
using Wms.Domain.Entities.GoodsReceipt;

namespace Wms.Application.GoodsReceipt.Mappings;

public sealed class GrChildMappingProfile : Profile
{
    public GrChildMappingProfile()
    {
        CreateMap<GrLine, GrLineDto>();
        CreateMap<CreateGrLineDto, GrLine>();
        CreateMap<UpdateGrLineDto, GrLine>();

        CreateMap<GrRoute, GrRouteDto>();
        CreateMap<CreateGrRouteDto, GrRoute>();
        CreateMap<UpdateGrRouteDto, GrRoute>();

        CreateMap<GrTerminalLine, GrTerminalLineDto>();
        CreateMap<CreateGrTerminalLineDto, GrTerminalLine>();
        CreateMap<UpdateGrTerminalLineDto, GrTerminalLine>();

        CreateMap<GrImportDocument, GrImportDocumentDto>();
        CreateMap<CreateGrImportDocumentDto, GrImportDocument>();
        CreateMap<UpdateGrImportDocumentDto, GrImportDocument>();

        CreateMap<CreateGrImportLineDto, GrImportLine>();
        CreateMap<UpdateGrImportLineDto, GrImportLine>();
        CreateMap<UpdateGrLineSerialDto, GrLineSerial>();
    }
}
