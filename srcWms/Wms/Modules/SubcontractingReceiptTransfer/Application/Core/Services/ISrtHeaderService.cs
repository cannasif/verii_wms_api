using Wms.Application.Common;
using Wms.Application.SubcontractingReceiptTransfer.Dtos;

namespace Wms.Application.SubcontractingReceiptTransfer.Services;

public interface ISrtHeaderService
{
    Task<ApiResponse<IEnumerable<SrtHeaderDto>>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<ApiResponse<PagedResponse<SrtHeaderDto>>> GetPagedAsync(PagedRequest request, CancellationToken cancellationToken = default);
    Task<ApiResponse<SrtHeaderDto>> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<ApiResponse<SrtHeaderDto>> CreateAsync(CreateSrtHeaderDto createDto, CancellationToken cancellationToken = default);
    Task<ApiResponse<SrtHeaderDto>> UpdateAsync(long id, UpdateSrtHeaderDto updateDto, CancellationToken cancellationToken = default);
    Task<ApiResponse<bool>> SoftDeleteAsync(long id, CancellationToken cancellationToken = default);
    Task<ApiResponse<bool>> CompleteAsync(long id, CancellationToken cancellationToken = default);
    Task<ApiResponse<IEnumerable<SrtHeaderDto>>> GetAssignedSubcontractingReceiptTransferOrdersAsync(long userId, CancellationToken cancellationToken = default);
    Task<ApiResponse<SrtAssignedSubcontractingReceiptTransferOrderLinesDto>> GetAssignedSubcontractingReceiptTransferOrderLinesAsync(long headerId, CancellationToken cancellationToken = default);
    Task<ApiResponse<SrtAssignedSubcontractingReceiptTransferOrderLinesDto>> GetAssignedOrderLinesAsync(long headerId, CancellationToken cancellationToken = default);
    Task<ApiResponse<SrtHeaderDto>> GenerateSubcontractingReceiptTransferOrderAsync(GenerateSubcontractingReceiptTransferOrderRequestDto request, CancellationToken cancellationToken = default);
    Task<ApiResponse<int>> BulkSrtGenerateAsync(BulkSrtGenerateRequestDto request, CancellationToken cancellationToken = default);
    Task<ApiResponse<int>> BulkCreateSubcontractingReceiptTransferAsync(BulkSrtGenerateRequestDto request, CancellationToken cancellationToken = default);
    Task<ApiResponse<PagedResponse<SrtHeaderDto>>> GetCompletedAwaitingErpApprovalPagedAsync(PagedRequest request, CancellationToken cancellationToken = default);
    Task<ApiResponse<SrtHeaderDto>> SetApprovalAsync(long id, bool approved, CancellationToken cancellationToken = default);
}
