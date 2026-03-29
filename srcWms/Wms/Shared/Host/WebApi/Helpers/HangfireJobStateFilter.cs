using Hangfire;
using Hangfire.States;
using Hangfire.Storage;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Wms.Application.Common;
using Wms.Infrastructure.Options;
using Wms.Infrastructure.Persistence.Context;

namespace Wms.WebApi.Helpers;

public sealed class HangfireJobStateFilter : IApplyStateFilter
{
    private readonly ILogger<HangfireJobStateFilter> _logger;
    private readonly IBackgroundJobClient _backgroundJobClient;
    private readonly HangfireMonitoringOptions _options;
    private readonly IServiceScopeFactory _scopeFactory;

    public HangfireJobStateFilter(
        ILogger<HangfireJobStateFilter> logger,
        IBackgroundJobClient backgroundJobClient,
        IOptions<HangfireMonitoringOptions> options,
        IServiceScopeFactory scopeFactory)
    {
        _logger = logger;
        _backgroundJobClient = backgroundJobClient;
        _options = options.Value;
        _scopeFactory = scopeFactory;
    }

    public void OnStateApplied(ApplyStateContext context, IWriteOnlyTransaction transaction)
    {
        var jobId = context.BackgroundJob?.Id ?? "unknown";
        var job = context.BackgroundJob?.Job;
        var jobName = job == null ? "unknown" : $"{job.Type.FullName}.{job.Method.Name}";
        var queue = context.GetJobParameter<string>("Queue");

        if (context.NewState is FailedState failedState)
        {
            var retryCount = context.GetJobParameter<int>("RetryCount");

            _logger.LogError(
                failedState.Exception,
                "Hangfire job failed. JobId: {JobId}, Job: {JobName}, RetryCount: {RetryCount}, Reason: {Reason}",
                jobId,
                jobName,
                retryCount,
                failedState.Reason);

            if (_options.EnableFailureSqlLog)
            {
                TryPersistFailure(jobId, jobName, queue, retryCount, failedState);
            }

            if (IsCriticalJob(jobName) && retryCount >= _options.FinalRetryCountThreshold)
            {
                var payload = new HangfireDeadLetterPayload
                {
                    JobId = jobId,
                    JobName = jobName,
                    Queue = "dead-letter",
                    RetryCount = retryCount,
                    Reason = failedState.Reason,
                    ExceptionType = failedState.Exception?.GetType().FullName,
                    ExceptionMessage = failedState.Exception?.Message,
                    OccurredAtUtc = DateTime.UtcNow
                };

                _backgroundJobClient.Create<IHangfireDeadLetterJob>(
                    x => x.ProcessAsync(payload),
                    new EnqueuedState("dead-letter"));
            }
        }
        else if (context.NewState is SucceededState succeededState)
        {
            _logger.LogInformation(
                "Hangfire job succeeded. JobId: {JobId}, Job: {JobName}, Latency: {Latency}, Duration: {Duration}",
                jobId,
                jobName,
                succeededState.Latency,
                succeededState.PerformanceDuration);
        }
    }

    public void OnStateUnapplied(ApplyStateContext context, IWriteOnlyTransaction transaction)
    {
    }

    private void TryPersistFailure(string jobId, string jobName, string? queue, int retryCount, FailedState failedState)
    {
        try
        {
            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<WmsDbContext>();
            db.JobFailureLogs.Add(new Domain.Entities.Common.JobFailureLog
            {
                JobId = jobId,
                JobName = jobName,
                FailedAt = DateTime.UtcNow,
                Reason = failedState.Reason,
                ExceptionType = failedState.Exception?.GetType().FullName,
                ExceptionMessage = failedState.Exception?.Message,
                StackTrace = Truncate(failedState.Exception?.StackTrace, 4000),
                Queue = queue,
                RetryCount = retryCount,
                CreatedDate = Domain.Common.DateTimeProvider.Now,
                IsDeleted = false
            });
            db.SaveChanges();
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "JobFailureLog write failed. JobId: {JobId}", jobId);
        }
    }

    private bool IsCriticalJob(string jobName)
        => _options.CriticalJobs.Any(pattern =>
            !string.IsNullOrWhiteSpace(pattern) &&
            jobName.Contains(pattern, StringComparison.OrdinalIgnoreCase));

    private static string? Truncate(string? value, int maxLength)
        => string.IsNullOrWhiteSpace(value) ? value : value.Length <= maxLength ? value : value[..maxLength];
}
