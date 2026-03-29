using AutoMapper;
using Wms.Application.ProductionTransfer.Dtos;
using Wms.Domain.Entities.ProductionTransfer;

namespace Wms.Application.ProductionTransfer.Mappings;

public sealed class PtHeaderMappingProfile : Profile
{
    public PtHeaderMappingProfile()
    {
        CreateMap<PtHeader, PtHeaderDto>();
        CreateMap<CreatePtHeaderDto, PtHeader>()
            .ForMember(dest => dest.BranchCode, opt => opt.MapFrom(src => string.IsNullOrWhiteSpace(src.BranchCode) ? "0" : src.BranchCode));
        CreateMap<UpdatePtHeaderDto, PtHeader>()
            .ForMember(dest => dest.BranchCode, opt => opt.MapFrom(src => string.IsNullOrWhiteSpace(src.BranchCode) ? "0" : src.BranchCode));
    }
}
