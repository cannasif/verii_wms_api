using Microsoft.EntityFrameworkCore;
using AutoMapper;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Interfaces;
using WMS_WEBAPI.Models;
using Microsoft.AspNetCore.Http;
using WMS_WEBAPI.UnitOfWork;

namespace WMS_WEBAPI.Services
{
    public class WoFunctionService : IWoFunctionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILocalizationService _localizationService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public WoFunctionService(IUnitOfWork unitOfWork, IMapper mapper, ILocalizationService localizationService, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizationService = localizationService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ApiResponse<List<WoOpenOrderHeaderDto>>> GetWoOpenOrderHeaderAsync(string customerCode)
        {
            try
            {
                var branchCodeStr = _httpContextAccessor.HttpContext?.Items["BranchCode"] as string ?? "0";
                var rows = await _unitOfWork.SqlQuery<FN_WoOpenOrder_Header>("SELECT * FROM dbo.RII_FN_WO_HEADER({0}, {1})", customerCode, branchCodeStr)
                    .ToListAsync();
                var dtos = _mapper.Map<List<WoOpenOrderHeaderDto>>(rows);
                return ApiResponse<List<WoOpenOrderHeaderDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("WoFunctionOpenOrderHeaderRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<List<WoOpenOrderHeaderDto>>.ErrorResult(_localizationService.GetLocalizedString("WoFunctionOpenOrderHeaderRetrievalError"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<List<WoOpenOrderLineDto>>> GetWoOpenOrderLineAsync(string siparisNoCsv)
        {
            try
            {
                var branchCodeStr = _httpContextAccessor.HttpContext?.Items["BranchCode"] as string ?? "0";
                var rows = await _unitOfWork.SqlQuery<FN_WoOpenOrder_Line>("SELECT * FROM dbo.RII_FN_WO_LINE({0}, {1})", siparisNoCsv, branchCodeStr)
                    .ToListAsync();
                var dtos = _mapper.Map<List<WoOpenOrderLineDto>>(rows);
                return ApiResponse<List<WoOpenOrderLineDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("WoFunctionOpenOrderLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<List<WoOpenOrderLineDto>>.ErrorResult(_localizationService.GetLocalizedString("WoFunctionOpenOrderLineRetrievalError"), ex.Message ?? string.Empty, 500);
            }
        }
    }
}
