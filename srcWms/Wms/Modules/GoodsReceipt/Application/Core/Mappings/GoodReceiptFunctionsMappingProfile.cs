using AutoMapper;
using Wms.Application.GoodsReceipt.Dtos;
using Wms.Domain.Entities.GoodsReceipt.Functions;

namespace Wms.Application.GoodsReceipt.Mappings;

public sealed class GoodReceiptFunctionsMappingProfile : Profile
{
    public GoodReceiptFunctionsMappingProfile()
    {
        CreateMap<FnGoodsOpenOrdersHeader, GoodsOpenOrdersHeaderDto>();
        CreateMap<FnGoodsOpenOrdersLine, GoodsOpenOrdersLineDto>();
    }
}
