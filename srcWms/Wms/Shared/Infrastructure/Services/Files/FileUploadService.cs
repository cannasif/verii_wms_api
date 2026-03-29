using Microsoft.AspNetCore.Http;
using Wms.Application.Common;

namespace Wms.Infrastructure.Services.Files;

public sealed class FileUploadService : IFileUploadService
{
    private readonly ILocalizationService _localizationService;

    public FileUploadService(ILocalizationService localizationService)
    {
        _localizationService = localizationService;
    }

    public async Task<ApiResponse<string>> UploadProfilePictureAsync(IFormFile file, long userId, CancellationToken cancellationToken = default)
    {
        if (file == null || file.Length == 0)
        {
            var message = _localizationService.GetLocalizedString("ProfilePictureFileIsRequired");
            return ApiResponse<string>.ErrorResult(message, message, 400);
        }

        var uploads = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "profile-pictures");
        Directory.CreateDirectory(uploads);

        var extension = Path.GetExtension(file.FileName);
        var fileName = $"{userId}_{Guid.NewGuid():N}{extension}";
        var fullPath = Path.Combine(uploads, fileName);

        await using var stream = File.Create(fullPath);
        await file.CopyToAsync(stream, cancellationToken);

        return ApiResponse<string>.SuccessResult(GetProfilePictureUrl(fileName, userId), _localizationService.GetLocalizedString("ProfilePictureUploadedSuccessfully"));
    }

    public Task<ApiResponse<bool>> DeleteProfilePictureAsync(string fileUrl, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(fileUrl))
        {
            return Task.FromResult(ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("ProfilePictureDeletedSuccessfully")));
        }

        var relativePath = fileUrl.TrimStart('/').Replace('/', Path.DirectorySeparatorChar);
        var fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", relativePath);
        if (File.Exists(fullPath))
        {
            File.Delete(fullPath);
        }

        return Task.FromResult(ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("ProfilePictureDeletedSuccessfully")));
    }

    public string GetProfilePictureUrl(string fileName, long userId)
    {
        _ = userId;
        return $"/profile-pictures/{fileName}";
    }
}
