using Hangfire;
using Microsoft.AspNetCore.Mvc;
using WMS_WEBAPI.Services;

namespace WMS_WEBAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class JobController : ControllerBase
    {
        private readonly BackgroundJobService _backgroundJobService;

        public JobController(BackgroundJobService backgroundJobService)
        {
            _backgroundJobService = backgroundJobService;
        }

        /// <summary>
        /// Fire-and-Forget Job - Hemen çalışır ve unutulur
        /// </summary>
        [HttpPost("send-welcome-email")]
        public IActionResult SendWelcomeEmail([FromBody] WelcomeEmailRequest request)
        {
            var jobId = BackgroundJob.Enqueue(() => _backgroundJobService.SendWelcomeEmail(request.Email, request.UserName));
            
            return Ok(new { JobId = jobId, Message = "Hoş geldin e-postası kuyruğa eklendi" });
        }

        /// <summary>
        /// Delayed Job - Belirli bir süre sonra çalışır
        /// </summary>
        [HttpPost("send-reminder-email")]
        public IActionResult SendReminderEmail([FromBody] ReminderEmailRequest request)
        {
            var jobId = BackgroundJob.Schedule(
                () => _backgroundJobService.SendReminderEmail(request.Email, request.Message),
                TimeSpan.FromMinutes(request.DelayMinutes));
            
            return Ok(new { JobId = jobId, Message = $"Hatırlatma e-postası {request.DelayMinutes} dakika sonra gönderilmek üzere planlandı" });
        }

        /// <summary>
        /// Recurring Job - Belirli aralıklarla tekrar eden iş
        /// </summary>
        [HttpPost("schedule-daily-report")]
        public IActionResult ScheduleDailyReport()
        {
            RecurringJob.AddOrUpdate(
                "daily-report",
                () => _backgroundJobService.GenerateDailyReport(),
                Cron.Daily(9)); // Her gün saat 09:00'da çalışır
            
            return Ok(new { Message = "Günlük rapor işi her gün saat 09:00'da çalışacak şekilde planlandı" });
        }

        /// <summary>
        /// Continuation Job - Başka bir iş tamamlandıktan sonra çalışır
        /// </summary>
        [HttpPost("process-with-cleanup")]
        public IActionResult ProcessWithCleanup([FromBody] InventoryUpdateRequest request)
        {
            var parentJobId = BackgroundJob.Enqueue(() => _backgroundJobService.ProcessInventoryUpdate(request.InventoryIds));
            
            var continuationJobId = BackgroundJob.ContinueJobWith(
                parentJobId,
                () => _backgroundJobService.ProcessDataCleanup());
            
            return Ok(new 
            { 
                ParentJobId = parentJobId, 
                ContinuationJobId = continuationJobId,
                Message = "Envanter güncelleme işi ve ardından temizlik işi planlandı" 
            });
        }

        /// <summary>
        /// Recurring Job'u durdur
        /// </summary>
        [HttpDelete("stop-daily-report")]
        public IActionResult StopDailyReport()
        {
            RecurringJob.RemoveIfExists("daily-report");
            return Ok(new { Message = "Günlük rapor işi durduruldu" });
        }

        /// <summary>
        /// Job'u iptal et
        /// </summary>
        [HttpDelete("cancel-job/{jobId}")]
        public IActionResult CancelJob(string jobId)
        {
            BackgroundJob.Delete(jobId);
            return Ok(new { Message = $"Job {jobId} iptal edildi" });
        }
    }

    // Request DTOs
    public class WelcomeEmailRequest
    {
        public string Email { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
    }

    public class ReminderEmailRequest
    {
        public string Email { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public int DelayMinutes { get; set; } = 5;
    }

    public class InventoryUpdateRequest
    {
        public List<int> InventoryIds { get; set; } = new List<int>();
    }
}