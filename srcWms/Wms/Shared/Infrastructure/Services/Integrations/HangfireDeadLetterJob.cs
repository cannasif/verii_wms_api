using Wms.Application.Common;
using Wms.Domain.Common;
using Wms.Domain.Entities.Common;

namespace Wms.Infrastructure.Services.Integrations;

public sealed class HangfireDeadLetterJob : IHangfireDeadLetterJob
{
    private readonly IRepository<JobFailureLog> _jobFailureLogs;
    private readonly IUnitOfWork _unitOfWork;

    public HangfireDeadLetterJob(IRepository<JobFailureLog> jobFailureLogs, IUnitOfWork unitOfWork)
    {
        _jobFailureLogs = jobFailureLogs;
        _unitOfWork = unitOfWork;
    }

    public async Task ProcessAsync(HangfireDeadLetterPayload payload)
    {
        var entity = new JobFailureLog
        {
            JobId = payload.JobId,
            JobName = payload.JobName,
            FailedAt = payload.OccurredAtUtc,
            Reason = payload.Reason,
            ExceptionType = payload.ExceptionType,
            ExceptionMessage = payload.ExceptionMessage,
            Queue = payload.Queue,
            RetryCount = payload.RetryCount,
            CreatedDate = DateTimeProvider.Now,
            IsDeleted = false
        };

        await _jobFailureLogs.AddAsync(entity);
        await _unitOfWork.SaveChangesAsync();
    }
}
