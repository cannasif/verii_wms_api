using Microsoft.EntityFrameworkCore;
using AutoMapper;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Interfaces;
using WMS_WEBAPI.Models;
using Microsoft.AspNetCore.Http;
using WMS_WEBAPI.UnitOfWork;

namespace WMS_WEBAPI.Services
{
    public class GoodReciptFunctionsService : IGoodReciptFunctionsService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILocalizationService _localizationService;
        private readonly ICurrentUserService _executionContextAccessor;
        private readonly IRequestCancellationAccessor _requestCancellationAccessor;

        public GoodReciptFunctionsService(IUnitOfWork unitOfWork, IMapper mapper, ILocalizationService localizationService, ICurrentUserService executionContextAccessor, IRequestCancellationAccessor requestCancellationAccessor)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizationService = localizationService;
            _executionContextAccessor = executionContextAccessor;
            _requestCancellationAccessor = requestCancellationAccessor;
        }
        private CancellationToken ResolveCancellationToken(CancellationToken token = default)
        {
            return _requestCancellationAccessor.Get(token);
        }

        private CancellationToken RequestCancellationToken => ResolveCancellationToken();


        public async Task<ApiResponse<List<GoodsOpenOrdersHeaderDto>>> GetGoodsReceiptHeaderAsync(string customerCode, CancellationToken cancellationToken = default)
        {
            var requestCancellationToken = ResolveCancellationToken(cancellationToken);
var branchCodeStr = _executionContextAccessor.BranchCode ?? "0";

var headers = await _unitOfWork.SqlQuery<FN_GoodsOpenOrders_Header>("SELECT * FROM dbo.RII_FN_GR_OPENORDERS_HEADER({0}, {1})", customerCode, branchCodeStr)
    .ToListAsync(requestCancellationToken);

var headerDtos = _mapper.Map<List<GoodsOpenOrdersHeaderDto>>(headers);

return ApiResponse<List<GoodsOpenOrdersHeaderDto>>.SuccessResult(headerDtos, _localizationService.GetLocalizedString("GoodReciptFunctionsHeaderRetrievedSuccessfully"));
        }

        public async Task<ApiResponse<List<GoodsOpenOrdersLineDto>>> GetGoodsReceiptLineAsync(string siparisNoCsv, string customerCode, CancellationToken cancellationToken = default)
        {
            var requestCancellationToken = ResolveCancellationToken(cancellationToken);
var branchCodeStr = _executionContextAccessor.BranchCode ?? "0";
var ordersCsv = string.IsNullOrWhiteSpace(siparisNoCsv) ? "" : siparisNoCsv;
var lines = await _unitOfWork.SqlQuery<FN_GoodsOpenOrders_Line>("SELECT * FROM dbo.RII_FN_GR_OPENORDERS_LINE({0}, {1}, {2})", ordersCsv, customerCode, branchCodeStr)
    .ToListAsync(requestCancellationToken);

var lineDtos = _mapper.Map<List<GoodsOpenOrdersLineDto>>(lines);

return ApiResponse<List<GoodsOpenOrdersLineDto>>.SuccessResult(lineDtos, _localizationService.GetLocalizedString("GoodReciptFunctionsLineRetrievedSuccessfully"));
        }

        public async Task<ApiResponse<List<GoodsOpenOrdersLineDto>>> GetGoodsReceiptLineByCustomerCodeAndBranchCodeAsync(string branchCode, string customerCode, CancellationToken cancellationToken = default)
        {
            var requestCancellationToken = ResolveCancellationToken(cancellationToken);
var lines = await _unitOfWork.SqlQuery<FN_GoodsOpenOrders_Line>("SELECT * FROM dbo.RII_FN_GR_OPENORDERS_LINE({0}, {1}, {2})", null!, customerCode, branchCode)
    .ToListAsync(requestCancellationToken);

var lineDtos = _mapper.Map<List<GoodsOpenOrdersLineDto>>(lines);

return ApiResponse<List<GoodsOpenOrdersLineDto>>.SuccessResult(lineDtos, _localizationService.GetLocalizedString("GoodReciptFunctionsLineRetrievedSuccessfully"));
        }
    }
}
