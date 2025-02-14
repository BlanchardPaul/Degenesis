using DataAccessLayer;
using Domain.Artifacts;
using Microsoft.EntityFrameworkCore;

namespace Business.Artifacts;
public interface ICharacterArtifactService
{
    Task<CharacterArtifact?> GetByIdAsync(Guid id);
    Task<IEnumerable<CharacterArtifact>> GetByCharacterIdAsync(Guid characterId);
    Task<CharacterArtifact> CreateAsync(CharacterArtifact characterArtifact);
    Task<bool> UpdateAsync(Guid id, CharacterArtifact characterArtifact);
    Task<bool> DeleteAsync(Guid id);
}

public class CharacterArtifactService : ICharacterArtifactService
{
    private readonly ApplicationDbContext _context;

    public CharacterArtifactService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<CharacterArtifact?> GetByIdAsync(Guid id)
    {
        return await _context.CharacterArtifacts
            .Include(ca => ca.Artifact)
            .FirstOrDefaultAsync(ca => ca.Id == id);
    }

    public async Task<IEnumerable<CharacterArtifact>> GetByCharacterIdAsync(Guid characterId)
    {
        return await _context.CharacterArtifacts
            .Where(ca => ca.CharacterId == characterId)
            .Include(ca => ca.Artifact)
            .ToListAsync();
    }

    public async Task<CharacterArtifact> CreateAsync(CharacterArtifact characterArtifact)
    {
        _context.CharacterArtifacts.Add(characterArtifact);
        await _context.SaveChangesAsync();
        return characterArtifact;
    }

    public async Task<bool> UpdateAsync(Guid id, CharacterArtifact characterArtifact)
    {
        var existing = await _context.CharacterArtifacts.FindAsync(id);
        if (existing == null) return false;

        existing = characterArtifact;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var characterArtifact = await _context.CharacterArtifacts.FindAsync(id);
        if (characterArtifact == null) return false;

        _context.CharacterArtifacts.Remove(characterArtifact);
        await _context.SaveChangesAsync();
        return true;
    }
}