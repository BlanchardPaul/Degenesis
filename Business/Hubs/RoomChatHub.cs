using Business.Rooms;
using Microsoft.AspNetCore.SignalR;

namespace Business.Hubs;

public class RoomChatHub(IRoomChatMessageService chatService) : Hub
{
    public async Task JoinRoom(string roomId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, roomId);

        // Send messages history to user when joined
        var history = await chatService.GetLastMessagesAsync(Guid.Parse(roomId), 50);
        await Clients.Caller.SendAsync("ReceiveHistory", history);
    }

    public async Task LeaveRoom(string roomId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomId);
    }

    public async Task SendMessage(string roomId, string user, string message)
    {
        if (string.IsNullOrWhiteSpace(message))
            return;

        // Save message in db
        await chatService.SaveMessageAsync(Guid.Parse(roomId), user!, message);

        // Send message to others
        await Clients.Group(roomId).SendAsync("ReceiveMessage", user, message);
    }
}