using WMS_WEBAPI.DTOs;

namespace WMS_WEBAPI.Interfaces
{
    public interface ISrtHeaderService
    {
        Task<ApiResponse<IEnumerable<SrtHeaderDto>>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<ApiResponse<PagedResponse<SrtHeaderDto>>> GetPagedAsync(PagedRequest request, CancellationToken cancellationToken = default);
        Task<ApiResponse<SrtHeaderDto>> GetByIdAsync(long id, CancellationToken cancellationToken = default);
        Task<ApiResponse<IEnumerable<SrtHeaderDto>>> GetByCustomerCodeAsync(string customerCode, CancellationToken cancellationToken = default);
        Task<ApiResponse<IEnumerable<SrtHeaderDto>>> GetByDocumentTypeAsync(string documentType, CancellationToken cancellationToken = default);
        Task<ApiResponse<IEnumerable<SrtHeaderDto>>> GetByDocumentNoAsync(string documentNo, CancellationToken cancellationToken = default);
        Task<ApiResponse<SrtHeaderDto>> CreateAsync(CreateSrtHeaderDto createDto, CancellationToken cancellationToken = default);
        Task<ApiResponse<SrtHeaderDto>> UpdateAsync(long id, UpdateSrtHeaderDto updateDto, CancellationToken cancellationToken = default);
        Task<ApiResponse<bool>> SoftDeleteAsync(long id, CancellationToken cancellationToken = default);
        Task<ApiResponse<bool>> CompleteAsync(long id, CancellationToken cancellationToken = default);

        Task<ApiResponse<IEnumerable<SrtHeaderDto>>> GetAssignedOrdersAsync(long userId, CancellationToken cancellationToken = default);
        Task<ApiResponse<SrtAssignedOrderLinesDto>> GetAssignedOrderLinesAsync(long headerId, CancellationToken cancellationToken = default);
        Task<ApiResponse<SrtHeaderDto>> GenerateOrderAsync(GenerateSubcontractingReceiptOrderRequestDto request, CancellationToken cancellationToken = default);
        Task<ApiResponse<int>> BulkCreateSubcontractingReceiptTransferAsync(BulkCreateSrtRequestDto request, CancellationToken cancellationToken = default);
        Task<ApiResponse<PagedResponse<SrtHeaderDto>>> GetCompletedAwaitingErpApprovalPagedAsync(PagedRequest request, CancellationToken cancellationToken = default);
        Task<ApiResponse<SrtHeaderDto>> SetApprovalAsync(long id, bool approved, CancellationToken cancellationToken = default);
    }
}
