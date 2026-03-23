using Hangfire;
using Hangfire.Storage;
using Hangfire.Storage.Monitoring;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Models;
using WMS_WEBAPI.UnitOfWork;

namespace WMS_WEBAPI.Controllers
{
    [ApiController]
    [Route("api/hangfire")]
        [Authorize]
    public class HangfireController : ControllerBase
    {
        private readonly IMonitoringApi _monitoringApi;
        private readonly IUnitOfWork _unitOfWork;

        public HangfireController(IUnitOfWork unitOfWork)
        {
            _monitoringApi = JobStorage.Current.GetMonitoringApi();
            _unitOfWork = unitOfWork;
        }

        [HttpGet("stats")]
        public IActionResult GetStats()
        {
            var stats = _monitoringApi.GetStatistics();

            return Ok(new
            {
                stats.Enqueued,
                stats.Processing,
                stats.Scheduled,
                stats.Succeeded,
                stats.Failed,
                stats.Deleted,
                stats.Servers,
                stats.Queues,
                Timestamp = DateTime.UtcNow
            });
        }

        [HttpGet("failed")]
        public IActionResult GetFailed([FromQuery] int from = 0, [FromQuery] int count = 20)
        {
            if (from < 0) from = 0;
            if (count <= 0) count = 20;
            if (count > 100) count = 100;

            var failed = _monitoringApi.FailedJobs(from, count)
                .Select(MapJob)
                .ToList();

            return Ok(new
            {
                Items = failed,
                Total = _monitoringApi.FailedCount(),
                Timestamp = DateTime.UtcNow
            });
        }

        [HttpGet("dead-letter")]
        public IActionResult GetDeadLetter([FromQuery] int from = 0, [FromQuery] int count = 20)
        {
            if (from < 0) from = 0;
            if (count <= 0) count = 20;
            if (count > 100) count = 100;

            var queue = _monitoringApi.Queues().FirstOrDefault(x => x.Name == "dead-letter");
            var jobs = queue == null
                ? new List<object>()
                : _monitoringApi.EnqueuedJobs("dead-letter", from, count)
                    .Select((KeyValuePair<string, EnqueuedJobDto> item) => MapEnqueuedJob(item))
                    .Cast<object>()
                    .ToList();

            return Ok(new
            {
                Queue = "dead-letter",
                Enqueued = queue?.Length ?? 0,
                Items = jobs,
                Timestamp = DateTime.UtcNow
            });
        }

        [HttpGet("failure-logs")]
        public async Task<ActionResult<ApiResponse<PagedResponse<JobFailureLog>>>> GetFailureLogs(
            [FromQuery] int pageNumber = 0,
            [FromQuery] int pageSize = 20)
        {
            if (pageNumber < 0) pageNumber = 0;
            if (pageSize <= 0) pageSize = 20;
            if (pageSize > 100) pageSize = 100;

            var query = _unitOfWork.JobFailureLogs.Query()
                .OrderByDescending(x => x.FailedAt);

            var totalCount = await query.CountAsync();
            var items = await query
                .Skip(pageNumber * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var paged = new PagedResponse<JobFailureLog>(items, totalCount, pageNumber, pageSize);
            return Ok(ApiResponse<PagedResponse<JobFailureLog>>.SuccessResult(paged, "Hangfire SQL failure logs retrieved successfully."));
        }

        private static object MapJob(KeyValuePair<string, FailedJobDto> kvp)
        {
            var details = kvp.Value;
            var job = details.Job;

            return new
            {
                JobId = kvp.Key,
                JobName = job == null ? "unknown" : $"{job.Type.Name}.{job.Method.Name}",
                FailedAt = details.FailedAt,
                State = "Failed",
                Reason = details.ExceptionMessage
            };
        }

        private static object MapEnqueuedJob(KeyValuePair<string, EnqueuedJobDto> kvp)
        {
            var details = kvp.Value;
            var job = details?.Job;

            return new
            {
                JobId = kvp.Key,
                JobName = job == null ? "unknown" : $"{job.Type.Name}.{job.Method.Name}",
                EnqueuedAt = details?.EnqueuedAt,
                State = "Enqueued",
                Reason = "dead-letter"
            };
        }
    }
}
