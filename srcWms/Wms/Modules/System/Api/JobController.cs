using Microsoft.AspNetCore.Mvc;
using Wms.Application.System.Services;

namespace Wms.WebApi.Controllers.System;

[ApiController]
[Route("api/[controller]")]
public sealed class JobController : ControllerBase
{
    private readonly IJobService _service; public JobController(IJobService service) => _service = service;
    [HttpPost("send-welcome-email")] public async Task<IActionResult> SendWelcomeEmail([FromBody] WelcomeEmailRequest request, CancellationToken cancellationToken = default) => StatusCode((await _service.EnqueueWelcomeEmailAsync(request.Email, request.UserName, cancellationToken)).StatusCode, await _service.EnqueueWelcomeEmailAsync(request.Email, request.UserName, cancellationToken));
    [HttpPost("send-reminder-email")] public async Task<IActionResult> SendReminderEmail([FromBody] ReminderEmailRequest request, CancellationToken cancellationToken = default) => StatusCode((await _service.ScheduleReminderEmailAsync(request.Email, request.Message, request.DelayMinutes, cancellationToken)).StatusCode, await _service.ScheduleReminderEmailAsync(request.Email, request.Message, request.DelayMinutes, cancellationToken));
    [HttpPost("schedule-daily-report")] public async Task<IActionResult> ScheduleDailyReport(CancellationToken cancellationToken = default) => StatusCode((await _service.ScheduleDailyReportAsync(cancellationToken)).StatusCode, await _service.ScheduleDailyReportAsync(cancellationToken));
    [HttpPost("process-with-cleanup")] public async Task<IActionResult> ProcessWithCleanup([FromBody] InventoryUpdateRequest request, CancellationToken cancellationToken = default) => StatusCode((await _service.ProcessWithCleanupAsync(request.InventoryIds, cancellationToken)).StatusCode, await _service.ProcessWithCleanupAsync(request.InventoryIds, cancellationToken));
    [HttpDelete("stop-daily-report")] public async Task<IActionResult> StopDailyReport(CancellationToken cancellationToken = default) => StatusCode((await _service.StopDailyReportAsync(cancellationToken)).StatusCode, await _service.StopDailyReportAsync(cancellationToken));
    [HttpDelete("cancel-job/{jobId}")] public async Task<IActionResult> CancelJob(string jobId, CancellationToken cancellationToken = default) => StatusCode((await _service.CancelJobAsync(jobId, cancellationToken)).StatusCode, await _service.CancelJobAsync(jobId, cancellationToken));
}
public sealed class WelcomeEmailRequest { public string Email { get; set; } = string.Empty; public string UserName { get; set; } = string.Empty; }
public sealed class ReminderEmailRequest { public string Email { get; set; } = string.Empty; public string Message { get; set; } = string.Empty; public int DelayMinutes { get; set; } = 5; }
public sealed class InventoryUpdateRequest { public List<int> InventoryIds { get; set; } = new(); }
