using Microsoft.EntityFrameworkCore;
using AutoMapper;
using WMS_WEBAPI.Data;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Interfaces;
using WMS_WEBAPI.Models;
using Microsoft.AspNetCore.Http;

namespace WMS_WEBAPI.Services
{
    public class GoodReciptFunctionsService : IGoodReciptFunctionsService
    {
        private readonly WmsDbContext _wmsDbContext;
        private readonly IMapper _mapper;
        private readonly ILocalizationService _localizationService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GoodReciptFunctionsService(WmsDbContext wmsDbContext, IMapper mapper, ILocalizationService localizationService, IHttpContextAccessor httpContextAccessor)
        {
            _wmsDbContext = wmsDbContext;
            _mapper = mapper;
            _localizationService = localizationService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ApiResponse<List<GoodsOpenOrdersHeaderDto>>> GetGoodsReceiptHeaderAsync(string customerCode)
        {
            try
            {
                var branchCodeStr = _httpContextAccessor.HttpContext?.Items["BranchCode"] as string ?? "0";
                
                var headers = await _wmsDbContext.Set<FN_GoodsOpenOrders_Header>()
                    .FromSqlRaw("SELECT * FROM dbo.RII_FN_GR_OPENORDERS_HEADER({0}, {1})", customerCode, branchCodeStr)
                    .AsNoTracking()
                    .ToListAsync();

                var headerDtos = _mapper.Map<List<GoodsOpenOrdersHeaderDto>>(headers);

                return ApiResponse<List<GoodsOpenOrdersHeaderDto>>.SuccessResult(headerDtos, _localizationService.GetLocalizedString("GoodReciptFunctionsHeaderRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<List<GoodsOpenOrdersHeaderDto>>.ErrorResult(_localizationService.GetLocalizedString("GoodReciptFunctionsHeaderRetrievalError"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<List<GoodsOpenOrdersLineDto>>> GetGoodsReceiptLineAsync(string siparisNoCsv, string customerCode)
        {
            try
            {
                var branchCodeStr = _httpContextAccessor.HttpContext?.Items["BranchCode"] as string ?? "0";
                var ordersCsv = string.IsNullOrWhiteSpace(siparisNoCsv) ? "" : siparisNoCsv;
                var lines = await _wmsDbContext.Set<FN_GoodsOpenOrders_Line>()
                    .FromSqlRaw("SELECT * FROM dbo.RII_FN_GR_OPENORDERS_LINE({0}, {1}, {2})", ordersCsv, customerCode, branchCodeStr)
                    .AsNoTracking()
                    .ToListAsync();

                var lineDtos = _mapper.Map<List<GoodsOpenOrdersLineDto>>(lines);

                return ApiResponse<List<GoodsOpenOrdersLineDto>>.SuccessResult(lineDtos, _localizationService.GetLocalizedString("GoodReciptFunctionsLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<List<GoodsOpenOrdersLineDto>>.ErrorResult(_localizationService.GetLocalizedString("GoodReciptFunctionsLineRetrievalError"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<List<GoodsOpenOrdersLineDto>>> GetGoodsReceiptLineByCustomerCodeAndBranchCodeAsync(string branchCode, string customerCode)
        {
            try
            {
                var lines = await _wmsDbContext.Set<FN_GoodsOpenOrders_Line>()
                    .FromSqlRaw("SELECT * FROM dbo.RII_FN_GR_OPENORDERS_LINE({0}, {1}, {2})", null, customerCode, branchCode)
                    .AsNoTracking()
                    .ToListAsync();

                var lineDtos = _mapper.Map<List<GoodsOpenOrdersLineDto>>(lines);

                return ApiResponse<List<GoodsOpenOrdersLineDto>>.SuccessResult(lineDtos, _localizationService.GetLocalizedString("GoodReciptFunctionsLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<List<GoodsOpenOrdersLineDto>>.ErrorResult(_localizationService.GetLocalizedString("GoodReciptFunctionsLineRetrievalError"), ex.Message ?? string.Empty, 500);
            }
        }
    }
}
