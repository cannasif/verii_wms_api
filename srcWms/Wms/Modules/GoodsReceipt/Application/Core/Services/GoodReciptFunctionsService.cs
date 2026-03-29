using AutoMapper;
using Wms.Application.Common;
using Wms.Application.GoodsReceipt.Dtos;

namespace Wms.Application.GoodsReceipt.Services;

public sealed class GoodReciptFunctionsService : IGoodReciptFunctionsService
{
    private readonly IGoodsReceiptOpenOrderReadRepository _openOrderReadRepository;
    private readonly ICurrentUserAccessor _currentUserAccessor;
    private readonly ILocalizationService _localizationService;
    private readonly IMapper _mapper;

    public GoodReciptFunctionsService(
        IGoodsReceiptOpenOrderReadRepository openOrderReadRepository,
        ICurrentUserAccessor currentUserAccessor,
        ILocalizationService localizationService,
        IMapper mapper)
    {
        _openOrderReadRepository = openOrderReadRepository;
        _currentUserAccessor = currentUserAccessor;
        _localizationService = localizationService;
        _mapper = mapper;
    }

    public async Task<ApiResponse<List<GoodsOpenOrdersHeaderDto>>> GetGoodsReceiptHeaderAsync(string customerCode, CancellationToken cancellationToken = default)
    {
        var branchCode = NormalizeBranchCode(_currentUserAccessor.BranchCode);
        var headers = await _openOrderReadRepository.GetOpenOrderHeadersAsync(customerCode, branchCode, cancellationToken);
        var dtos = _mapper.Map<List<GoodsOpenOrdersHeaderDto>>(headers);
        return ApiResponse<List<GoodsOpenOrdersHeaderDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("GoodReciptFunctionsHeaderRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<List<GoodsOpenOrdersLineDto>>> GetGoodsReceiptLineAsync(string siparisNoCsv, string customerCode, CancellationToken cancellationToken = default)
    {
        var branchCode = NormalizeBranchCode(_currentUserAccessor.BranchCode);
        var ordersCsv = string.IsNullOrWhiteSpace(siparisNoCsv) ? string.Empty : siparisNoCsv.Trim();
        var lines = await _openOrderReadRepository.GetOpenOrderLinesAsync(ordersCsv, customerCode, branchCode, cancellationToken);
        var dtos = _mapper.Map<List<GoodsOpenOrdersLineDto>>(lines);
        return ApiResponse<List<GoodsOpenOrdersLineDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("GoodReciptFunctionsLineRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<List<GoodsOpenOrdersLineDto>>> GetGoodsReceiptLineByCustomerCodeAndBranchCodeAsync(string branchCode, string customerCode, CancellationToken cancellationToken = default)
    {
        var normalizedBranchCode = NormalizeBranchCode(branchCode);
        var lines = await _openOrderReadRepository.GetOpenOrderLinesAsync(string.Empty, customerCode, normalizedBranchCode, cancellationToken);
        var dtos = _mapper.Map<List<GoodsOpenOrdersLineDto>>(lines);
        return ApiResponse<List<GoodsOpenOrdersLineDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("GoodReciptFunctionsLineRetrievedSuccessfully"));
    }

    private static string NormalizeBranchCode(string? branchCode)
    {
        return string.IsNullOrWhiteSpace(branchCode) ? "0" : branchCode.Trim();
    }
}
