using Wms.Application.Common;
using Wms.Application.Production.Dtos;

namespace Wms.Application.Production.Services;

public interface IPrHeaderService
{
    Task<ApiResponse<IEnumerable<PrHeaderDto>>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<ApiResponse<PagedResponse<PrHeaderDto>>> GetPagedAsync(PagedRequest request, CancellationToken cancellationToken = default);
    Task<ApiResponse<PrHeaderDto>> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<ApiResponse<PrHeaderDto>> CreateAsync(CreatePrHeaderDto createDto, CancellationToken cancellationToken = default);
    Task<ApiResponse<PrHeaderDto>> UpdateAsync(long id, UpdatePrHeaderDto updateDto, CancellationToken cancellationToken = default);
    Task<ApiResponse<bool>> SoftDeleteAsync(long id, CancellationToken cancellationToken = default);
    Task<ApiResponse<bool>> CompleteAsync(long id, CancellationToken cancellationToken = default);
    Task<ApiResponse<IEnumerable<PrHeaderDto>>> GetAssignedProductionOrdersAsync(long userId, CancellationToken cancellationToken = default);
    Task<ApiResponse<PrAssignedProductionOrderLinesDto>> GetAssignedProductionOrderLinesAsync(long headerId, CancellationToken cancellationToken = default);
    Task<ApiResponse<PrHeaderDto>> GenerateProductionOrderAsync(GenerateProductionOrderRequestDto request, CancellationToken cancellationToken = default);
    Task<ApiResponse<PrHeaderDto>> BulkPrGenerateAsync(BulkPrGenerateRequestDto request, CancellationToken cancellationToken = default);
    Task<ApiResponse<PagedResponse<PrHeaderDto>>> GetCompletedAwaitingErpApprovalPagedAsync(PagedRequest request, CancellationToken cancellationToken = default);
    Task<ApiResponse<PrHeaderDto>> SetApprovalAsync(long id, bool approved, CancellationToken cancellationToken = default);
}
