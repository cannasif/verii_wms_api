using AutoMapper;
using Wms.Application.GoodsReceipt.Dtos;
using Wms.Domain.Entities.GoodsReceipt;

namespace Wms.Application.GoodsReceipt.Mappings;

public sealed class GrHeaderMappingProfile : Profile
{
    public GrHeaderMappingProfile()
    {
        CreateMap<GrHeader, GrHeaderDto>();
        CreateMap<CreateGrHeaderDto, GrHeader>()
            .ForMember(dest => dest.BranchCode, opt => opt.MapFrom(src => string.IsNullOrWhiteSpace(src.BranchCode) ? "0" : src.BranchCode));
        CreateMap<UpdateGrHeaderDto, GrHeader>()
            .ForMember(dest => dest.BranchCode, opt => opt.MapFrom(src => string.IsNullOrWhiteSpace(src.BranchCode) ? "0" : src.BranchCode));
        CreateMap<GrLine, GrLineDto>();
        CreateMap<CreateGrLineWithKeyDto, GrLine>();
        CreateMap<GrImportLine, GrImportLineDto>();
        CreateMap<CreateGrImportLineWithLineKeyDto, GrImportLine>();
        CreateMap<GrRoute, GrRouteDto>();
        CreateMap<CreateGrRouteWithImportLineKeyDto, GrRoute>();
        CreateMap<GrLineSerial, GrLineSerialDto>();
        CreateMap<CreateGrLineSerialDto, GrLineSerial>();
        CreateMap<CreateGrLineSerialWithLineKeyDto, GrLineSerial>();
        CreateMap<CreateGrImportDocumentSimpleDto, GrImportDocument>();
        CreateMap<CreateGrTerminalLineWithUserDto, GrTerminalLine>();
    }
}
