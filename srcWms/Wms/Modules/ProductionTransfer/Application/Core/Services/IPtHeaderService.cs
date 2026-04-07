using Wms.Application.Common;
using Wms.Application.ProductionTransfer.Dtos;

namespace Wms.Application.ProductionTransfer.Services;

public interface IPtHeaderService
{
    Task<ApiResponse<IEnumerable<PtHeaderDto>>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<ApiResponse<PagedResponse<PtHeaderDto>>> GetPagedAsync(PagedRequest request, CancellationToken cancellationToken = default);
    Task<ApiResponse<PtHeaderDto>> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<ApiResponse<PtHeaderDto>> CreateAsync(CreatePtHeaderDto createDto, CancellationToken cancellationToken = default);
    Task<ApiResponse<PtHeaderDto>> UpdateAsync(long id, UpdatePtHeaderDto updateDto, CancellationToken cancellationToken = default);
    Task<ApiResponse<bool>> SoftDeleteAsync(long id, CancellationToken cancellationToken = default);
    Task<ApiResponse<bool>> CompleteAsync(long id, CancellationToken cancellationToken = default);
    Task<ApiResponse<IEnumerable<PtHeaderDto>>> GetAssignedProductionTransferOrdersAsync(long userId, CancellationToken cancellationToken = default);
    Task<ApiResponse<PagedResponse<PtHeaderDto>>> GetAssignedProductionTransferOrdersPagedAsync(long userId, PagedRequest request, CancellationToken cancellationToken = default);
    Task<ApiResponse<PtAssignedProductionTransferOrderLinesDto>> GetAssignedProductionTransferOrderLinesAsync(long headerId, CancellationToken cancellationToken = default);
    Task<ApiResponse<ProductionTransferDetailDto>> GetProductionTransferDetailAsync(long id, CancellationToken cancellationToken = default);
    Task<ApiResponse<PtHeaderDto>> CreateProductionTransferAsync(CreateProductionTransferRequestDto request, CancellationToken cancellationToken = default);
    Task<ApiResponse<PtHeaderDto>> UpdateProductionTransferAsync(long id, CreateProductionTransferRequestDto request, CancellationToken cancellationToken = default);
    Task<ApiResponse<List<ProductionTransferSuggestedLineDto>>> GetProductionTransferSuggestionsAsync(ProductionTransferSuggestionRequestDto request, CancellationToken cancellationToken = default);
    Task<ApiResponse<PtHeaderDto>> GenerateProductionTransferOrderAsync(GenerateProductionTransferOrderRequestDto request, CancellationToken cancellationToken = default);
    Task<ApiResponse<PtHeaderDto>> BulkPtGenerateAsync(BulkPtGenerateRequestDto request, CancellationToken cancellationToken = default);
    Task<ApiResponse<PagedResponse<PtHeaderDto>>> GetCompletedAwaitingErpApprovalPagedAsync(PagedRequest request, CancellationToken cancellationToken = default);
    Task<ApiResponse<PtHeaderDto>> SetApprovalAsync(long id, bool approved, CancellationToken cancellationToken = default);
}
