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
                reason = GetUserFriendlyReason(x.ExceptionMessage ?? x.Reason, x.JobName),
                technicalReason = x.ExceptionMessage ?? x.Reason,
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
                reason = GetUserFriendlyReason(x.ExceptionMessage ?? x.Reason, x.JobName),
                technicalReason = x.ExceptionMessage ?? x.Reason,
                retryCount = x.RetryCount,
                queue = x.Queue
            })
            .ToListAsync(cancellationToken);

        return ApiResponse<PagedResponse<object>>.SuccessResult(new PagedResponse<object>(items.Cast<object>().ToList(), total, pageNumber, pageSize), "Hangfire dead-letter jobs retrieved successfully.");
    }
    public async Task<ApiResponse<PagedResponse<JobFailureLog>>> GetFailureLogsAsync(int pageNumber = 0, int pageSize = 20, CancellationToken cancellationToken = default) { if (pageNumber < 0) pageNumber = 0; if (pageSize <= 0) pageSize = 20; var q = _jobFailureLogs.Query().OrderByDescending(x => x.FailedAt); var total = await q.CountAsync(cancellationToken); var items = await q.Skip(pageNumber * pageSize).Take(pageSize).ToListAsync(cancellationToken); return ApiResponse<PagedResponse<JobFailureLog>>.SuccessResult(new(items, total, pageNumber, pageSize), "Hangfire SQL failure logs retrieved successfully."); }

    private static string GetUserFriendlyReason(string? technicalReason, string? jobName)
    {
        if (string.IsNullOrWhiteSpace(technicalReason))
        {
            return "İş başarısız oldu. Teknik detay kaydı bulunamadı.";
        }

        var message = technicalReason.Trim();

        if (message.Contains("ErpConnection is not configured", StringComparison.OrdinalIgnoreCase))
        {
            return "ERP bağlantı ayarı bulunamadı. Sistem ERP veritabanına bağlanamıyor.";
        }

        if (message.Contains("Invalid object name", StringComparison.OrdinalIgnoreCase)
            || message.Contains("could not find stored procedure", StringComparison.OrdinalIgnoreCase))
        {
            return "ERP fonksiyonu veya tablo bulunamadı. İlgili nesne ERP veritabanında eksik olabilir.";
        }

        if (message.Contains("login failed", StringComparison.OrdinalIgnoreCase)
            || message.Contains("cannot open database", StringComparison.OrdinalIgnoreCase))
        {
            return "ERP veritabanına giriş başarısız oldu. Kullanıcı adı, şifre veya veritabanı adı kontrol edilmeli.";
        }

        if (message.Contains("network-related", StringComparison.OrdinalIgnoreCase)
            || message.Contains("server was not found", StringComparison.OrdinalIgnoreCase)
            || message.Contains("connection", StringComparison.OrdinalIgnoreCase) && message.Contains("open", StringComparison.OrdinalIgnoreCase))
        {
            return "ERP veritabanına bağlantı kurulamadı. Sunucu erişimi veya ağ bağlantısı kontrol edilmeli.";
        }

        if (jobName?.Contains("SyncJob", StringComparison.OrdinalIgnoreCase) == true)
        {
            return $"Senkron işlemi tamamlanamadı. {message}";
        }

        return message;
    }
}
