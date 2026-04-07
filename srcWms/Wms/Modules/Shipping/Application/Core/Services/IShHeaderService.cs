using Wms.Application.Common;
using Wms.Application.Shipping.Dtos;

namespace Wms.Application.Shipping.Services;

public interface IShHeaderService
{
    Task<ApiResponse<IEnumerable<ShHeaderDto>>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<ApiResponse<PagedResponse<ShHeaderDto>>> GetPagedAsync(PagedRequest request, CancellationToken cancellationToken = default);
    Task<ApiResponse<ShHeaderDto>> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<ApiResponse<ShHeaderDto>> CreateAsync(CreateShHeaderDto createDto, CancellationToken cancellationToken = default);
    Task<ApiResponse<ShHeaderDto>> UpdateAsync(long id, UpdateShHeaderDto updateDto, CancellationToken cancellationToken = default);
    Task<ApiResponse<bool>> SoftDeleteAsync(long id, CancellationToken cancellationToken = default);
    Task<ApiResponse<bool>> CompleteAsync(long id, CancellationToken cancellationToken = default);
    Task<ApiResponse<IEnumerable<ShHeaderDto>>> GetAssignedOrdersAsync(long userId, CancellationToken cancellationToken = default);
    Task<ApiResponse<PagedResponse<ShHeaderDto>>> GetAssignedOrdersPagedAsync(long userId, PagedRequest request, CancellationToken cancellationToken = default);
    Task<ApiResponse<ShAssignedOrderLinesDto>> GetAssignedOrderLinesAsync(long headerId, CancellationToken cancellationToken = default);
    Task<ApiResponse<ShHeaderDto>> GenerateShipmentOrderAsync(GenerateShipmentOrderRequestDto request, CancellationToken cancellationToken = default);
    Task<ApiResponse<int>> ProcessShipmentAsync(BulkCreateShRequestDto request, CancellationToken cancellationToken = default);
    Task<ApiResponse<int>> BulkCreateShipmentAsync(BulkCreateShRequestDto request, CancellationToken cancellationToken = default);
    Task<ApiResponse<PagedResponse<ShHeaderDto>>> GetCompletedAwaitingErpApprovalPagedAsync(PagedRequest request, CancellationToken cancellationToken = default);
    Task<ApiResponse<ShHeaderDto>> SetApprovalAsync(long id, bool approved, CancellationToken cancellationToken = default);
}
