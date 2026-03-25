using WMS_WEBAPI.Interfaces;

namespace WMS_WEBAPI.Services
{
    public sealed class HttpRequestCancellationAccessor : IRequestCancellationAccessor
    {
        public CancellationToken Get(CancellationToken cancellationToken = default)
        {
            // Host-agnostic: in non-HTTP execution (Hangfire/gRPC), the passed token is the only reliable cancellation signal.
            return cancellationToken.CanBeCanceled ? cancellationToken : CancellationToken.None;
        }
    }
}
