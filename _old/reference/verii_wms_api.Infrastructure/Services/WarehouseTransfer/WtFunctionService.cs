using Microsoft.EntityFrameworkCore;
using AutoMapper;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Interfaces;
using WMS_WEBAPI.Models;
using Microsoft.AspNetCore.Http;
using WMS_WEBAPI.UnitOfWork;

namespace WMS_WEBAPI.Services
{
    public class WtFunctionService : IWtFunctionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILocalizationService _localizationService;
        private readonly ICurrentUserService _executionContextAccessor;
        private readonly IRequestCancellationAccessor _requestCancellationAccessor;

        public WtFunctionService(IUnitOfWork unitOfWork, IMapper mapper, ILocalizationService localizationService, ICurrentUserService executionContextAccessor, IRequestCancellationAccessor requestCancellationAccessor)
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


        public async Task<ApiResponse<List<TransferOpenOrderHeaderDto>>> GetTransferOpenOrderHeaderAsync(string customerCode, CancellationToken cancellationToken = default)
        {
            var requestCancellationToken = ResolveCancellationToken(cancellationToken);
var branchCodeStr = _executionContextAccessor.BranchCode ?? "0";
var headers = await _unitOfWork.SqlQuery<FN_TransferOpenOrder_Header>("SELECT * FROM dbo.RII_FN_WT_HEADER({0}, {1})", customerCode, branchCodeStr)
    .ToListAsync(requestCancellationToken);

var headerDtos = _mapper.Map<List<TransferOpenOrderHeaderDto>>(headers);

return ApiResponse<List<TransferOpenOrderHeaderDto>>.SuccessResult(
    headerDtos,
    _localizationService.GetLocalizedString("WtFunctionTransferOpenOrderHeaderRetrievedSuccessfully")
);
        }

        public async Task<ApiResponse<List<TransferOpenOrderLineDto>>> GetTransferOpenOrderLineAsync(string siparisNoCsv, CancellationToken cancellationToken = default)
        {
            var requestCancellationToken = ResolveCancellationToken(cancellationToken);
var branchCodeStr = _executionContextAccessor.BranchCode ?? "0";
var lines = await _unitOfWork.SqlQuery<FN_TransferOpenOrder_Line>("SELECT * FROM dbo.RII_FN_WT_LINE({0}, {1})", siparisNoCsv, branchCodeStr)
    .ToListAsync(requestCancellationToken);

var lineDtos = _mapper.Map<List<TransferOpenOrderLineDto>>(lines);

return ApiResponse<List<TransferOpenOrderLineDto>>.SuccessResult(
    lineDtos,
    _localizationService.GetLocalizedString("WtFunctionTransferOpenOrderLineRetrievedSuccessfully")
);
        }
    }
}
