using DataAccessLayer;
using Domain.Characters;
using Microsoft.EntityFrameworkCore;

namespace Business.Characters;
public interface ICharacterPotentialService
{
    Task<CharacterPotential?> GetCharacterPotentialByIdAsync(Guid characterId, Guid potentialId);
    Task<IEnumerable<CharacterPotential>> GetCharacterPotentialsByCharacterIdAsync(Guid characterId);
    Task<IEnumerable<CharacterPotential>> GetCharacterPotentialsByPotentialIdAsync(Guid potentialId);
    Task<CharacterPotential> CreateCharacterPotentialAsync(CharacterPotential characterPotential);
    Task<bool> UpdateCharacterPotentialAsync(Guid characterId, Guid potentialId, CharacterPotential characterPotential);
    Task<bool> DeleteCharacterPotentialAsync(Guid characterId, Guid potentialId);
}
public class CharacterPotentialService : ICharacterPotentialService
{
    private readonly ApplicationDbContext _context;

    public CharacterPotentialService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<CharacterPotential?> GetCharacterPotentialByIdAsync(Guid characterId, Guid potentialId)
    {
        return await _context.CharacterPotentials
            .FirstOrDefaultAsync(cp => cp.CharacterId == characterId && cp.PotentialId == potentialId);
    }

    public async Task<IEnumerable<CharacterPotential>> GetCharacterPotentialsByCharacterIdAsync(Guid characterId)
    {
        return await _context.CharacterPotentials
            .Where(cp => cp.CharacterId == characterId)
            .ToListAsync();
    }

    public async Task<IEnumerable<CharacterPotential>> GetCharacterPotentialsByPotentialIdAsync(Guid potentialId)
    {
        return await _context.CharacterPotentials
            .Where(cp => cp.PotentialId == potentialId)
            .ToListAsync();
    }

    public async Task<CharacterPotential> CreateCharacterPotentialAsync(CharacterPotential characterPotential)
    {
        _context.CharacterPotentials.Add(characterPotential);
        await _context.SaveChangesAsync();
        return characterPotential;
    }

    public async Task<bool> UpdateCharacterPotentialAsync(Guid characterId, Guid potentialId, CharacterPotential characterPotential)
    {
        var existingCharacterPotential = await _context.CharacterPotentials
            .FirstOrDefaultAsync(cp => cp.CharacterId == characterId && cp.PotentialId == potentialId);

        if (existingCharacterPotential == null)
            return false;

        _context.Entry(existingCharacterPotential).CurrentValues.SetValues(characterPotential);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteCharacterPotentialAsync(Guid characterId, Guid potentialId)
    {
        var characterPotential = await _context.CharacterPotentials
            .FirstOrDefaultAsync(cp => cp.CharacterId == characterId && cp.PotentialId == potentialId);

        if (characterPotential == null)
            return false;

        _context.CharacterPotentials.Remove(characterPotential);
        await _context.SaveChangesAsync();
        return true;
    }
}
