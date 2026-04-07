namespace Wms.Application.Common;

public interface IBarcodeResolutionService
{
    Task<ResolvedBarcodeDto> ResolveAsync(ResolveBarcodeRequestDto request, CancellationToken cancellationToken = default);
}
