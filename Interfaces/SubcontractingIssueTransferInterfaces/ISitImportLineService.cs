using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Services;

namespace WMS_WEBAPI.Interfaces
{
    public interface ISitImportLineService
    {
        Task<ApiResponse<IEnumerable<SitImportLineDto>>> GetAllAsync();
        Task<ApiResponse<PagedResponse<SitImportLineDto>>> GetPagedAsync(PagedRequest request);
        Task<ApiResponse<SitImportLineDto>> GetByIdAsync(long id);
        Task<ApiResponse<IEnumerable<SitImportLineDto>>> GetByHeaderIdAsync(long headerId);
        Task<ApiResponse<IEnumerable<SitImportLineDto>>> GetByLineIdAsync(long lineId);
        Task<ApiResponse<SitImportLineDto>> CreateAsync(CreateSitImportLineDto createDto);
        Task<ApiResponse<SitImportLineDto>> UpdateAsync(long id, UpdateSitImportLineDto updateDto);
        Task<ApiResponse<bool>> SoftDeleteAsync(long id);
        Task<ApiResponse<SitImportLineDto>> AddBarcodeBasedonAssignedOrderAsync(AddSitImportBarcodeRequestDto request);
        Task<ApiResponse<IEnumerable<SitImportLineWithRoutesDto>>> GetCollectedBarcodesByHeaderIdAsync(long headerId);
    }
}
