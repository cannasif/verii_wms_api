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
        private readonly IExecutionContextAccessor _executionContextAccessor;
        private readonly IRequestCancellationAccessor _requestCancellationAccessor;

        public WoFunctionService(IUnitOfWork unitOfWork, IMapper mapper, ILocalizationService localizationService, IExecutionContextAccessor executionContextAccessor, IRequestCancellationAccessor requestCancellationAccessor)
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


        public async Task<ApiResponse<List<WoOpenOrderHeaderDto>>> GetWoOpenOrderHeaderAsync(string customerCode, CancellationToken cancellationToken = default)
        {
            var requestCancellationToken = ResolveCancellationToken(cancellationToken);
var branchCodeStr = _executionContextAccessor.BranchCode ?? "0";
var rows = await _unitOfWork.SqlQuery<FN_WoOpenOrder_Header>("SELECT * FROM dbo.RII_FN_WO_HEADER({0}, {1})", customerCode, branchCodeStr)
    .ToListAsync(requestCancellationToken);
var dtos = _mapper.Map<List<WoOpenOrderHeaderDto>>(rows);
return ApiResponse<List<WoOpenOrderHeaderDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("WoFunctionOpenOrderHeaderRetrievedSuccessfully"));
        }

        public async Task<ApiResponse<List<WoOpenOrderLineDto>>> GetWoOpenOrderLineAsync(string siparisNoCsv, CancellationToken cancellationToken = default)
        {
            var requestCancellationToken = ResolveCancellationToken(cancellationToken);
var branchCodeStr = _executionContextAccessor.BranchCode ?? "0";
var rows = await _unitOfWork.SqlQuery<FN_WoOpenOrder_Line>("SELECT * FROM dbo.RII_FN_WO_LINE({0}, {1})", siparisNoCsv, branchCodeStr)
    .ToListAsync(requestCancellationToken);
var dtos = _mapper.Map<List<WoOpenOrderLineDto>>(rows);
return ApiResponse<List<WoOpenOrderLineDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("WoFunctionOpenOrderLineRetrievedSuccessfully"));
        }
    }
}
