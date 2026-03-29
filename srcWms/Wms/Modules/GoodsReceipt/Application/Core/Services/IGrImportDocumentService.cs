using Wms.Application.Common;
using Wms.Application.GoodsReceipt.Dtos;

namespace Wms.Application.GoodsReceipt.Services;

public interface IGrImportDocumentService
{
    Task<ApiResponse<IEnumerable<GrImportDocumentDto>>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<ApiResponse<PagedResponse<GrImportDocumentDto>>> GetPagedAsync(PagedRequest request, CancellationToken cancellationToken = default);
    Task<ApiResponse<GrImportDocumentDto>> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<ApiResponse<IEnumerable<GrImportDocumentDto>>> GetByHeaderIdAsync(long headerId, CancellationToken cancellationToken = default);
    Task<ApiResponse<GrImportDocumentDto>> CreateAsync(CreateGrImportDocumentDto createDto, CancellationToken cancellationToken = default);
    Task<ApiResponse<GrImportDocumentDto>> UpdateAsync(long id, UpdateGrImportDocumentDto updateDto, CancellationToken cancellationToken = default);
    Task<ApiResponse<bool>> SoftDeleteAsync(long id, CancellationToken cancellationToken = default);
}
