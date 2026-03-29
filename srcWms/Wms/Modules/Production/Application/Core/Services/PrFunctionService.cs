using AutoMapper;
using Wms.Application.Common;
using Wms.Application.Production.Dtos;

namespace Wms.Application.Production.Services;

public sealed class PrFunctionService : IPrFunctionService
{
    private readonly IPrFunctionReadRepository _repo;
    private readonly ILocalizationService _localizationService;
    private readonly IMapper _mapper;

    public PrFunctionService(IPrFunctionReadRepository repo, ILocalizationService localizationService, IMapper mapper)
    {
        _repo = repo;
        _localizationService = localizationService;
        _mapper = mapper;
    }

    public async Task<ApiResponse<List<ProductHeaderDto>>> GetProductHeaderAsync(string isemriNo, CancellationToken cancellationToken = default)
    {
        var rows = await _repo.GetProductHeaderRowsAsync(isemriNo, cancellationToken);
        return ApiResponse<List<ProductHeaderDto>>.SuccessResult(_mapper.Map<List<ProductHeaderDto>>(rows), _localizationService.GetLocalizedString("ProductHeaderRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<List<ProductLineDto>>> GetProductLinesAsync(string? isemriNo = null, string? fisNo = null, string? mamulKodu = null, CancellationToken cancellationToken = default)
    {
        var rows = await _repo.GetProductLineRowsAsync(isemriNo, fisNo, mamulKodu, cancellationToken);
        return ApiResponse<List<ProductLineDto>>.SuccessResult(_mapper.Map<List<ProductLineDto>>(rows), _localizationService.GetLocalizedString("ProductLinesRetrievedSuccessfully"));
    }
}
