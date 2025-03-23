using DataAccessLayer;
using Domain.Burns;
using Microsoft.EntityFrameworkCore;

namespace Business.Burns;
public interface ICharacterBurnService
{
    Task<List<CharacterBurn>> GetAllCharacterBurnsAsync();
    Task<CharacterBurn?> GetCharacterBurnByIdAsync(Guid characterId, Guid burnId);
    Task<CharacterBurn> CreateCharacterBurnAsync(CharacterBurn characterBurn);
    Task<CharacterBurn?> UpdateCharacterBurnAsync(Guid characterId, Guid burnId, CharacterBurn characterBurn);
    Task<bool> DeleteCharacterBurnAsync(Guid characterId, Guid burnId);
}
public class CharacterBurnService : ICharacterBurnService
{
    private readonly ApplicationDbContext _context;

    public CharacterBurnService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<CharacterBurn>> GetAllCharacterBurnsAsync()
    {
        return await _context.CharacterBurns
            .Include(cb => cb.Character)
            .Include(cb => cb.Burn)
            .ToListAsync();
    }

    public async Task<CharacterBurn?> GetCharacterBurnByIdAsync(Guid characterId, Guid burnId)
    {
        return await _context.CharacterBurns
            .Include(cb => cb.Character)
            .Include(cb => cb.Burn)
            .FirstOrDefaultAsync(cb => cb.CharacterId == characterId && cb.BurnId == burnId);
    }

    public async Task<CharacterBurn> CreateCharacterBurnAsync(CharacterBurn characterBurn)
    {
        _context.CharacterBurns.Add(characterBurn);
        await _context.SaveChangesAsync();
        return characterBurn;
    }

    public async Task<CharacterBurn?> UpdateCharacterBurnAsync(Guid characterId, Guid burnId, CharacterBurn characterBurn)
    {
        var existingCharacterBurn = await _context.CharacterBurns
            .FirstOrDefaultAsync(cb => cb.CharacterId == characterId && cb.BurnId == burnId);

        if (existingCharacterBurn is null)
        {
            return null;
        }

        existingCharacterBurn = characterBurn;

        await _context.SaveChangesAsync();
        return existingCharacterBurn;
    }

    public async Task<bool> DeleteCharacterBurnAsync(Guid characterId, Guid burnId)
    {
        var existingCharacterBurn = await _context.CharacterBurns
            .FirstOrDefaultAsync(cb => cb.CharacterId == characterId && cb.BurnId == burnId);

        if (existingCharacterBurn is null)
        {
            return false;
        }

        _context.CharacterBurns.Remove(existingCharacterBurn);
        await _context.SaveChangesAsync();
        return true;
    }
}
