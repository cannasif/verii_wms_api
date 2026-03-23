using Microsoft.EntityFrameworkCore;
using AutoMapper;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Interfaces;
using WMS_WEBAPI.Models;
using Microsoft.AspNetCore.Http;
using WMS_WEBAPI.UnitOfWork;

namespace WMS_WEBAPI.Services
{
    public class WiFunctionService : IWiFunctionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILocalizationService _localizationService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public WiFunctionService(IUnitOfWork unitOfWork, IMapper mapper, ILocalizationService localizationService, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizationService = localizationService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ApiResponse<List<WiOpenOrderHeaderDto>>> GetWiOpenOrderHeaderAsync(string customerCode)
        {
            try
            {
                var branchCodeStr = _httpContextAccessor.HttpContext?.Items["BranchCode"] as string ?? "0";
                var rows = await _unitOfWork.SqlQuery<FN_WiOpenOrder_Header>("SELECT * FROM dbo.RII_FN_WI_HEADER({0}, {1})", customerCode, branchCodeStr)
                    .ToListAsync();
                var dtos = _mapper.Map<List<WiOpenOrderHeaderDto>>(rows);
                return ApiResponse<List<WiOpenOrderHeaderDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("WiFunctionOpenOrderHeaderRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<List<WiOpenOrderHeaderDto>>.ErrorResult(_localizationService.GetLocalizedString("WiFunctionOpenOrderHeaderRetrievalError"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<List<WiOpenOrderLineDto>>> GetWiOpenOrderLineAsync(string siparisNoCsv)
        {
            try
            {
                var branchCodeStr = _httpContextAccessor.HttpContext?.Items["BranchCode"] as string ?? "0";
                var rows = await _unitOfWork.SqlQuery<FN_WiOpenOrder_Line>("SELECT * FROM dbo.RII_FN_WI_LINE({0}, {1})", siparisNoCsv, branchCodeStr)
                    .ToListAsync();
                var dtos = _mapper.Map<List<WiOpenOrderLineDto>>(rows);
                return ApiResponse<List<WiOpenOrderLineDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("WiFunctionOpenOrderLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<List<WiOpenOrderLineDto>>.ErrorResult(_localizationService.GetLocalizedString("WiFunctionOpenOrderLineRetrievalError"), ex.Message ?? string.Empty, 500);
            }
        }
    }
}
