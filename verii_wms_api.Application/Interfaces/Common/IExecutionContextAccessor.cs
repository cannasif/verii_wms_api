namespace WMS_WEBAPI.Interfaces
{
    public interface IExecutionContextAccessor
    {
        long? UserId { get; }
        string? BranchCode { get; }
    }
}
