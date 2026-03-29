using AutoMapper;
using Wms.Application.Common;
using Wms.Application.WarehouseInbound.Dtos;

namespace Wms.Application.WarehouseInbound.Services;

public sealed class WiFunctionService : IWiFunctionService
{
    private readonly IWiFunctionReadRepository _repo;
    private readonly ICurrentUserAccessor _currentUserAccessor;
    private readonly ILocalizationService _localizationService;
    private readonly IMapper _mapper;

    public WiFunctionService(IWiFunctionReadRepository repo, ICurrentUserAccessor currentUserAccessor, ILocalizationService localizationService, IMapper mapper)
    {
        _repo = repo;
        _currentUserAccessor = currentUserAccessor;
        _localizationService = localizationService;
        _mapper = mapper;
    }

    public async Task<ApiResponse<List<WiOpenOrderHeaderDto>>> GetWiOpenOrderHeaderAsync(string customerCode, CancellationToken cancellationToken = default)
    {
        var branchCode = string.IsNullOrWhiteSpace(_currentUserAccessor.BranchCode) ? "0" : _currentUserAccessor.BranchCode!.Trim();
        var rows = await _repo.GetHeaderRowsAsync(customerCode, branchCode, cancellationToken);
        return ApiResponse<List<WiOpenOrderHeaderDto>>.SuccessResult(_mapper.Map<List<WiOpenOrderHeaderDto>>(rows), _localizationService.GetLocalizedString("WiFunctionOpenOrderHeaderRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<List<WiOpenOrderLineDto>>> GetWiOpenOrderLineAsync(string siparisNoCsv, CancellationToken cancellationToken = default)
    {
        var branchCode = string.IsNullOrWhiteSpace(_currentUserAccessor.BranchCode) ? "0" : _currentUserAccessor.BranchCode!.Trim();
        var rows = await _repo.GetLineRowsAsync(siparisNoCsv, branchCode, cancellationToken);
        return ApiResponse<List<WiOpenOrderLineDto>>.SuccessResult(_mapper.Map<List<WiOpenOrderLineDto>>(rows), _localizationService.GetLocalizedString("WiFunctionOpenOrderLineRetrievedSuccessfully"));
    }
}
