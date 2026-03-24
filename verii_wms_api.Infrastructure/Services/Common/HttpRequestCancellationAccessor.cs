using Microsoft.AspNetCore.Http;
using WMS_WEBAPI.Interfaces;

namespace WMS_WEBAPI.Services
{
    public sealed class HttpRequestCancellationAccessor : IRequestCancellationAccessor
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HttpRequestCancellationAccessor(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public CancellationToken Get(CancellationToken cancellationToken = default)
        {
            if (cancellationToken.CanBeCanceled)
            {
                return cancellationToken;
            }

            var requestAborted = _httpContextAccessor.HttpContext?.RequestAborted ?? CancellationToken.None;
            return requestAborted.CanBeCanceled ? requestAborted : CancellationToken.None;
        }
    }
}
