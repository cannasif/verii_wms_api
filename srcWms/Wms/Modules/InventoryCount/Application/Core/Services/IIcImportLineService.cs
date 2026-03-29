using Wms.Application.Common;
using Wms.Application.InventoryCount.Dtos;

namespace Wms.Application.InventoryCount.Services;

public interface IIcImportLineService
{
    Task<ApiResponse<IEnumerable<IcImportLineDto>>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<ApiResponse<PagedResponse<IcImportLineDto>>> GetPagedAsync(PagedRequest request, CancellationToken cancellationToken = default);
    Task<ApiResponse<IcImportLineDto>> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<ApiResponse<IEnumerable<IcImportLineDto>>> GetByHeaderIdAsync(long headerId, CancellationToken cancellationToken = default);
    Task<ApiResponse<IEnumerable<IcImportLineWithRoutesDto>>> GetCollectedBarcodesByHeaderIdAsync(long headerId, CancellationToken cancellationToken = default);
    Task<ApiResponse<IcImportLineDto>> CreateAsync(CreateIcImportLineDto createDto, CancellationToken cancellationToken = default);
    Task<ApiResponse<IcImportLineDto>> UpdateAsync(long id, UpdateIcImportLineDto updateDto, CancellationToken cancellationToken = default);
    Task<ApiResponse<bool>> SoftDeleteAsync(long id, CancellationToken cancellationToken = default);
}
