using AutoMapper;
using Wms.Application.Package.Dtos;
using Wms.Domain.Entities.Package;

namespace Wms.Application.Package.Mappings;

public sealed class PackageMappingProfile : Profile
{
    public PackageMappingProfile()
    {
        CreateMap<PHeader, PHeaderDto>();
        CreateMap<CreatePHeaderDto, PHeader>();
        CreateMap<UpdatePHeaderDto, PHeader>();

        CreateMap<PPackage, PPackageDto>();
        CreateMap<CreatePPackageDto, PPackage>();
        CreateMap<UpdatePPackageDto, PPackage>();

        CreateMap<PLine, PLineDto>();
        CreateMap<CreatePLineDto, PLine>();
        CreateMap<UpdatePLineDto, PLine>();
    }
}
