using Wms.Application.Common;
namespace Wms.Application.System.Services;
public interface IJobService
{
Task<ApiResponse<object>> EnqueueWelcomeEmailAsync(string email, string userName, CancellationToken cancellationToken = default);
Task<ApiResponse<object>> ScheduleReminderEmailAsync(string email, string message, int delayMinutes, CancellationToken cancellationToken = default);
Task<ApiResponse<object>> ScheduleDailyReportAsync(CancellationToken cancellationToken = default);
Task<ApiResponse<object>> ProcessWithCleanupAsync(IEnumerable<int> inventoryIds, CancellationToken cancellationToken = default);
Task<ApiResponse<object>> StopDailyReportAsync(CancellationToken cancellationToken = default);
Task<ApiResponse<object>> CancelJobAsync(string jobId, CancellationToken cancellationToken = default);
Task<ApiResponse<PagedResponse<object>>> GetFailedPagedAsync(PagedRequest request, CancellationToken cancellationToken = default);
Task<ApiResponse<PagedResponse<object>>> GetDeadLetterPagedAsync(PagedRequest request, CancellationToken cancellationToken = default);
Task<ApiResponse<PagedResponse<Wms.Domain.Entities.Common.JobFailureLog>>> GetFailureLogsAsync(int pageNumber = 0, int pageSize = 20, CancellationToken cancellationToken = default);
}
