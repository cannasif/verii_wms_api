using AutoMapper;
using Wms.Application.SubcontractingIssueTransfer.Dtos;
using Wms.Domain.Entities.SubcontractingIssueTransfer;

namespace Wms.Application.SubcontractingIssueTransfer.Mappings;

public sealed class SitChildMappingProfile : Profile
{
    public SitChildMappingProfile()
    {
        CreateMap<SitLine, SitLineDto>();
        CreateMap<CreateSitLineDto, SitLine>();
        CreateMap<UpdateSitLineDto, SitLine>();

        CreateMap<SitImportLine, SitImportLineDto>();
        CreateMap<CreateSitImportLineDto, SitImportLine>();
        CreateMap<UpdateSitImportLineDto, SitImportLine>();

        CreateMap<SitRoute, SitRouteDto>();
        CreateMap<CreateSitRouteDto, SitRoute>();
        CreateMap<UpdateSitRouteDto, SitRoute>();

        CreateMap<SitLineSerial, SitLineSerialDto>();
        CreateMap<CreateSitLineSerialDto, SitLineSerial>();
        CreateMap<UpdateSitLineSerialDto, SitLineSerial>();

        CreateMap<SitTerminalLine, SitTerminalLineDto>();
        CreateMap<CreateSitTerminalLineDto, SitTerminalLine>();
        CreateMap<UpdateSitTerminalLineDto, SitTerminalLine>();
    }
}
