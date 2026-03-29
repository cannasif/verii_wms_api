using AutoMapper;
using Wms.Application.Shipping.Dtos;
using Wms.Domain.Entities.Shipping.Functions;

namespace Wms.Application.Shipping.Mappings;

public sealed class ShFunctionMappingProfile : Profile
{
    public ShFunctionMappingProfile()
    {
        CreateMap<FnTransferOpenOrderHeader, TransferOpenOrderHeaderDto>();
        CreateMap<FnTransferOpenOrderLine, TransferOpenOrderLineDto>();
    }
}
