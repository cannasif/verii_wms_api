using WMS_WEBAPI.DTOs;

namespace WMS_WEBAPI.Interfaces
{
    public interface ISmtpSettingsService
    {
        Task<ApiResponse<SmtpSettingsDto>> GetAsync(CancellationToken cancellationToken = default);
        Task<ApiResponse<SmtpSettingsDto>> UpdateAsync(UpdateSmtpSettingsDto dto, long userId, CancellationToken cancellationToken = default);
        Task<SmtpSettingsRuntimeDto> GetRuntimeAsync(CancellationToken cancellationToken = default);
        void InvalidateCache();
    }
}
