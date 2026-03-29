using AutoMapper;
using Wms.Application.Common;
using Wms.Application.Shipping.Dtos;

namespace Wms.Application.Shipping.Services;

public sealed class ShFunctionService : IShFunctionService
{
    private readonly IShFunctionReadRepository _repo;
    private readonly ICurrentUserAccessor _currentUserAccessor;
    private readonly ILocalizationService _localizationService;
    private readonly IMapper _mapper;

    public ShFunctionService(IShFunctionReadRepository repo, ICurrentUserAccessor currentUserAccessor, ILocalizationService localizationService, IMapper mapper)
    {
        _repo = repo;
        _currentUserAccessor = currentUserAccessor;
        _localizationService = localizationService;
        _mapper = mapper;
    }

    public async Task<ApiResponse<List<TransferOpenOrderHeaderDto>>> GetShippingOpenOrderHeaderAsync(string customerCode, CancellationToken cancellationToken = default)
    {
        var branchCode = string.IsNullOrWhiteSpace(_currentUserAccessor.BranchCode) ? "0" : _currentUserAccessor.BranchCode!.Trim();
        var rows = await _repo.GetHeaderRowsAsync(customerCode, branchCode, cancellationToken);
        return ApiResponse<List<TransferOpenOrderHeaderDto>>.SuccessResult(_mapper.Map<List<TransferOpenOrderHeaderDto>>(rows), _localizationService.GetLocalizedString("ShFunctionOpenOrderHeaderRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<List<TransferOpenOrderLineDto>>> GetShippingOpenOrderLineAsync(string siparisNoCsv, CancellationToken cancellationToken = default)
    {
        var branchCode = string.IsNullOrWhiteSpace(_currentUserAccessor.BranchCode) ? "0" : _currentUserAccessor.BranchCode!.Trim();
        var rows = await _repo.GetLineRowsAsync(siparisNoCsv, branchCode, cancellationToken);
        return ApiResponse<List<TransferOpenOrderLineDto>>.SuccessResult(_mapper.Map<List<TransferOpenOrderLineDto>>(rows), _localizationService.GetLocalizedString("ShFunctionOpenOrderLineRetrievedSuccessfully"));
    }
}
