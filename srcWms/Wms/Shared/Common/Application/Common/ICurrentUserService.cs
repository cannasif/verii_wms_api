namespace Wms.Application.Common;

public interface ICurrentUserService
{
    long? UserId { get; }
    string? BranchCode { get; }
}
