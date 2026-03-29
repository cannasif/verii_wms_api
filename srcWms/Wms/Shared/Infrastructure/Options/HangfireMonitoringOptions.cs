namespace Wms.Infrastructure.Options;

public sealed class HangfireMonitoringOptions
{
    public bool EnableFailureSqlLog { get; set; } = true;
    public int FinalRetryCountThreshold { get; set; } = 3;
    public List<string> CriticalJobs { get; set; } = new();
    public List<string> AlertEmails { get; set; } = new();
}
