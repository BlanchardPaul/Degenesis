using Business.Hubs;
using DataAccessLayer;
using Degenesis.Shared.DTOs;
using Degenesis.Shared.DTOs.Characters.CRUD;
using Domain.Characters;
using Microsoft.EntityFrameworkCore;

namespace Business.Characters;
public interface ICharacterBackgroundService
{
    Task<Result<object>> UpdateCharacterBackgroundAsync(CharacterBackgroundDto characterBackground);
}

public class CharacterBackgroundService(ApplicationDbContext context, IRoomBroadcastService broadcast) : ICharacterBackgroundService
{
    private readonly ApplicationDbContext _context = context;
    private readonly IRoomBroadcastService _broadcast = broadcast;

    public async Task<Result<object>> UpdateCharacterBackgroundAsync(CharacterBackgroundDto characterBackground)
    {
        try
        {
            var existingCharacterBackground = await _context.CharacterBackgrounds
                .FirstOrDefaultAsync(cb => cb.CharacterId == characterBackground.CharacterId && cb.BackgroundId == characterBackground.BackgroundId);
            if (existingCharacterBackground is null)
                return new Result<object> { IsError = true, Error = "CharacterBackgrounds not found" };

            existingCharacterBackground.Level = characterBackground.Level;
             
            await _context.SaveChangesAsync();
            await _broadcast.NotifyCharacterUpdatedAsync(characterBackground.CharacterId);
            return new Result<object> { Value = null };
        }
        catch (Exception)
        {
            return new Result<object> { IsError = true, Error = "A server error occurred" };
        }
    }
}

