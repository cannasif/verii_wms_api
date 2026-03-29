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
        private readonly ICurrentUserService _executionContextAccessor;
        private readonly IRequestCancellationAccessor _requestCancellationAccessor;

        public SrtFunctionService(IUnitOfWork unitOfWork, IMapper mapper, ILocalizationService localizationService, ICurrentUserService executionContextAccessor, IRequestCancellationAccessor requestCancellationAccessor)
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


        public async Task<ApiResponse<List<SrtOpenOrderHeaderDto>>> GetSrtOpenOrderHeaderAsync(string customerCode, CancellationToken cancellationToken = default)
        {
            var requestCancellationToken = ResolveCancellationToken(cancellationToken);
var branchCodeStr = _executionContextAccessor.BranchCode ?? "0";
var rows = await _unitOfWork.SqlQuery<FN_SrtOpenOrder_Header>("SELECT * FROM dbo.RII_FN_SRT_HEADER({0}, {1})", customerCode, branchCodeStr)
    .ToListAsync(requestCancellationToken);
var dtos = _mapper.Map<List<SrtOpenOrderHeaderDto>>(rows);
return ApiResponse<List<SrtOpenOrderHeaderDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("SrtFunctionOpenOrderHeaderRetrievedSuccessfully"));
        }

        public async Task<ApiResponse<List<SrtOpenOrderLineDto>>> GetSrtOpenOrderLineAsync(string siparisNoCsv, CancellationToken cancellationToken = default)
        {
            var requestCancellationToken = ResolveCancellationToken(cancellationToken);
var branchCodeStr = _executionContextAccessor.BranchCode ?? "0";
var rows = await _unitOfWork.SqlQuery<FN_SrtOpenOrder_Line>("SELECT * FROM dbo.RII_FN_SRT_LINE({0}, {1})", siparisNoCsv, branchCodeStr)
    .ToListAsync(requestCancellationToken);
var dtos = _mapper.Map<List<SrtOpenOrderLineDto>>(rows);
return ApiResponse<List<SrtOpenOrderLineDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("SrtFunctionOpenOrderLineRetrievedSuccessfully"));
        }
    }
}
