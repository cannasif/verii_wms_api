using WMS_WEBAPI.DTOs;

namespace WMS_WEBAPI.Interfaces
{
    public interface ISrtImportLineService
    {
        Task<ApiResponse<IEnumerable<SrtImportLineDto>>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<ApiResponse<PagedResponse<SrtImportLineDto>>> GetPagedAsync(PagedRequest request, CancellationToken cancellationToken = default);
        Task<ApiResponse<SrtImportLineDto>> GetByIdAsync(long id, CancellationToken cancellationToken = default);
        Task<ApiResponse<IEnumerable<SrtImportLineDto>>> GetByHeaderIdAsync(long headerId, CancellationToken cancellationToken = default);
        Task<ApiResponse<IEnumerable<SrtImportLineDto>>> GetByLineIdAsync(long lineId, CancellationToken cancellationToken = default);
        Task<ApiResponse<SrtImportLineDto>> CreateAsync(CreateSrtImportLineDto createDto, CancellationToken cancellationToken = default);
        Task<ApiResponse<SrtImportLineDto>> UpdateAsync(long id, UpdateSrtImportLineDto updateDto, CancellationToken cancellationToken = default);
        Task<ApiResponse<bool>> SoftDeleteAsync(long id, CancellationToken cancellationToken = default);
        Task<ApiResponse<SrtImportLineDto>> AddBarcodeBasedonAssignedOrderAsync(AddSrtImportBarcodeRequestDto request, CancellationToken cancellationToken = default);
        Task<ApiResponse<IEnumerable<SrtImportLineWithRoutesDto>>> GetCollectedBarcodesByHeaderIdAsync(long headerId, CancellationToken cancellationToken = default);
    }
}
