using WMS_WEBAPI.DTOs;

namespace WMS_WEBAPI.Interfaces
{
    public interface ISmtpSettingsService
    {
        Task<ApiResponse<SmtpSettingsDto>> GetAsync();
        Task<ApiResponse<SmtpSettingsDto>> UpdateAsync(UpdateSmtpSettingsDto dto, long userId);
        Task<SmtpSettingsRuntimeDto> GetRuntimeAsync();
        void InvalidateCache();
    }
}
