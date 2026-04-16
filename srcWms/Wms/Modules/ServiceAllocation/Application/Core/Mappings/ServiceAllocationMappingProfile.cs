using AutoMapper;
using Wms.Application.ServiceAllocation.Dtos;
using Wms.Domain.Entities.ServiceAllocation;

namespace Wms.Application.ServiceAllocation.Mappings;

public sealed class ServiceAllocationMappingProfile : Profile
{
    public ServiceAllocationMappingProfile()
    {
        CreateMap<ServiceCase, ServiceCaseDto>();
        CreateMap<CreateServiceCaseDto, ServiceCase>();
        CreateMap<UpdateServiceCaseDto, ServiceCase>();

        CreateMap<ServiceCaseLine, ServiceCaseLineDto>();
        CreateMap<CreateServiceCaseLineDto, ServiceCaseLine>();
        CreateMap<UpdateServiceCaseLineDto, ServiceCaseLine>();

        CreateMap<OrderAllocationLine, OrderAllocationLineDto>();
        CreateMap<CreateOrderAllocationLineDto, OrderAllocationLine>();
        CreateMap<UpdateOrderAllocationLineDto, OrderAllocationLine>();

        CreateMap<BusinessDocumentLink, BusinessDocumentLinkDto>();
        CreateMap<CreateBusinessDocumentLinkDto, BusinessDocumentLink>();
        CreateMap<UpdateBusinessDocumentLinkDto, BusinessDocumentLink>();
    }
}
