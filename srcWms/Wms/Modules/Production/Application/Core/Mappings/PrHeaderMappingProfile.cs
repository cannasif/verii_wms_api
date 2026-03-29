using AutoMapper;
using Wms.Application.Production.Dtos;
using Wms.Domain.Entities.Production;

namespace Wms.Application.Production.Mappings;

public sealed class PrHeaderMappingProfile : Profile
{
    public PrHeaderMappingProfile()
    {
        CreateMap<PrHeader, PrHeaderDto>();
        CreateMap<CreatePrHeaderDto, PrHeader>()
            .ForMember(dest => dest.BranchCode, opt => opt.MapFrom(src => string.IsNullOrWhiteSpace(src.BranchCode) ? "0" : src.BranchCode));
        CreateMap<UpdatePrHeaderDto, PrHeader>()
            .ForMember(dest => dest.BranchCode, opt => opt.MapFrom(src => string.IsNullOrWhiteSpace(src.BranchCode) ? "0" : src.BranchCode));
    }
}
