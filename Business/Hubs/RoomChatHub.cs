using Microsoft.AspNetCore.SignalR;

namespace Business.Hubs;

public class RoomChatHub : Hub
{
    public async Task JoinRoom(string roomId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, roomId);
    }

    public async Task LeaveRoom(string roomId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomId);
    }

    public async Task SendMessage(string roomId, string user, string message)
    {
        await Clients.Group(roomId).SendAsync("ReceiveMessage", user, message);
    }
}