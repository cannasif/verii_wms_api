using AutoMapper;
using Wms.Application.Common;
using Wms.Application.WarehouseOutbound.Dtos;

namespace Wms.Application.WarehouseOutbound.Services;

public sealed class WoFunctionService : IWoFunctionService
{
    private readonly IWoFunctionReadRepository _repo;
    private readonly ICurrentUserAccessor _currentUserAccessor;
    private readonly ILocalizationService _localizationService;
    private readonly IMapper _mapper;

    public WoFunctionService(IWoFunctionReadRepository repo, ICurrentUserAccessor currentUserAccessor, ILocalizationService localizationService, IMapper mapper)
    {
        _repo = repo;
        _currentUserAccessor = currentUserAccessor;
        _localizationService = localizationService;
        _mapper = mapper;
    }

    public async Task<ApiResponse<List<WoOpenOrderHeaderDto>>> GetWoOpenOrderHeaderAsync(string customerCode, CancellationToken cancellationToken = default)
    {
        var branchCode = string.IsNullOrWhiteSpace(_currentUserAccessor.BranchCode) ? "0" : _currentUserAccessor.BranchCode!.Trim();
        var rows = await _repo.GetHeaderRowsAsync(customerCode, branchCode, cancellationToken);
        return ApiResponse<List<WoOpenOrderHeaderDto>>.SuccessResult(_mapper.Map<List<WoOpenOrderHeaderDto>>(rows), _localizationService.GetLocalizedString("WoFunctionOpenOrderHeaderRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<List<WoOpenOrderLineDto>>> GetWoOpenOrderLineAsync(string siparisNoCsv, CancellationToken cancellationToken = default)
    {
        var branchCode = string.IsNullOrWhiteSpace(_currentUserAccessor.BranchCode) ? "0" : _currentUserAccessor.BranchCode!.Trim();
        var rows = await _repo.GetLineRowsAsync(siparisNoCsv, branchCode, cancellationToken);
        return ApiResponse<List<WoOpenOrderLineDto>>.SuccessResult(_mapper.Map<List<WoOpenOrderLineDto>>(rows), _localizationService.GetLocalizedString("WoFunctionOpenOrderLineRetrievedSuccessfully"));
    }
}
