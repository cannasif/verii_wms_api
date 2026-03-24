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
        private readonly IExecutionContextAccessor _executionContextAccessor;
        private readonly IRequestCancellationAccessor _requestCancellationAccessor;

        public SitFunctionService(IUnitOfWork unitOfWork, IMapper mapper, ILocalizationService localizationService, IExecutionContextAccessor executionContextAccessor, IRequestCancellationAccessor requestCancellationAccessor)
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


        public async Task<ApiResponse<List<SitOpenOrderHeaderDto>>> GetSitOpenOrderHeaderAsync(string customerCode, CancellationToken cancellationToken = default)
        {
            var requestCancellationToken = ResolveCancellationToken(cancellationToken);
var branchCodeStr = _executionContextAccessor.BranchCode ?? "0";
var rows = await _unitOfWork.SqlQuery<FN_SitOpenOrder_Header>("SELECT * FROM dbo.RII_FN_SIT_HEADER({0}, {1})", customerCode, branchCodeStr)
    .ToListAsync(requestCancellationToken);
var dtos = _mapper.Map<List<SitOpenOrderHeaderDto>>(rows);
return ApiResponse<List<SitOpenOrderHeaderDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("SitFunctionOpenOrderHeaderRetrievedSuccessfully"));
        }

        public async Task<ApiResponse<List<SitOpenOrderLineDto>>> GetSitOpenOrderLineAsync(string siparisNoCsv, CancellationToken cancellationToken = default)
        {
            var requestCancellationToken = ResolveCancellationToken(cancellationToken);
var branchCodeStr = _executionContextAccessor.BranchCode ?? "0";
var rows = await _unitOfWork.SqlQuery<FN_SitOpenOrder_Line>("SELECT * FROM dbo.RII_FN_SIT_LINE({0}, {1})", siparisNoCsv, branchCodeStr)
    .ToListAsync(requestCancellationToken);
var dtos = _mapper.Map<List<SitOpenOrderLineDto>>(rows);
return ApiResponse<List<SitOpenOrderLineDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("SitFunctionOpenOrderLineRetrievedSuccessfully"));
        }
    }
}
