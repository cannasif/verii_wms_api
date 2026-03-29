using AutoMapper;
using Wms.Application.WarehouseOutbound.Dtos;
using Wms.Domain.Entities.WarehouseOutbound.Functions;

namespace Wms.Application.WarehouseOutbound.Mappings;

public sealed class WoFunctionMappingProfile : Profile
{
    public WoFunctionMappingProfile()
    {
        CreateMap<FnWoOpenOrderHeader, WoOpenOrderHeaderDto>();
        CreateMap<FnWoOpenOrderLine, WoOpenOrderLineDto>();
    }
}
