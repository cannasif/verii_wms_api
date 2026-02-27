using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Models;

namespace WMS_WEBAPI.Interfaces
{
    public interface IMobilePageGroupService
    {
        Task<ApiResponse<IEnumerable<MobilePageGroupDto>>> GetAllAsync();
        Task<ApiResponse<MobilePageGroupDto>> GetByIdAsync(long id);
        Task<ApiResponse<IEnumerable<MobilePageGroupDto>>> GetByGroupCodeAsync(string groupCode);
        Task<ApiResponse<IEnumerable<MobilePageGroupDto>>> GetByMenuHeaderIdAsync(int menuHeaderId);
        Task<ApiResponse<IEnumerable<MobilePageGroupDto>>> GetByMenuLineIdAsync(int menuLineId);
        Task<ApiResponse<MobilePageGroupDto>> CreateAsync(CreateMobilePageGroupDto createDto);
        Task<ApiResponse<MobilePageGroupDto>> UpdateAsync(long id, UpdateMobilePageGroupDto updateDto);
        Task<ApiResponse<bool>> SoftDeleteAsync(long id);
        Task<ApiResponse<IEnumerable<MobilePageGroupDto>>> GetMobilPageGroupsByGroupCodeAsync();

    }
}