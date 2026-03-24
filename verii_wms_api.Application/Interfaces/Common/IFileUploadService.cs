using WMS_WEBAPI.DTOs;
using Microsoft.AspNetCore.Http;

namespace WMS_WEBAPI.Interfaces
{
    public interface IFileUploadService
    {
        Task<ApiResponse<string>> UploadProfilePictureAsync(IFormFile file, long userId, CancellationToken cancellationToken = default);
        Task<ApiResponse<bool>> DeleteProfilePictureAsync(string fileUrl, CancellationToken cancellationToken = default);
        string GetProfilePictureUrl(string fileName, long userId);
    }
}
