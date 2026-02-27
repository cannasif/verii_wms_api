using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Models;

namespace WMS_WEBAPI.Interfaces
{
    public interface IMobilemenuLineService
    {
        Task<ApiResponse<IEnumerable<MobilemenuLineDto>>> GetAllAsync();
        Task<ApiResponse<MobilemenuLineDto>> GetByIdAsync(long id);
        Task<ApiResponse<MobilemenuLineDto>> GetByItemIdAsync(string itemId);
        Task<ApiResponse<IEnumerable<MobilemenuLineDto>>> GetByHeaderIdAsync(int headerId);
        Task<ApiResponse<IEnumerable<MobilemenuLineDto>>> GetByTitleAsync(string title);
        Task<ApiResponse<MobilemenuLineDto>> CreateAsync(CreateMobilemenuLineDto createDto);
        Task<ApiResponse<MobilemenuLineDto>> UpdateAsync(long id, UpdateMobilemenuLineDto updateDto);
        Task<ApiResponse<bool>> SoftDeleteAsync(long id);
    }
}