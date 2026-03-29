using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Wms.WebApi.Hubs;

[Authorize]
public sealed class NotificationHub : Hub
{
    private static readonly Dictionary<string, string> UserConnections = new();
    private static readonly Dictionary<string, string> ConnectionUsers = new();

    public override Task OnConnectedAsync()
    {
        var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!string.IsNullOrWhiteSpace(userId))
        {
            UserConnections[userId] = Context.ConnectionId;
            ConnectionUsers[Context.ConnectionId] = userId;
        }

        return base.OnConnectedAsync();
    }

    public override Task OnDisconnectedAsync(Exception? exception)
    {
        if (ConnectionUsers.TryGetValue(Context.ConnectionId, out var userId))
        {
            UserConnections.Remove(userId);
            ConnectionUsers.Remove(Context.ConnectionId);
        }

        return base.OnDisconnectedAsync(exception);
    }

    public static async Task SendNotificationToUser(IHubContext<NotificationHub> hubContext, string userId, object payload, CancellationToken cancellationToken = default)
    {
        try
        {
            await hubContext.Clients.User(userId).SendAsync("ReceiveNotification", payload, cancellationToken);
            if (UserConnections.TryGetValue(userId, out var connectionId))
            {
                await hubContext.Clients.Client(connectionId).SendAsync("ReceiveNotification", payload, cancellationToken);
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error sending notification to user {userId}: {ex.Message}");
        }
    }

    public static Task SendNotificationToAll(IHubContext<NotificationHub> hubContext, object payload, CancellationToken cancellationToken = default)
    {
        return hubContext.Clients.All.SendAsync("ReceiveNotification", payload, cancellationToken);
    }
}
