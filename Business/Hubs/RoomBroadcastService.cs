using DataAccessLayer;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace Business.Hubs;
public interface IRoomBroadcastService
{
    Task NotifyCharacterUpdatedAsync(Guid characterId, string? excludedConnectionId = null);
    Task NotifyCultRelationshipUpdatedAsync(Guid relationshipId);
    Task NotifyNpcUpdatedAsync(Guid npcId);
}

public class RoomBroadcastService(ApplicationDbContext context, IHubContext<RoomChatHub> hubContext) : IRoomBroadcastService
{
    private readonly ApplicationDbContext _context = context;
    private readonly IHubContext<RoomChatHub> _hubContext = hubContext;

    public async Task NotifyCharacterUpdatedAsync(Guid characterId, string? excludedConnectionId = null)
    {
        var roomId = await _context.Characters
            .AsNoTracking()
            .Where(c => c.Id == characterId)
            .Select(c => c.IdRoom)
            .FirstOrDefaultAsync();

        if (roomId == Guid.Empty)
            return;

        var clients = string.IsNullOrWhiteSpace(excludedConnectionId)
            ? _hubContext.Clients.Group(roomId.ToString())
            : _hubContext.Clients.GroupExcept(roomId.ToString(), [excludedConnectionId]);

        await clients.SendAsync("CharacterUpdated", characterId);
    }

    public async Task NotifyCultRelationshipUpdatedAsync(Guid relationshipId)
    {
        var roomId = await _context.RoomCultRelationships
            .AsNoTracking()
            .Where(r => r.Id == relationshipId)
            .Select(r => r.IdRoom)
            .FirstOrDefaultAsync();

        if (roomId != Guid.Empty)
            await _hubContext.Clients.Group(roomId.ToString()).SendAsync("CultRelationshipUpdated");
    }

    public async Task NotifyNpcUpdatedAsync(Guid npcId)
    {
        var roomId = await _context.NPCs
            .AsNoTracking()
            .Where(n => n.Id == npcId)
            .Select(n => n.IdRoom)
            .FirstOrDefaultAsync();
        if (roomId != Guid.Empty)
            await _hubContext.Clients.Group(roomId.ToString()).SendAsync("NpcUpdated", npcId);
    }
}