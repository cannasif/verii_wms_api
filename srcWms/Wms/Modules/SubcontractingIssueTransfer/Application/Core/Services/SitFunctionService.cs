using AutoMapper;
using Wms.Application.Common;
using Wms.Application.SubcontractingIssueTransfer.Dtos;

namespace Wms.Application.SubcontractingIssueTransfer.Services;

public sealed class SitFunctionService : ISitFunctionService
{
    private readonly ISitFunctionReadRepository _repo;
    private readonly ICurrentUserAccessor _currentUserAccessor;
    private readonly ILocalizationService _localizationService;
    private readonly IMapper _mapper;

    public SitFunctionService(ISitFunctionReadRepository repo, ICurrentUserAccessor currentUserAccessor, ILocalizationService localizationService, IMapper mapper)
    {
        _repo = repo;
        _currentUserAccessor = currentUserAccessor;
        _localizationService = localizationService;
        _mapper = mapper;
    }

    public async Task<ApiResponse<List<SitOpenOrderHeaderDto>>> GetSitOpenOrderHeaderAsync(string customerCode, CancellationToken cancellationToken = default)
    {
        var branchCode = string.IsNullOrWhiteSpace(_currentUserAccessor.BranchCode) ? "0" : _currentUserAccessor.BranchCode!.Trim();
        var rows = await _repo.GetHeaderRowsAsync(customerCode, branchCode, cancellationToken);
        return ApiResponse<List<SitOpenOrderHeaderDto>>.SuccessResult(_mapper.Map<List<SitOpenOrderHeaderDto>>(rows), _localizationService.GetLocalizedString("SitFunctionOpenOrderHeaderRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<List<SitOpenOrderLineDto>>> GetSitOpenOrderLineAsync(string siparisNoCsv, CancellationToken cancellationToken = default)
    {
        var branchCode = string.IsNullOrWhiteSpace(_currentUserAccessor.BranchCode) ? "0" : _currentUserAccessor.BranchCode!.Trim();
        var rows = await _repo.GetLineRowsAsync(siparisNoCsv, branchCode, cancellationToken);
        return ApiResponse<List<SitOpenOrderLineDto>>.SuccessResult(_mapper.Map<List<SitOpenOrderLineDto>>(rows), _localizationService.GetLocalizedString("SitFunctionOpenOrderLineRetrievedSuccessfully"));
    }
}
