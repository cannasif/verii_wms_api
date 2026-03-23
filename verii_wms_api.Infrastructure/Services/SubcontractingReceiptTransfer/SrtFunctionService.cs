using Microsoft.EntityFrameworkCore;
using AutoMapper;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Interfaces;
using WMS_WEBAPI.Models;
using Microsoft.AspNetCore.Http;
using WMS_WEBAPI.UnitOfWork;

namespace WMS_WEBAPI.Services
{
    public class SrtFunctionService : ISrtFunctionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILocalizationService _localizationService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SrtFunctionService(IUnitOfWork unitOfWork, IMapper mapper, ILocalizationService localizationService, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizationService = localizationService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ApiResponse<List<SrtOpenOrderHeaderDto>>> GetSrtOpenOrderHeaderAsync(string customerCode)
        {
            try
            {
                var branchCodeStr = _httpContextAccessor.HttpContext?.Items["BranchCode"] as string ?? "0";
                var rows = await _unitOfWork.SqlQuery<FN_SrtOpenOrder_Header>("SELECT * FROM dbo.RII_FN_SRT_HEADER({0}, {1})", customerCode, branchCodeStr)
                    .ToListAsync();
                var dtos = _mapper.Map<List<SrtOpenOrderHeaderDto>>(rows);
                return ApiResponse<List<SrtOpenOrderHeaderDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("SrtFunctionOpenOrderHeaderRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<List<SrtOpenOrderHeaderDto>>.ErrorResult(_localizationService.GetLocalizedString("SrtFunctionOpenOrderHeaderRetrievalError"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<List<SrtOpenOrderLineDto>>> GetSrtOpenOrderLineAsync(string siparisNoCsv)
        {
            try
            {
                var branchCodeStr = _httpContextAccessor.HttpContext?.Items["BranchCode"] as string ?? "0";
                var rows = await _unitOfWork.SqlQuery<FN_SrtOpenOrder_Line>("SELECT * FROM dbo.RII_FN_SRT_LINE({0}, {1})", siparisNoCsv, branchCodeStr)
                    .ToListAsync();
                var dtos = _mapper.Map<List<SrtOpenOrderLineDto>>(rows);
                return ApiResponse<List<SrtOpenOrderLineDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("SrtFunctionOpenOrderLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<List<SrtOpenOrderLineDto>>.ErrorResult(_localizationService.GetLocalizedString("SrtFunctionOpenOrderLineRetrievalError"), ex.Message ?? string.Empty, 500);
            }
        }
    }
}
