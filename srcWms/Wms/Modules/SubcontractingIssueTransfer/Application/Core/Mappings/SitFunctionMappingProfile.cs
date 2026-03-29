using AutoMapper;
using Wms.Application.SubcontractingIssueTransfer.Dtos;
using Wms.Domain.Entities.SubcontractingIssueTransfer.Functions;

namespace Wms.Application.SubcontractingIssueTransfer.Mappings;

public sealed class SitFunctionMappingProfile : Profile
{
    public SitFunctionMappingProfile()
    {
        CreateMap<FnSitOpenOrderHeader, SitOpenOrderHeaderDto>();
        CreateMap<FnSitOpenOrderLine, SitOpenOrderLineDto>();
    }
}
