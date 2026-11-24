using Degenesis.Shared.DTOs.Rooms;
using Microsoft.AspNetCore.SignalR;

namespace Business.Hubs;

public interface IRoomMapBroadcastService
{
    Task NotifyMapSelectedAsync(Guid roomId, Guid mapId);
    Task NotifyGridConfigurationUpdatedAsync(Guid roomId, Guid mapId, int gridCellSize, bool isGridVisible);
    Task NotifyDrawingAddedAsync(Guid roomId, Guid mapId, RoomMapDrawingDto drawing);
    Task NotifyDrawingDeletedAsync(Guid roomId, Guid mapId, Guid drawingId);
    Task NotifyMapStateUpdatedAsync(Guid roomId, Guid mapId, RoomMapStateDto state);
}

public class RoomMapBroadcastService(IHubContext<RoomChatHub> hubContext) : IRoomMapBroadcastService
{
    private readonly IHubContext<RoomChatHub> _hubContext = hubContext;

    public Task NotifyMapSelectedAsync(Guid roomId, Guid mapId)
        => _hubContext.Clients.Group(roomId.ToString()).SendAsync("ReceiveRoomMapSelected", mapId);

    public Task NotifyGridConfigurationUpdatedAsync(Guid roomId, Guid mapId, int gridCellSize, bool isGridVisible)
        => _hubContext.Clients.Group(roomId.ToString()).SendAsync("ReceiveGridConfigurationUpdated", mapId, gridCellSize, isGridVisible);

    public Task NotifyDrawingAddedAsync(Guid roomId, Guid mapId, RoomMapDrawingDto drawing)
        => _hubContext.Clients.Group(roomId.ToString()).SendAsync("ReceiveMapDrawingAdded", mapId, drawing);

    public Task NotifyDrawingDeletedAsync(Guid roomId, Guid mapId, Guid drawingId)
        => _hubContext.Clients.Group(roomId.ToString()).SendAsync("ReceiveMapDrawingDeleted", mapId, drawingId);

    public Task NotifyMapStateUpdatedAsync(Guid roomId, Guid mapId, RoomMapStateDto state)
        => _hubContext.Clients.Group(roomId.ToString()).SendAsync("ReceiveMapStateUpdated", mapId, state);
}
