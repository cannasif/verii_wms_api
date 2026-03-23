using Microsoft.EntityFrameworkCore;
using AutoMapper;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Interfaces;
using WMS_WEBAPI.Models;
using Microsoft.AspNetCore.Http;
using WMS_WEBAPI.UnitOfWork;

namespace WMS_WEBAPI.Services
{
    public class SitFunctionService : ISitFunctionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILocalizationService _localizationService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SitFunctionService(IUnitOfWork unitOfWork, IMapper mapper, ILocalizationService localizationService, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizationService = localizationService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ApiResponse<List<SitOpenOrderHeaderDto>>> GetSitOpenOrderHeaderAsync(string customerCode)
        {
            try
            {
                var branchCodeStr = _httpContextAccessor.HttpContext?.Items["BranchCode"] as string ?? "0";
                var rows = await _unitOfWork.SqlQuery<FN_SitOpenOrder_Header>("SELECT * FROM dbo.RII_FN_SIT_HEADER({0}, {1})", customerCode, branchCodeStr)
                    .ToListAsync();
                var dtos = _mapper.Map<List<SitOpenOrderHeaderDto>>(rows);
                return ApiResponse<List<SitOpenOrderHeaderDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("SitFunctionOpenOrderHeaderRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<List<SitOpenOrderHeaderDto>>.ErrorResult(_localizationService.GetLocalizedString("SitFunctionOpenOrderHeaderRetrievalError"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<List<SitOpenOrderLineDto>>> GetSitOpenOrderLineAsync(string siparisNoCsv)
        {
            try
            {
                var branchCodeStr = _httpContextAccessor.HttpContext?.Items["BranchCode"] as string ?? "0";
                var rows = await _unitOfWork.SqlQuery<FN_SitOpenOrder_Line>("SELECT * FROM dbo.RII_FN_SIT_LINE({0}, {1})", siparisNoCsv, branchCodeStr)
                    .ToListAsync();
                var dtos = _mapper.Map<List<SitOpenOrderLineDto>>(rows);
                return ApiResponse<List<SitOpenOrderLineDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("SitFunctionOpenOrderLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<List<SitOpenOrderLineDto>>.ErrorResult(_localizationService.GetLocalizedString("SitFunctionOpenOrderLineRetrievalError"), ex.Message ?? string.Empty, 500);
            }
        }
    }
}
