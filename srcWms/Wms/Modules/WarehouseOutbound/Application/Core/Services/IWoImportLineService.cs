using Wms.Application.Common;
using Wms.Application.WarehouseOutbound.Dtos;

namespace Wms.Application.WarehouseOutbound.Services;

public interface IWoImportLineService
{
    Task<ApiResponse<IEnumerable<WoImportLineDto>>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<ApiResponse<PagedResponse<WoImportLineDto>>> GetPagedAsync(PagedRequest request, CancellationToken cancellationToken = default);
    Task<ApiResponse<WoImportLineDto?>> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<ApiResponse<IEnumerable<WoImportLineDto>>> GetByHeaderIdAsync(long headerId, CancellationToken cancellationToken = default);
    Task<ApiResponse<IEnumerable<WoImportLineDto>>> GetByLineIdAsync(long lineId, CancellationToken cancellationToken = default);
    Task<ApiResponse<WoImportLineDto>> CreateAsync(CreateWoImportLineDto createDto, CancellationToken cancellationToken = default);
    Task<ApiResponse<WoImportLineDto>> UpdateAsync(long id, UpdateWoImportLineDto updateDto, CancellationToken cancellationToken = default);
    Task<ApiResponse<bool>> SoftDeleteAsync(long id, CancellationToken cancellationToken = default);
    Task<ApiResponse<WoImportLineDto>> AddBarcodeBasedonAssignedOrderAsync(AddWoImportBarcodeRequestDto request, CancellationToken cancellationToken = default);
    Task<ApiResponse<IEnumerable<WoImportLineWithRoutesDto>>> GetCollectedBarcodesByHeaderIdAsync(long headerId, CancellationToken cancellationToken = default);
}
