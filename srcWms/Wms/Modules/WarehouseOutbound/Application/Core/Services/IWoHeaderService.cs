using Wms.Application.Common;
using Wms.Application.WarehouseOutbound.Dtos;

namespace Wms.Application.WarehouseOutbound.Services;

public interface IWoHeaderService
{
    Task<ApiResponse<IEnumerable<WoHeaderDto>>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<ApiResponse<PagedResponse<WoHeaderDto>>> GetPagedAsync(PagedRequest request, CancellationToken cancellationToken = default);
    Task<ApiResponse<WoHeaderDto>> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<ApiResponse<WoHeaderDto>> CreateAsync(CreateWoHeaderDto createDto, CancellationToken cancellationToken = default);
    Task<ApiResponse<WoHeaderDto>> UpdateAsync(long id, UpdateWoHeaderDto updateDto, CancellationToken cancellationToken = default);
    Task<ApiResponse<bool>> SoftDeleteAsync(long id, CancellationToken cancellationToken = default);
    Task<ApiResponse<bool>> CompleteAsync(long id, CancellationToken cancellationToken = default);
    Task<ApiResponse<IEnumerable<WoHeaderDto>>> GetAssignedOrdersAsync(long userId, CancellationToken cancellationToken = default);
    Task<ApiResponse<PagedResponse<WoHeaderDto>>> GetAssignedOrdersPagedAsync(long userId, PagedRequest request, CancellationToken cancellationToken = default);
    Task<ApiResponse<WoAssignedOrderLinesDto>> GetAssignedOrderLinesAsync(long headerId, CancellationToken cancellationToken = default);
    Task<ApiResponse<WoHeaderDto>> GenerateWarehouseOutboundOrderAsync(GenerateWarehouseOutboundOrderRequestDto request, CancellationToken cancellationToken = default);
    Task<ApiResponse<int>> ProcessWarehouseOutboundAsync(ProcessWoRequestDto request, CancellationToken cancellationToken = default);
    Task<ApiResponse<int>> BulkCreateWarehouseOutboundAsync(BulkCreateWoRequestDto request, CancellationToken cancellationToken = default);
    Task<ApiResponse<PagedResponse<WoHeaderDto>>> GetCompletedAwaitingErpApprovalPagedAsync(PagedRequest request, CancellationToken cancellationToken = default);
    Task<ApiResponse<WoHeaderDto>> SetApprovalAsync(long id, bool approved, CancellationToken cancellationToken = default);
}
