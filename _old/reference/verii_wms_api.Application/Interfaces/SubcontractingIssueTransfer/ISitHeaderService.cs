using WMS_WEBAPI.DTOs;

namespace WMS_WEBAPI.Interfaces
{
    public interface ISitHeaderService
    {
        Task<ApiResponse<IEnumerable<SitHeaderDto>>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<ApiResponse<PagedResponse<SitHeaderDto>>> GetPagedAsync(PagedRequest request, CancellationToken cancellationToken = default);
        Task<ApiResponse<SitHeaderDto>> GetByIdAsync(long id, CancellationToken cancellationToken = default);
        Task<ApiResponse<IEnumerable<SitHeaderDto>>> GetByCustomerCodeAsync(string customerCode, CancellationToken cancellationToken = default);
        Task<ApiResponse<IEnumerable<SitHeaderDto>>> GetByDocumentTypeAsync(string documentType, CancellationToken cancellationToken = default);
        Task<ApiResponse<IEnumerable<SitHeaderDto>>> GetByDocumentNoAsync(string documentNo, CancellationToken cancellationToken = default);
        Task<ApiResponse<SitHeaderDto>> CreateAsync(CreateSitHeaderDto createDto, CancellationToken cancellationToken = default);
        Task<ApiResponse<SitHeaderDto>> UpdateAsync(long id, UpdateSitHeaderDto updateDto, CancellationToken cancellationToken = default);
        Task<ApiResponse<bool>> SoftDeleteAsync(long id, CancellationToken cancellationToken = default);
        Task<ApiResponse<bool>> CompleteAsync(long id, CancellationToken cancellationToken = default);

        Task<ApiResponse<IEnumerable<SitHeaderDto>>> GetAssignedOrdersAsync(long userId, CancellationToken cancellationToken = default);
        Task<ApiResponse<SitAssignedOrderLinesDto>> GetAssignedOrderLinesAsync(long headerId, CancellationToken cancellationToken = default);
        Task<ApiResponse<SitHeaderDto>> GenerateOrderAsync(GenerateSubcontractingIssueOrderRequestDto request, CancellationToken cancellationToken = default);
        Task<ApiResponse<PagedResponse<SitHeaderDto>>> GetCompletedAwaitingErpApprovalPagedAsync(PagedRequest request, CancellationToken cancellationToken = default);
        Task<ApiResponse<int>> BulkCreateSubcontractingIssueTransferAsync(BulkCreateSitRequestDto request, CancellationToken cancellationToken = default);
        Task<ApiResponse<SitHeaderDto>> SetApprovalAsync(long id, bool approved, CancellationToken cancellationToken = default);
    }
}
