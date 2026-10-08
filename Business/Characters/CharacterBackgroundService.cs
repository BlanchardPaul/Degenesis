using DataAccessLayer;
using Degenesis.Shared.DTOs;
using Degenesis.Shared.DTOs.Characters.CRUD;
using Microsoft.EntityFrameworkCore;

namespace Business.Characters;
public interface ICharacterBackgroundService
{
    Task<Result<object>> UpdateCharacterBackgroundAsync(CharacterBackgroundDto characterBackground);
}

public class CharacterBackgroundService(ApplicationDbContext context) : ICharacterBackgroundService
{
    private readonly ApplicationDbContext _context = context;

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
            return new Result<object> { Value = null };
        }
        catch (Exception)
        {
            return new Result<object> { IsError = true, Error = "A server error occurred" };
        }
    }
}

