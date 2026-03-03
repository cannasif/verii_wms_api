using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Services;

namespace WMS_WEBAPI.Interfaces
{
    public interface ISrtImportLineService
    {
        Task<ApiResponse<IEnumerable<SrtImportLineDto>>> GetAllAsync();
        Task<ApiResponse<PagedResponse<SrtImportLineDto>>> GetPagedAsync(PagedRequest request);
        Task<ApiResponse<SrtImportLineDto>> GetByIdAsync(long id);
        Task<ApiResponse<IEnumerable<SrtImportLineDto>>> GetByHeaderIdAsync(long headerId);
        Task<ApiResponse<IEnumerable<SrtImportLineDto>>> GetByLineIdAsync(long lineId);
        Task<ApiResponse<SrtImportLineDto>> CreateAsync(CreateSrtImportLineDto createDto);
        Task<ApiResponse<SrtImportLineDto>> UpdateAsync(long id, UpdateSrtImportLineDto updateDto);
        Task<ApiResponse<bool>> SoftDeleteAsync(long id);
        Task<ApiResponse<SrtImportLineDto>> AddBarcodeBasedonAssignedOrderAsync(AddSrtImportBarcodeRequestDto request);
        Task<ApiResponse<IEnumerable<SrtImportLineWithRoutesDto>>> GetCollectedBarcodesByHeaderIdAsync(long headerId);
    }
}
