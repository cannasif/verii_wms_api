using WMS_WEBAPI.Services.Jobs;

namespace WMS_WEBAPI.Interfaces
{
    public interface IHangfireDeadLetterJob
    {
        Task ProcessAsync(HangfireDeadLetterPayload payload);
    }
}
