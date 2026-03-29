using AutoMapper;
using Wms.Application.SubcontractingIssueTransfer.Dtos;
using Wms.Domain.Entities.SubcontractingIssueTransfer;

namespace Wms.Application.SubcontractingIssueTransfer.Mappings;

public sealed class SitHeaderMappingProfile : Profile
{
    public SitHeaderMappingProfile()
    {
        CreateMap<SitHeader, SitHeaderDto>();
        CreateMap<CreateSitHeaderDto, SitHeader>()
            .ForMember(dest => dest.BranchCode, opt => opt.MapFrom(src => string.IsNullOrWhiteSpace(src.BranchCode) ? "0" : src.BranchCode));
        CreateMap<UpdateSitHeaderDto, SitHeader>()
            .ForMember(dest => dest.BranchCode, opt => opt.MapFrom(src => string.IsNullOrWhiteSpace(src.BranchCode) ? "0" : src.BranchCode));
    }
}
