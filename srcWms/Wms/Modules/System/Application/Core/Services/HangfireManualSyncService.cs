using System.Collections.Concurrent;
using Hangfire;
using Wms.Application.Common;
using Wms.Application.Customer.Services;
using Wms.Application.Stock.Services;
using Wms.Application.System.Dtos;
using Wms.Application.Warehouse.Services;
using Wms.Application.YapKod.Services;

namespace Wms.Application.System.Services;

public sealed class HangfireManualSyncService : IHangfireManualSyncService
{
    private static readonly TimeSpan Cooldown = TimeSpan.FromMinutes(5);
    private static readonly ConcurrentDictionary<string, DateTime> LastTriggeredAtUtcByJobKey = new(StringComparer.OrdinalIgnoreCase);

    private readonly IBackgroundJobClient _backgroundJobs;
    private readonly TimeProvider _timeProvider;

    private sealed record JobDefinition(string Key, string Name, Func<string> Enqueue);

    private readonly IReadOnlyDictionary<string, JobDefinition> _jobs;

    public HangfireManualSyncService(
        IBackgroundJobClient backgroundJobs,
        TimeProvider timeProvider)
    {
        _backgroundJobs = backgroundJobs;
        _timeProvider = timeProvider;

        _jobs = new Dictionary<string, JobDefinition>(StringComparer.OrdinalIgnoreCase)
        {
            ["customer"] = new("customer", "Customer Sync", () => _backgroundJobs.Enqueue<ICustomerSyncJob>(job => job.RunAsync(CancellationToken.None))),
            ["stock"] = new("stock", "Stock Sync", () => _backgroundJobs.Enqueue<IStockSyncJob>(job => job.RunAsync(CancellationToken.None))),
            ["warehouse"] = new("warehouse", "Warehouse Sync", () => _backgroundJobs.Enqueue<IWarehouseSyncJob>(job => job.RunAsync(CancellationToken.None))),
            ["yapkod"] = new("yapkod", "YapKod Sync", () => _backgroundJobs.Enqueue<IYapKodSyncJob>(job => job.RunAsync(CancellationToken.None))),
        };
    }

    public Task<ApiResponse<IReadOnlyList<ManualSyncJobStatusDto>>> GetJobStatusesAsync(CancellationToken cancellationToken = default)
    {
        var now = _timeProvider.GetUtcNow().UtcDateTime;
        var items = _jobs.Values
            .OrderBy(x => x.Name)
            .Select(job => BuildStatus(job, now))
            .Cast<ManualSyncJobStatusDto>()
            .ToList()
            .AsReadOnly();

        return Task.FromResult(ApiResponse<IReadOnlyList<ManualSyncJobStatusDto>>.SuccessResult(items, "Manual sync jobs retrieved successfully."));
    }

    public Task<ApiResponse<TriggerManualSyncJobResponseDto>> TriggerAsync(TriggerManualSyncJobRequestDto request, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(request.JobKey) || !_jobs.TryGetValue(request.JobKey.Trim(), out var job))
        {
            return Task.FromResult(ApiResponse<TriggerManualSyncJobResponseDto>.ErrorResult("Unknown sync job.", "Unknown sync job.", 400));
        }

        var now = _timeProvider.GetUtcNow().UtcDateTime;
        var status = BuildStatus(job, now);
        if (status.IsCoolingDown)
        {
            return Task.FromResult(ApiResponse<TriggerManualSyncJobResponseDto>.ErrorResult(
                $"{job.Name} is cooling down.",
                $"{job.Name} can be triggered again after {status.NextAvailableAtUtc:O}.",
                429));
        }

        var jobId = job.Enqueue();
        LastTriggeredAtUtcByJobKey[job.Key] = now;

        var nextAvailableAtUtc = now.Add(Cooldown);
        var response = new TriggerManualSyncJobResponseDto
        {
            JobKey = job.Key,
            JobName = job.Name,
            JobId = jobId,
            Queue = "default",
            EnqueuedAtUtc = now,
            NextAvailableAtUtc = nextAvailableAtUtc,
            CooldownSecondsRemaining = (int)Math.Ceiling(Cooldown.TotalSeconds)
        };

        return Task.FromResult(ApiResponse<TriggerManualSyncJobResponseDto>.SuccessResult(response, $"{job.Name} queued successfully."));
    }

    private ManualSyncJobStatusDto BuildStatus(JobDefinition job, DateTime now)
    {
        LastTriggeredAtUtcByJobKey.TryGetValue(job.Key, out var lastTriggeredAtUtc);
        DateTime? last = lastTriggeredAtUtc == default ? null : lastTriggeredAtUtc;
        DateTime? next = last?.Add(Cooldown);
        var remaining = next.HasValue ? Math.Max(0, (int)Math.Ceiling((next.Value - now).TotalSeconds)) : 0;

        return new ManualSyncJobStatusDto
        {
            JobKey = job.Key,
            JobName = job.Name,
            LastTriggeredAtUtc = last,
            NextAvailableAtUtc = next,
            IsCoolingDown = remaining > 0,
            CooldownSecondsRemaining = remaining
        };
    }
}
