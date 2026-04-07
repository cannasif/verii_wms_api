namespace Wms.Application.Common;

public interface IBarcodeDefinitionService
{
    Task<BarcodeDefinitionDto?> GetDefinitionAsync(string moduleKey, CancellationToken cancellationToken = default);
    Task<BarcodeDefinitionDto?> GetDefinitionByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<BarcodeDefinitionDto>> GetDefinitionsAsync(CancellationToken cancellationToken = default);
    Task<ApiResponse<BarcodeDefinitionDto>> CreateAsync(SaveBarcodeDefinitionRequestDto request, CancellationToken cancellationToken = default);
    Task<ApiResponse<BarcodeDefinitionDto>> UpdateAsync(long id, SaveBarcodeDefinitionRequestDto request, CancellationToken cancellationToken = default);
    Task<ApiResponse<bool>> DeleteAsync(long id, CancellationToken cancellationToken = default);
}
