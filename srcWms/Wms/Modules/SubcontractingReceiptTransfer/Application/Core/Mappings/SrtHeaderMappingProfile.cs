using AutoMapper;
using Wms.Application.SubcontractingReceiptTransfer.Dtos;
using Wms.Domain.Entities.SubcontractingReceiptTransfer;

namespace Wms.Application.SubcontractingReceiptTransfer.Mappings;

public sealed class SrtHeaderMappingProfile : Profile
{
    public SrtHeaderMappingProfile()
    {
        CreateMap<SrtHeader, SrtHeaderDto>();
        CreateMap<CreateSrtHeaderDto, SrtHeader>()
            .ForMember(dest => dest.BranchCode, opt => opt.MapFrom(src => string.IsNullOrWhiteSpace(src.BranchCode) ? "0" : src.BranchCode));
        CreateMap<UpdateSrtHeaderDto, SrtHeader>()
            .ForMember(dest => dest.BranchCode, opt => opt.MapFrom(src => string.IsNullOrWhiteSpace(src.BranchCode) ? "0" : src.BranchCode));
    }
}
