using AutoMapper;
using Wms.Application.Common;
using Wms.Application.SubcontractingReceiptTransfer.Dtos;

namespace Wms.Application.SubcontractingReceiptTransfer.Services;

public sealed class SrtFunctionService : ISrtFunctionService
{
    private readonly ISrtFunctionReadRepository _repo;
    private readonly ICurrentUserAccessor _currentUserAccessor;
    private readonly ILocalizationService _localizationService;
    private readonly IMapper _mapper;

    public SrtFunctionService(ISrtFunctionReadRepository repo, ICurrentUserAccessor currentUserAccessor, ILocalizationService localizationService, IMapper mapper)
    {
        _repo = repo;
        _currentUserAccessor = currentUserAccessor;
        _localizationService = localizationService;
        _mapper = mapper;
    }

    public async Task<ApiResponse<List<SrtOpenOrderHeaderDto>>> GetSrtOpenOrderHeaderAsync(string customerCode, CancellationToken cancellationToken = default)
    {
        var branchCode = string.IsNullOrWhiteSpace(_currentUserAccessor.BranchCode) ? "0" : _currentUserAccessor.BranchCode!.Trim();
        var rows = await _repo.GetHeaderRowsAsync(customerCode, branchCode, cancellationToken);
        return ApiResponse<List<SrtOpenOrderHeaderDto>>.SuccessResult(_mapper.Map<List<SrtOpenOrderHeaderDto>>(rows), _localizationService.GetLocalizedString("SrtFunctionOpenOrderHeaderRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<List<SrtOpenOrderLineDto>>> GetSrtOpenOrderLineAsync(string siparisNoCsv, CancellationToken cancellationToken = default)
    {
        var branchCode = string.IsNullOrWhiteSpace(_currentUserAccessor.BranchCode) ? "0" : _currentUserAccessor.BranchCode!.Trim();
        var rows = await _repo.GetLineRowsAsync(siparisNoCsv, branchCode, cancellationToken);
        return ApiResponse<List<SrtOpenOrderLineDto>>.SuccessResult(_mapper.Map<List<SrtOpenOrderLineDto>>(rows), _localizationService.GetLocalizedString("SrtFunctionOpenOrderLineRetrievedSuccessfully"));
    }
}
