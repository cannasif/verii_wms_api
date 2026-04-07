using Wms.Application.Common;
using Wms.Application.SubcontractingIssueTransfer.Dtos;

namespace Wms.Application.SubcontractingIssueTransfer.Services;

public interface ISitHeaderService
{
    Task<ApiResponse<IEnumerable<SitHeaderDto>>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<ApiResponse<PagedResponse<SitHeaderDto>>> GetPagedAsync(PagedRequest request, CancellationToken cancellationToken = default);
    Task<ApiResponse<SitHeaderDto>> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<ApiResponse<SitHeaderDto>> CreateAsync(CreateSitHeaderDto createDto, CancellationToken cancellationToken = default);
    Task<ApiResponse<SitHeaderDto>> UpdateAsync(long id, UpdateSitHeaderDto updateDto, CancellationToken cancellationToken = default);
    Task<ApiResponse<bool>> SoftDeleteAsync(long id, CancellationToken cancellationToken = default);
    Task<ApiResponse<bool>> CompleteAsync(long id, CancellationToken cancellationToken = default);
    Task<ApiResponse<IEnumerable<SitHeaderDto>>> GetAssignedSubcontractingIssueTransferOrdersAsync(long userId, CancellationToken cancellationToken = default);
    Task<ApiResponse<PagedResponse<SitHeaderDto>>> GetAssignedSubcontractingIssueTransferOrdersPagedAsync(long userId, PagedRequest request, CancellationToken cancellationToken = default);
    Task<ApiResponse<SitAssignedSubcontractingIssueTransferOrderLinesDto>> GetAssignedSubcontractingIssueTransferOrderLinesAsync(long headerId, CancellationToken cancellationToken = default);
    Task<ApiResponse<SitAssignedSubcontractingIssueTransferOrderLinesDto>> GetAssignedOrderLinesAsync(long headerId, CancellationToken cancellationToken = default);
    Task<ApiResponse<SitHeaderDto>> GenerateSubcontractingIssueTransferOrderAsync(GenerateSubcontractingIssueTransferOrderRequestDto request, CancellationToken cancellationToken = default);
    Task<ApiResponse<int>> ProcessSubcontractingIssueTransferAsync(BulkSitGenerateRequestDto request, CancellationToken cancellationToken = default);
    Task<ApiResponse<int>> BulkSitGenerateAsync(BulkSitGenerateRequestDto request, CancellationToken cancellationToken = default);
    Task<ApiResponse<int>> BulkCreateSubcontractingIssueTransferAsync(BulkSitGenerateRequestDto request, CancellationToken cancellationToken = default);
    Task<ApiResponse<PagedResponse<SitHeaderDto>>> GetCompletedAwaitingErpApprovalPagedAsync(PagedRequest request, CancellationToken cancellationToken = default);
    Task<ApiResponse<SitHeaderDto>> SetApprovalAsync(long id, bool approved, CancellationToken cancellationToken = default);
}
