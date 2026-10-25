using Business.Hubs;
using DataAccessLayer;
using Degenesis.Shared.DTOs;
using Degenesis.Shared.DTOs.Rooms;
using Microsoft.EntityFrameworkCore;

namespace Business.Rooms;
public interface IRoomCultRelationshipService
{
    Task<Result<List<CultRelationshipDto>>> GetByRoomAsync(Guid roomId);
    Task<Result<object>> DiscoverAsync(Guid roomId, Guid relationshipId);
    Task<Result<object>> UpdateRelationshipAsync(IdIntEditDto idIntEditDto);
}

public class RoomCultRelationshipService(ApplicationDbContext context, IRoomBroadcastService broadcast) : IRoomCultRelationshipService
{
    private readonly ApplicationDbContext _context = context;
    private readonly IRoomBroadcastService _broadcast = broadcast;

    public async Task<Result<List<CultRelationshipDto>>> GetByRoomAsync(Guid roomId)
    {
        try
        {
            var relationships = await _context.RoomCultRelationships
                .Where(r => r.IdRoom == roomId)
                .OrderBy(r => r.Cult.Name)
                .Select(r => new CultRelationshipDto
                {
                    Id = r.Id,
                    IsDiscovered = r.IsDiscovered,
                    Relationship = r.Relationship,
                    IdCult = r.IdCult,
                    CultName = r.Cult.Name,
                    CultImageName = r.Cult.ImageName
                })
                .ToListAsync();

            return new Result<List<CultRelationshipDto>> { Value = relationships };
        }
        catch (Exception)
        {
            return new Result<List<CultRelationshipDto>> { IsError = true, Error = "A server error occurred" };
        }
    }

    public async Task<Result<object>> DiscoverAsync(Guid roomId, Guid relationshipId)
    {
        try
        {
            var relationship = await _context.RoomCultRelationships
                .FirstOrDefaultAsync(r => r.Id == relationshipId && r.IdRoom == roomId);
            if (relationship is null)
                return new Result<object> { IsError = true, Error = "Relationship not found" };

            relationship.IsDiscovered = true;
            await _context.SaveChangesAsync();
            await _broadcast.NotifyCultRelationshipUpdatedAsync(relationship.Id);
            return new Result<object> { Value = null };
        }
        catch (Exception)
        {
            return new Result<object> { IsError = true, Error = "A server error occurred" };
        }
    }

    public async Task<Result<object>> UpdateRelationshipAsync(IdIntEditDto idIntEditDto)
    {
        try
        {
            if (idIntEditDto.Value is < -6 or > 6)
                return new Result<object> { IsError = true, Error = "Relationship must be between -6 and 6" };

            var relationship = await _context.RoomCultRelationships
                .FirstOrDefaultAsync(r => r.Id == idIntEditDto.Id);
            if (relationship is null)
                return new Result<object> { IsError = true, Error = "Relationship not found" };
            relationship.Relationship = idIntEditDto.Value;

            await _context.SaveChangesAsync();
            await _broadcast.NotifyCultRelationshipUpdatedAsync(relationship.Id);
            return new Result<object> { Value = null };
        }
        catch (Exception)
        {
            return new Result<object> { IsError = true, Error = "A server error occurred" };
        }
    }
}