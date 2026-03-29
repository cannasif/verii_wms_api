using Microsoft.AspNetCore.SignalR;
using Wms.Application.Communications.Services;
using Wms.WebApi.Hubs;

namespace Wms.WebApi.Realtime;

public sealed class SignalRNotificationPublisher : INotificationPublisher
{
    private readonly IHubContext<NotificationHub> _hubContext;

    public SignalRNotificationPublisher(IHubContext<NotificationHub> hubContext)
    {
        _hubContext = hubContext;
    }

    public Task PublishToUserAsync(string userId, object payload, CancellationToken cancellationToken = default)
    {
        return NotificationHub.SendNotificationToUser(_hubContext, userId, payload, cancellationToken);
    }

    public Task PublishToAllAsync(object payload, CancellationToken cancellationToken = default)
    {
        return NotificationHub.SendNotificationToAll(_hubContext, payload, cancellationToken);
    }
}
