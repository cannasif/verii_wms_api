using WMS_WEBAPI.DTOs;

namespace WMS_WEBAPI.Interfaces
{
    public interface IWiImportLineService
    {
        Task<ApiResponse<IEnumerable<WiImportLineDto>>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<ApiResponse<PagedResponse<WiImportLineDto>>> GetPagedAsync(PagedRequest request, CancellationToken cancellationToken = default);
        Task<ApiResponse<WiImportLineDto>> GetByIdAsync(long id, CancellationToken cancellationToken = default);
        Task<ApiResponse<IEnumerable<WiImportLineDto>>> GetByHeaderIdAsync(long headerId, CancellationToken cancellationToken = default);
        Task<ApiResponse<IEnumerable<WiImportLineDto>>> GetByLineIdAsync(long lineId, CancellationToken cancellationToken = default);
        Task<ApiResponse<WiImportLineDto>> CreateAsync(CreateWiImportLineDto createDto, CancellationToken cancellationToken = default);
        Task<ApiResponse<WiImportLineDto>> UpdateAsync(long id, UpdateWiImportLineDto updateDto, CancellationToken cancellationToken = default);
        Task<ApiResponse<bool>> SoftDeleteAsync(long id, CancellationToken cancellationToken = default);
        Task<ApiResponse<WiImportLineDto>> AddBarcodeBasedonAssignedOrderAsync(AddWiImportBarcodeRequestDto request, CancellationToken cancellationToken = default);
        Task<ApiResponse<IEnumerable<WiImportLineWithRoutesDto>>> GetCollectedBarcodesByHeaderIdAsync(long headerId, CancellationToken cancellationToken = default);
    }
}
