using DataAccessLayer;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace Business.Hubs;
public interface IRoomBroadcastService
{
    Task NotifyCharacterUpdatedAsync(Guid characterId);
    Task NotifyCultRelationshipUpdatedAsync(Guid relationshipId);
}

public class RoomBroadcastService(ApplicationDbContext context, IHubContext<RoomChatHub> hubContext) : IRoomBroadcastService
{
    private readonly ApplicationDbContext _context = context;
    private readonly IHubContext<RoomChatHub> _hubContext = hubContext;

    public async Task NotifyCharacterUpdatedAsync(Guid characterId)
    {
        var roomId = await _context.Characters
            .AsNoTracking()
            .Where(c => c.Id == characterId)
            .Select(c => c.IdRoom)
            .FirstOrDefaultAsync();

        if (roomId != Guid.Empty)
            await _hubContext.Clients.Group(roomId.ToString()).SendAsync("CharacterUpdated", characterId);
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
}