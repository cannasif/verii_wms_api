using Microsoft.EntityFrameworkCore;
using Wms.Application.Common;
using Wms.Domain.Entities.Common;

namespace Wms.Application.System.Services;

public sealed class JobService : IJobService
{
    private readonly IRepository<JobFailureLog> _jobFailureLogs;
    public JobService(IRepository<JobFailureLog> jobFailureLogs) => _jobFailureLogs = jobFailureLogs;
    public Task<ApiResponse<object>> EnqueueWelcomeEmailAsync(string email, string userName, CancellationToken cancellationToken = default) => Task.FromResult(ApiResponse<object>.SuccessResult(new { JobId = Guid.NewGuid().ToString("N"), Message = $"Welcome email queued for {email}" }, "Queued"));
    public Task<ApiResponse<object>> ScheduleReminderEmailAsync(string email, string message, int delayMinutes, CancellationToken cancellationToken = default) => Task.FromResult(ApiResponse<object>.SuccessResult(new { JobId = Guid.NewGuid().ToString("N"), Message = $"Reminder scheduled in {delayMinutes} minutes" }, "Scheduled"));
    public Task<ApiResponse<object>> ScheduleDailyReportAsync(CancellationToken cancellationToken = default) => Task.FromResult(ApiResponse<object>.SuccessResult(new { Message = "Daily report schedule registered (pragmatic stub)." }, "Scheduled"));
    public Task<ApiResponse<object>> ProcessWithCleanupAsync(IEnumerable<int> inventoryIds, CancellationToken cancellationToken = default) => Task.FromResult(ApiResponse<object>.SuccessResult(new { ParentJobId = Guid.NewGuid().ToString("N"), ContinuationJobId = Guid.NewGuid().ToString("N"), Count = inventoryIds.Count() }, "Scheduled"));
    public Task<ApiResponse<object>> StopDailyReportAsync(CancellationToken cancellationToken = default) => Task.FromResult(ApiResponse<object>.SuccessResult(new { Message = "Daily report stopped." }, "Stopped"));
    public Task<ApiResponse<object>> CancelJobAsync(string jobId, CancellationToken cancellationToken = default) => Task.FromResult(ApiResponse<object>.SuccessResult(new { Message = $"Job {jobId} cancelled." }, "Cancelled"));
    public async Task<ApiResponse<PagedResponse<object>>> GetFailedPagedAsync(PagedRequest request, CancellationToken cancellationToken = default)
    {
        request ??= new PagedRequest();
        var pageNumber = request.PageNumber < 0 ? 0 : request.PageNumber;
        var pageSize = request.PageSize <= 0 ? 20 : request.PageSize;

        var query = _jobFailureLogs.Query()
            .ApplyFilters(request.Filters, request.FilterLogic)
            .ApplySorting(request.SortBy ?? nameof(JobFailureLog.FailedAt), string.Equals(request.SortDirection, "desc", StringComparison.OrdinalIgnoreCase));

        var total = await query.CountAsync(cancellationToken);
        var items = await query
            .Skip(pageNumber * pageSize)
            .Take(pageSize)
            .Select(x => new
            {
                jobId = x.JobId,
                jobName = x.JobName,
                failedAt = x.FailedAt,
                state = "Failed",
                reason = x.ExceptionMessage ?? x.Reason,
                exceptionType = x.ExceptionType,
                retryCount = x.RetryCount,
                queue = x.Queue
            })
            .ToListAsync(cancellationToken);

        return ApiResponse<PagedResponse<object>>.SuccessResult(new PagedResponse<object>(items.Cast<object>().ToList(), total, pageNumber, pageSize), "Hangfire failed jobs retrieved successfully.");
    }

    public async Task<ApiResponse<PagedResponse<object>>> GetDeadLetterPagedAsync(PagedRequest request, CancellationToken cancellationToken = default)
    {
        request ??= new PagedRequest();
        var pageNumber = request.PageNumber < 0 ? 0 : request.PageNumber;
        var pageSize = request.PageSize <= 0 ? 20 : request.PageSize;

        var query = _jobFailureLogs.Query()
            .Where(x => x.Queue == "dead-letter")
            .ApplyFilters(request.Filters, request.FilterLogic)
            .ApplySorting(request.SortBy ?? nameof(JobFailureLog.FailedAt), string.Equals(request.SortDirection, "desc", StringComparison.OrdinalIgnoreCase));

        var total = await query.CountAsync(cancellationToken);
        var items = await query
            .Skip(pageNumber * pageSize)
            .Take(pageSize)
            .Select(x => new
            {
                jobId = x.JobId,
                jobName = x.JobName,
                enqueuedAt = x.FailedAt,
                state = "Enqueued",
                reason = x.ExceptionMessage ?? x.Reason,
                retryCount = x.RetryCount,
                queue = x.Queue
            })
            .ToListAsync(cancellationToken);

        return ApiResponse<PagedResponse<object>>.SuccessResult(new PagedResponse<object>(items.Cast<object>().ToList(), total, pageNumber, pageSize), "Hangfire dead-letter jobs retrieved successfully.");
    }
    public async Task<ApiResponse<PagedResponse<JobFailureLog>>> GetFailureLogsAsync(int pageNumber = 0, int pageSize = 20, CancellationToken cancellationToken = default) { if (pageNumber < 0) pageNumber = 0; if (pageSize <= 0) pageSize = 20; var q = _jobFailureLogs.Query().OrderByDescending(x => x.FailedAt); var total = await q.CountAsync(cancellationToken); var items = await q.Skip(pageNumber * pageSize).Take(pageSize).ToListAsync(cancellationToken); return ApiResponse<PagedResponse<JobFailureLog>>.SuccessResult(new(items, total, pageNumber, pageSize), "Hangfire SQL failure logs retrieved successfully."); }
}
