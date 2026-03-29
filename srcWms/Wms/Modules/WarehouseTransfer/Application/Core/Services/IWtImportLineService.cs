using Wms.Application.Common;
using Wms.Application.WarehouseTransfer.Dtos;

namespace Wms.Application.WarehouseTransfer.Services;

public interface IWtImportLineService
{
    Task<ApiResponse<IEnumerable<WtImportLineDto>>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<ApiResponse<PagedResponse<WtImportLineDto>>> GetPagedAsync(PagedRequest request, CancellationToken cancellationToken = default);
    Task<ApiResponse<WtImportLineDto?>> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<ApiResponse<IEnumerable<WtImportLineDto>>> GetByLineIdAsync(long lineId, CancellationToken cancellationToken = default);
    Task<ApiResponse<IEnumerable<WtImportLineDto>>> GetByHeaderIdAsync(long headerId, CancellationToken cancellationToken = default);
    Task<ApiResponse<IEnumerable<WtImportLineDto>>> GetByRouteIdAsync(long routeId, CancellationToken cancellationToken = default);
    Task<ApiResponse<WtImportLineDto>> CreateAsync(CreateWtImportLineDto createDto, CancellationToken cancellationToken = default);
    Task<ApiResponse<WtImportLineDto>> UpdateAsync(long id, UpdateWtImportLineDto updateDto, CancellationToken cancellationToken = default);
    Task<ApiResponse<bool>> SoftDeleteAsync(long id, CancellationToken cancellationToken = default);
    Task<ApiResponse<WtImportLineDto>> AddBarcodeBasedonAssignedOrderAsync(AddWtImportBarcodeRequestDto request, CancellationToken cancellationToken = default);
    Task<ApiResponse<IEnumerable<WtImportLineWithRoutesDto>>> GetCollectedBarcodesByHeaderIdAsync(long headerId, CancellationToken cancellationToken = default);
}
