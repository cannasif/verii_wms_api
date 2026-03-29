using Wms.Application.Common;
using Wms.Application.GoodsReceipt.Dtos;

namespace Wms.Application.GoodsReceipt.Services;

public interface IGrImportLineService
{
    Task<ApiResponse<IEnumerable<GrImportLineDto>>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<ApiResponse<PagedResponse<GrImportLineDto>>> GetPagedAsync(PagedRequest request, CancellationToken cancellationToken = default);
    Task<ApiResponse<GrImportLineDto?>> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<ApiResponse<IEnumerable<GrImportLineDto>>> GetByHeaderIdAsync(long headerId, CancellationToken cancellationToken = default);
    Task<ApiResponse<IEnumerable<GrImportLineWithRoutesDto>>> GetWithRoutesByHeaderIdAsync(long headerId, CancellationToken cancellationToken = default);
    Task<ApiResponse<IEnumerable<GrImportLineDto>>> GetByLineIdAsync(long lineId, CancellationToken cancellationToken = default);
    Task<ApiResponse<GrImportLineDto>> CreateAsync(CreateGrImportLineDto createDto, CancellationToken cancellationToken = default);
    Task<ApiResponse<GrImportLineDto>> UpdateAsync(long id, UpdateGrImportLineDto updateDto, CancellationToken cancellationToken = default);
    Task<ApiResponse<bool>> SoftDeleteAsync(long id, CancellationToken cancellationToken = default);
    Task<ApiResponse<GrImportLineDto>> AddBarcodeBasedonAssignedOrderAsync(AddGrImportBarcodeRequestDto request, CancellationToken cancellationToken = default);
    Task<ApiResponse<IEnumerable<GrImportLineWithRoutesDto>>> GetCollectedBarcodesByHeaderIdAsync(long headerId, CancellationToken cancellationToken = default);
}
