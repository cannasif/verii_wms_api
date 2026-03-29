using Wms.Application.Common;
using Wms.Application.GoodsReceipt.Dtos;

namespace Wms.Application.GoodsReceipt.Services;

public interface IGrHeaderService
{
    Task<ApiResponse<IEnumerable<GrHeaderDto>>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<ApiResponse<PagedResponse<GrHeaderDto>>> GetPagedAsync(PagedRequest request, CancellationToken cancellationToken = default);
    Task<ApiResponse<GrHeaderDto>> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<ApiResponse<long>> BulkCreateAsync(BulkCreateGrRequestDto request, CancellationToken cancellationToken = default);
    Task<ApiResponse<GrHeaderDto>> CreateAsync(CreateGrHeaderDto createDto, CancellationToken cancellationToken = default);
    Task<ApiResponse<bool>> CompleteAsync(int id, CancellationToken cancellationToken = default);
    Task<ApiResponse<IEnumerable<GrHeaderDto>>> GetByCustomerCodeAsync(string customerCode, CancellationToken cancellationToken = default);
    Task<ApiResponse<GrHeaderDto>> GenerateGoodReceiptOrderAsync(GenerateGoodReceiptOrderRequestDto request, CancellationToken cancellationToken = default);
    Task<ApiResponse<PagedResponse<GrHeaderDto>>> GetAssignedOrdersPagedAsync(long userId, PagedRequest request, CancellationToken cancellationToken = default);
    Task<ApiResponse<GrAssignedOrderLinesDto>> GetAssignedOrderLinesAsync(long headerId, CancellationToken cancellationToken = default);
    Task<ApiResponse<PagedResponse<GrHeaderDto>>> GetCompletedAwaitingErpApprovalPagedAsync(PagedRequest request, CancellationToken cancellationToken = default);
    Task<ApiResponse<GrHeaderDto>> SetApprovalAsync(long id, bool approved, CancellationToken cancellationToken = default);
    Task<ApiResponse<bool>> SoftDeleteAsync(int id, CancellationToken cancellationToken = default);
    Task<ApiResponse<GrHeaderDto>> UpdateAsync(int id, UpdateGrHeaderDto updateDto, CancellationToken cancellationToken = default);
}
