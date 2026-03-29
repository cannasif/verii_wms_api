using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Wms.Application.Common;

namespace Wms.Infrastructure.Services.Security;

public sealed class HttpCurrentUserAccessor : ICurrentUserAccessor
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public HttpCurrentUserAccessor(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public long? UserId
    {
        get
        {
            var claimValue = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return long.TryParse(claimValue, out var userId) ? userId : null;
        }
    }

    public string? BranchCode
    {
        get
        {
            var context = _httpContextAccessor.HttpContext;
            var itemValue = context?.Items["BranchCode"] as string;
            if (!string.IsNullOrWhiteSpace(itemValue))
            {
                return itemValue;
            }

            var headerValue = context?.Request.Headers["X-Branch-Code"].FirstOrDefault();
            return BranchCodeDefaults.Normalize(headerValue);
        }
    }
}
