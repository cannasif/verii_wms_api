using AutoMapper;
using Wms.Application.Stock.Dtos;
using StockEntity = Wms.Domain.Entities.Stock.Stock;

namespace Wms.Application.Stock.Mappings;

public sealed class StockMappingProfile : Profile
{
    public StockMappingProfile()
    {
        CreateMap<StockEntity, StockDto>();
        CreateMap<CreateStockDto, StockEntity>();
        CreateMap<UpdateStockDto, StockEntity>();
        CreateMap<SyncStockDto, StockEntity>();
    }
}
