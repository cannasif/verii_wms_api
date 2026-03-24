using System.Collections.Generic;
using WMS_WEBAPI.DTOs;

namespace WMS_WEBAPI.Interfaces
{
    public interface IWtHeaderService
    {
        Task<ApiResponse<IEnumerable<WtHeaderDto>>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<ApiResponse<PagedResponse<WtHeaderDto>>> GetPagedAsync(PagedRequest request, CancellationToken cancellationToken = default);
        Task<ApiResponse<WtHeaderDto>> GetByIdAsync(long id, CancellationToken cancellationToken = default);
        Task<ApiResponse<WtHeaderDto>> CreateAsync(CreateWtHeaderDto createDto, CancellationToken cancellationToken = default);
        Task<ApiResponse<WtHeaderDto>> UpdateAsync(long id, UpdateWtHeaderDto updateDto, CancellationToken cancellationToken = default);
        Task<ApiResponse<bool>> SoftDeleteAsync(long id, CancellationToken cancellationToken = default);
        Task<ApiResponse<bool>> CompleteAsync(long id, CancellationToken cancellationToken = default);
        Task<ApiResponse<PagedResponse<WtHeaderDto>>> GetAssignedTransferOrdersAsync(long userId, PagedRequest request, CancellationToken cancellationToken = default);
        Task<ApiResponse<WtAssignedTransferOrderLinesDto>> GetAssignedTransferOrderLinesAsync(long headerId, CancellationToken cancellationToken = default);
        Task<ApiResponse<PagedResponse<WtHeaderDto>>> GetCompletedAwaitingErpApprovalPagedAsync(PagedRequest request, CancellationToken cancellationToken = default);
        Task<ApiResponse<WtHeaderDto>> GenerateWarehouseTransferOrderAsync(GenerateWarehouseTransferOrderRequestDto request, CancellationToken cancellationToken = default);
        Task<ApiResponse<WtHeaderDto>> SetApprovalAsync(long id, bool approved, CancellationToken cancellationToken = default);
        Task<ApiResponse<WtHeaderDto>> BulkWtGenerateAsync(BulkWtGenerateRequestDto request, CancellationToken cancellationToken = default);
    }
}
