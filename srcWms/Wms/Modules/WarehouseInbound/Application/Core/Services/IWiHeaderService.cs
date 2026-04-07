using Wms.Application.Common;
using Wms.Application.WarehouseInbound.Dtos;

namespace Wms.Application.WarehouseInbound.Services;

public interface IWiHeaderService
{
    Task<ApiResponse<IEnumerable<WiHeaderDto>>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<ApiResponse<PagedResponse<WiHeaderDto>>> GetPagedAsync(PagedRequest request, CancellationToken cancellationToken = default);
    Task<ApiResponse<WiHeaderDto>> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<ApiResponse<IEnumerable<WiHeaderDto>>> GetByInboundTypeAsync(string inboundType, CancellationToken cancellationToken = default);
    Task<ApiResponse<IEnumerable<WiHeaderDto>>> GetByAccountCodeAsync(string accountCode, CancellationToken cancellationToken = default);
    Task<ApiResponse<WiHeaderDto>> CreateAsync(CreateWiHeaderDto createDto, CancellationToken cancellationToken = default);
    Task<ApiResponse<WiHeaderDto>> UpdateAsync(long id, UpdateWiHeaderDto updateDto, CancellationToken cancellationToken = default);
    Task<ApiResponse<bool>> SoftDeleteAsync(long id, CancellationToken cancellationToken = default);
    Task<ApiResponse<bool>> CompleteAsync(long id, CancellationToken cancellationToken = default);
    Task<ApiResponse<IEnumerable<WiHeaderDto>>> GetAssignedOrdersAsync(long userId, CancellationToken cancellationToken = default);
    Task<ApiResponse<PagedResponse<WiHeaderDto>>> GetAssignedOrdersPagedAsync(long userId, PagedRequest request, CancellationToken cancellationToken = default);
    Task<ApiResponse<WiAssignedOrderLinesDto>> GetAssignedOrderLinesAsync(long headerId, CancellationToken cancellationToken = default);
    Task<ApiResponse<WiHeaderDto>> GenerateWarehouseInboundOrderAsync(GenerateWarehouseInboundOrderRequestDto request, CancellationToken cancellationToken = default);
    Task<ApiResponse<int>> BulkCreateWarehouseInboundAsync(BulkCreateWiRequestDto request, CancellationToken cancellationToken = default);
    Task<ApiResponse<int>> ProcessWarehouseInboundAsync(ProcessWiRequestDto request, CancellationToken cancellationToken = default);
    Task<ApiResponse<PagedResponse<WiHeaderDto>>> GetCompletedAwaitingErpApprovalPagedAsync(PagedRequest request, CancellationToken cancellationToken = default);
    Task<ApiResponse<WiHeaderDto>> SetApprovalAsync(long id, bool approved, CancellationToken cancellationToken = default);
}
