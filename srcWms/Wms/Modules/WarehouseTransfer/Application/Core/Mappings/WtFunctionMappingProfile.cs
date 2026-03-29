using AutoMapper;
using Wms.Application.WarehouseTransfer.Dtos;
using Wms.Domain.Entities.WarehouseTransfer.Functions;

namespace Wms.Application.WarehouseTransfer.Mappings;

public sealed class WtFunctionMappingProfile : Profile
{
    public WtFunctionMappingProfile()
    {
        CreateMap<FnTransferOpenOrderHeader, TransferOpenOrderHeaderDto>();
        CreateMap<FnTransferOpenOrderLine, TransferOpenOrderLineDto>();
    }
}
