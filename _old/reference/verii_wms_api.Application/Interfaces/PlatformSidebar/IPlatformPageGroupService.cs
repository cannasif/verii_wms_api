using System;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Models;

namespace WMS_WEBAPI.Interfaces
{
    public interface IPlatformPageGroupService
    {
        Task<ApiResponse<IEnumerable<PlatformPageGroupDto>>> GetAllAsync();
        Task<ApiResponse<PagedResponse<PlatformPageGroupDto>>> GetPagedAsync(PagedRequest request);
        Task<ApiResponse<PlatformPageGroupDto>> GetByIdAsync(long id);
        Task<ApiResponse<PlatformPageGroupDto>> CreateAsync(CreatePlatformPageGroupDto createDto);
        Task<ApiResponse<PlatformPageGroupDto>> UpdateAsync(long id, UpdatePlatformPageGroupDto updateDto);
        Task<ApiResponse<bool>> SoftDeleteAsync(long id);
        Task<ApiResponse<IEnumerable<PlatformPageGroupDto>>> GetByGroupCodeAsync(string groupCode);
        Task<ApiResponse<IEnumerable<PlatformPageGroupDto>>> GetPageGroupsGroupByGroupCodeAsync();
    }
}
