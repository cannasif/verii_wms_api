using Wms.Application.Common;
using Wms.Application.WarehouseTransfer.Dtos;

namespace Wms.Application.WarehouseTransfer.Services;

public interface IWtHeaderService
{
    Task<ApiResponse<IEnumerable<WtHeaderDto>>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<ApiResponse<PagedResponse<WtHeaderDto>>> GetPagedAsync(PagedRequest request, CancellationToken cancellationToken = default);
    Task<ApiResponse<WtHeaderDto>> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<ApiResponse<WtHeaderDto>> CreateAsync(CreateWtHeaderDto createDto, CancellationToken cancellationToken = default);
    Task<ApiResponse<WtHeaderDto>> UpdateAsync(long id, UpdateWtHeaderDto updateDto, CancellationToken cancellationToken = default);
    Task<ApiResponse<bool>> SoftDeleteAsync(long id, CancellationToken cancellationToken = default);
    Task<ApiResponse<bool>> CompleteAsync(long id, CancellationToken cancellationToken = default);
    Task<ApiResponse<IEnumerable<WtHeaderDto>>> GetAssignedOrdersAsync(long userId, CancellationToken cancellationToken = default);
    Task<ApiResponse<PagedResponse<WtHeaderDto>>> GetAssignedOrdersPagedAsync(long userId, PagedRequest request, CancellationToken cancellationToken = default);
    Task<ApiResponse<WtAssignedOrderLinesDto>> GetAssignedOrderLinesAsync(long headerId, CancellationToken cancellationToken = default);
    Task<ApiResponse<WtAssignedOrderLinesDto>> GetAssignedTransferOrderLinesAsync(long headerId, CancellationToken cancellationToken = default);
    Task<ApiResponse<WtHeaderDto>> GenerateWarehouseTransferOrderAsync(GenerateWarehouseTransferOrderRequestDto request, CancellationToken cancellationToken = default);
    Task<ApiResponse<int>> ProcessWarehouseTransferAsync(ProcessWtRequestDto request, CancellationToken cancellationToken = default);
    Task<ApiResponse<WtHeaderDto>> BulkCreateWarehouseTransferAsync(BulkCreateWtRequestDto request, CancellationToken cancellationToken = default);
    Task<ApiResponse<WtHeaderDto>> BulkWtGenerateAsync(BulkCreateWtRequestDto request, CancellationToken cancellationToken = default);
    Task<ApiResponse<PagedResponse<WtHeaderDto>>> GetCompletedAwaitingErpApprovalPagedAsync(PagedRequest request, CancellationToken cancellationToken = default);
    Task<ApiResponse<WtHeaderDto>> SetApprovalAsync(long id, bool approved, CancellationToken cancellationToken = default);
}
