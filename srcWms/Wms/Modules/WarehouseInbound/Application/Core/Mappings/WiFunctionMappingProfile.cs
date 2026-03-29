using AutoMapper;
using Wms.Application.WarehouseInbound.Dtos;
using Wms.Domain.Entities.WarehouseInbound.Functions;

namespace Wms.Application.WarehouseInbound.Mappings;

public sealed class WiFunctionMappingProfile : Profile
{
    public WiFunctionMappingProfile()
    {
        CreateMap<FnWiOpenOrderHeader, WiOpenOrderHeaderDto>();
        CreateMap<FnWiOpenOrderLine, WiOpenOrderLineDto>();
    }
}
