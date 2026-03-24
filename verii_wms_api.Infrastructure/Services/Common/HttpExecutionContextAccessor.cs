using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using WMS_WEBAPI.Interfaces;

namespace WMS_WEBAPI.Services
{
    public sealed class HttpExecutionContextAccessor : IExecutionContextAccessor
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HttpExecutionContextAccessor(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public long? UserId
        {
            get
            {
                var user = _httpContextAccessor.HttpContext?.User;
                var userIdClaim = user?.FindFirst(ClaimTypes.NameIdentifier)?.Value
                                  ?? user?.FindFirst("UserId")?.Value;

                return long.TryParse(userIdClaim, out var userId) ? userId : null;
            }
        }

        public string? BranchCode => _httpContextAccessor.HttpContext?.Items["BranchCode"] as string;
    }
}
