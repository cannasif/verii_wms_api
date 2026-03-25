namespace WMS_WEBAPI.Interfaces
{
    public interface ICurrentUserService
    {
        long? UserId { get; }
        string? BranchCode { get; }
    }
}
