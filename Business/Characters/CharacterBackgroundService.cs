using DataAccessLayer;
using Domain.Characters;
using Microsoft.EntityFrameworkCore;

namespace Business.Characters;
public interface ICharacterBackgroundService
{
    Task<CharacterBackground?> GetCharacterBackgroundByIdAsync(Guid characterId, Guid backgroundId);
    Task<IEnumerable<CharacterBackground>> GetCharacterBackgroundsByCharacterIdAsync(Guid characterId);
    Task<IEnumerable<CharacterBackground>> GetCharacterBackgroundsByBackgroundIdAsync(Guid backgroundId);
    Task<CharacterBackground> CreateCharacterBackgroundAsync(CharacterBackground characterBackground);
    Task<bool> UpdateCharacterBackgroundAsync(Guid characterId, Guid backgroundId, CharacterBackground characterBackground);
    Task<bool> DeleteCharacterBackgroundAsync(Guid characterId, Guid backgroundId);
}
public class CharacterBackgroundService : ICharacterBackgroundService
{
    private readonly ApplicationDbContext _context;

    public CharacterBackgroundService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<CharacterBackground?> GetCharacterBackgroundByIdAsync(Guid characterId, Guid backgroundId)
    {
        return await _context.CharacterBackgrounds
            .FirstOrDefaultAsync(cb => cb.CharacterId == characterId && cb.BackgroundId == backgroundId);
    }

    public async Task<IEnumerable<CharacterBackground>> GetCharacterBackgroundsByCharacterIdAsync(Guid characterId)
    {
        return await _context.CharacterBackgrounds
            .Where(cb => cb.CharacterId == characterId)
            .ToListAsync();
    }

    public async Task<IEnumerable<CharacterBackground>> GetCharacterBackgroundsByBackgroundIdAsync(Guid backgroundId)
    {
        return await _context.CharacterBackgrounds
            .Where(cb => cb.BackgroundId == backgroundId)
            .ToListAsync();
    }

    public async Task<CharacterBackground> CreateCharacterBackgroundAsync(CharacterBackground characterBackground)
    {
        _context.CharacterBackgrounds.Add(characterBackground);
        await _context.SaveChangesAsync();
        return characterBackground;
    }

    public async Task<bool> UpdateCharacterBackgroundAsync(Guid characterId, Guid backgroundId, CharacterBackground characterBackground)
    {
        var existingCharacterBackground = await _context.CharacterBackgrounds
            .FirstOrDefaultAsync(cb => cb.CharacterId == characterId && cb.BackgroundId == backgroundId);

        if (existingCharacterBackground == null)
            return false;

        _context.Entry(existingCharacterBackground).CurrentValues.SetValues(characterBackground);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteCharacterBackgroundAsync(Guid characterId, Guid backgroundId)
    {
        var characterBackground = await _context.CharacterBackgrounds
            .FirstOrDefaultAsync(cb => cb.CharacterId == characterId && cb.BackgroundId == backgroundId);

        if (characterBackground == null)
            return false;

        _context.CharacterBackgrounds.Remove(characterBackground);
        await _context.SaveChangesAsync();
        return true;
    }
}

