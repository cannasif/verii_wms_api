using Microsoft.EntityFrameworkCore;
using AutoMapper;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Interfaces;
using WMS_WEBAPI.Models;
using Microsoft.AspNetCore.Http;
using WMS_WEBAPI.UnitOfWork;

namespace WMS_WEBAPI.Services
{
    public class ShFunctionService : IShFunctionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILocalizationService _localizationService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IRequestCancellationAccessor _requestCancellationAccessor;

        public ShFunctionService(IUnitOfWork unitOfWork, IMapper mapper, ILocalizationService localizationService, IHttpContextAccessor httpContextAccessor, IRequestCancellationAccessor requestCancellationAccessor)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizationService = localizationService;
            _httpContextAccessor = httpContextAccessor;
            _requestCancellationAccessor = requestCancellationAccessor;
        }
        private CancellationToken ResolveCancellationToken(CancellationToken token = default)
        {
            return _requestCancellationAccessor.Get(token);
        }

        private CancellationToken RequestCancellationToken => ResolveCancellationToken();


        public async Task<ApiResponse<List<TransferOpenOrderHeaderDto>>> GetShippingOpenOrderHeaderAsync(string customerCode, CancellationToken cancellationToken = default)
        {
            var requestCancellationToken = ResolveCancellationToken(cancellationToken);
            try
            {
                var branchCodeStr = _httpContextAccessor.HttpContext?.Items["BranchCode"] as string ?? "0";
                var rows = await _unitOfWork.SqlQuery<FN_ShOpenOrder_Header>("SELECT * FROM dbo.RII_FN_SH_HEADER({0}, {1})", customerCode, branchCodeStr)
                    .ToListAsync(requestCancellationToken);
                var dtos = _mapper.Map<List<TransferOpenOrderHeaderDto>>(rows);
                return ApiResponse<List<TransferOpenOrderHeaderDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("ShFunctionOpenOrderHeaderRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<List<TransferOpenOrderHeaderDto>>.ErrorResult(_localizationService.GetLocalizedString("ShFunctionOpenOrderHeaderRetrievalError"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<List<TransferOpenOrderLineDto>>> GetShippingOpenOrderLineAsync(string siparisNoCsv, CancellationToken cancellationToken = default)
        {
            var requestCancellationToken = ResolveCancellationToken(cancellationToken);
            try
            {
                var branchCodeStr = _httpContextAccessor.HttpContext?.Items["BranchCode"] as string ?? "0";
                var rows = await _unitOfWork.SqlQuery<FN_ShOpenOrder_Line>("SELECT * FROM dbo.RII_FN_SH_LINE({0}, {1})", siparisNoCsv, branchCodeStr)
                    .ToListAsync(requestCancellationToken);
                var dtos = _mapper.Map<List<TransferOpenOrderLineDto>>(rows);
                return ApiResponse<List<TransferOpenOrderLineDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("ShFunctionOpenOrderLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<List<TransferOpenOrderLineDto>>.ErrorResult(_localizationService.GetLocalizedString("ShFunctionOpenOrderLineRetrievalError"), ex.Message ?? string.Empty, 500);
            }
        }
    }
}
