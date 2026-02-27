using AutoMapper;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Models;

namespace WMS_WEBAPI.Mappings
{
    public class GoodReciptFunctionsMappingProfile : Profile
    {
        public GoodReciptFunctionsMappingProfile()
        {
            CreateMap<FN_GoodsOpenOrders_Header, GoodsOpenOrdersHeaderDto>();
            CreateMap<FN_GoodsOpenOrders_Line, GoodsOpenOrdersLineDto>();
        }
    }
}