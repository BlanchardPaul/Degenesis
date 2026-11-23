using Business.Rooms;
using DataAccessLayer;
using Degenesis.Shared.DTOs.Rooms;
using Domain.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace Business.Hubs;

public class RoomChatHub(
    IRoomChatMessageService chatService,
    ApplicationDbContext context,
    UserManager<ApplicationUser> userManager) : Hub
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

    public async Task SendMapDrawing(Guid roomId, Guid mapId, RoomMapDrawingDto drawing)
    {
        var map = await context.RoomMaps
            .Where(item => item.Id == mapId && item.IdRoom == roomId)
            .Select(item => new { item.Width, item.Height })
            .FirstOrDefaultAsync();
        if (map is null || !await IsRoomMemberAsync(roomId))
            throw new HubException("Room access denied.");

        if (drawing is null || drawing.Id == Guid.Empty || drawing.StrokeWidth is < 1 or > 20 ||
            string.IsNullOrEmpty(drawing.Color) || drawing.Color[0] != '#' || drawing.Color.Length is not (7 or 9) ||
            !drawing.Color.AsSpan(1).ToString().All(Uri.IsHexDigit) ||
            drawing.Points is null || drawing.Points.Count is < 1 or > 10000 ||
            drawing.Points.Any(point => point is null || !double.IsFinite(point.X) || !double.IsFinite(point.Y) ||
                                        point.X < 0 || point.X > map.Width || point.Y < 0 || point.Y > map.Height))
            throw new HubException("Map drawing contains invalid data.");

        await Clients.OthersInGroup(roomId.ToString()).SendAsync("ReceiveMapDrawing", mapId, drawing);
    }

    public async Task DeleteMapDrawing(Guid roomId, Guid mapId, Guid drawingId)
    {
        var isMapInRoom = await context.RoomMaps.AnyAsync(item => item.Id == mapId && item.IdRoom == roomId);
        if (!isMapInRoom || !await IsRoomGmAsync(roomId))
            throw new HubException("Only the room GM can delete map drawings.");

        await Clients.OthersInGroup(roomId.ToString()).SendAsync("ReceiveMapDrawingDeleted", mapId, drawingId);
    }

    private async Task<bool> IsRoomMemberAsync(Guid roomId)
    {
        var user = await userManager.FindByNameAsync(Context.User?.Identity?.Name ?? string.Empty);
        return user is not null && await context.UserRooms.AnyAsync(userRoom =>
            userRoom.IdRoom == roomId && userRoom.IdApplicationUser == user.Id &&
            (userRoom.IsGM || userRoom.InvitationAccepted));
    }

    private async Task<bool> IsRoomGmAsync(Guid roomId)
    {
        var user = await userManager.FindByNameAsync(Context.User?.Identity?.Name ?? string.Empty);
        return user is not null && await context.UserRooms.AnyAsync(userRoom =>
            userRoom.IdRoom == roomId && userRoom.IdApplicationUser == user.Id && userRoom.IsGM);
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

    public async Task SendRollResult(string roomId, DiceRollResultDto result)
    {
        await chatService.SaveRollResultAsync(Guid.Parse(roomId), result);
        await Clients.Group(roomId).SendAsync("ReceiveRollResult", result);
    }

    public async Task GetHistory(string roomId)
    {
        var history = await chatService.GetLastMessagesAsync(Guid.Parse(roomId), 50);
        await Clients.Caller.SendAsync("ReceiveHistory", history);
    }
}