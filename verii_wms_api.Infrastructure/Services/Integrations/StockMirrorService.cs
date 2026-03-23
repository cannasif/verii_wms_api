using Microsoft.EntityFrameworkCore;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Interfaces;
using WMS_WEBAPI.UnitOfWork;

namespace WMS_WEBAPI.Services
{
    public class StockMirrorService : IStockMirrorService
    {
        private readonly IUnitOfWork _unitOfWork;

        public StockMirrorService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ApiResponse<PagedResponse<StockPagedDto>>> GetPagedAsync(PagedRequest request)
        {
            try
            {
                request ??= new PagedRequest();
                request.Filters ??= new List<Filter>();

                var columnMapping = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                {
                    { "code", nameof(Models.Stock.ErpStockCode) },
                    { "name", nameof(Models.Stock.StockName) }
                };

                var query = _unitOfWork.Stocks.Query()
                    .ApplySearch(
                        request.Search,
                        nameof(Models.Stock.ErpStockCode),
                        nameof(Models.Stock.StockName),
                        nameof(Models.Stock.Unit),
                        nameof(Models.Stock.UreticiKodu),
                        nameof(Models.Stock.GrupKodu),
                        nameof(Models.Stock.GrupAdi),
                        nameof(Models.Stock.Kod1),
                        nameof(Models.Stock.Kod1Adi),
                        nameof(Models.Stock.Kod2),
                        nameof(Models.Stock.Kod2Adi),
                        nameof(Models.Stock.Kod3),
                        nameof(Models.Stock.Kod3Adi),
                        nameof(Models.Stock.Kod4),
                        nameof(Models.Stock.Kod4Adi),
                        nameof(Models.Stock.Kod5),
                        nameof(Models.Stock.Kod5Adi))
                    .ApplyFilters(request.Filters, request.FilterLogic, columnMapping)
                    .ApplySorting(request.SortBy ?? nameof(Models.Stock.Id), string.Equals(request.SortDirection, "desc", StringComparison.OrdinalIgnoreCase), columnMapping);

                var totalCount = await query.CountAsync();
                var items = await query
                    .ApplyPagination(request.PageNumber, request.PageSize)
                    .Select(x => new StockPagedDto
                    {
                        Id = x.Id,
                        ErpStockCode = x.ErpStockCode,
                        StockName = x.StockName,
                        Unit = x.Unit,
                        UreticiKodu = x.UreticiKodu,
                        GrupKodu = x.GrupKodu,
                        GrupAdi = x.GrupAdi,
                        Kod1 = x.Kod1,
                        Kod1Adi = x.Kod1Adi,
                        Kod2 = x.Kod2,
                        Kod2Adi = x.Kod2Adi,
                        Kod3 = x.Kod3,
                        Kod3Adi = x.Kod3Adi,
                        Kod4 = x.Kod4,
                        Kod4Adi = x.Kod4Adi,
                        Kod5 = x.Kod5,
                        Kod5Adi = x.Kod5Adi,
                        BranchCode = x.BranchCode,
                        LastSyncDate = x.LastSyncDate,
                        CreatedDate = x.CreatedDate
                    })
                    .ToListAsync();

                var paged = new PagedResponse<StockPagedDto>(items, totalCount, request.PageNumber, request.PageSize);
                return ApiResponse<PagedResponse<StockPagedDto>>.SuccessResult(paged, "Stock mirror records retrieved successfully.");
            }
            catch (Exception ex)
            {
                return ApiResponse<PagedResponse<StockPagedDto>>.ErrorResult("Stock mirror records could not be retrieved.", ex.Message, 500);
            }
        }
    }
}
