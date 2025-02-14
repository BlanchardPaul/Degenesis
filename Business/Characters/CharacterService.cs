using DataAccessLayer;
using Domain.Characters;
using Microsoft.EntityFrameworkCore;

namespace Business.Characters;
public interface ICharacterService
{
    Task<Character?> GetCharacterByIdAsync(Guid id);
    Task<IEnumerable<Character>> GetAllCharactersAsync();
    Task<Character> CreateCharacterAsync(Character character);
    Task<bool> UpdateCharacterAsync(Guid id, Character character);
    Task<bool> DeleteCharacterAsync(Guid id);
}

public class CharacterService : ICharacterService
{
    private readonly ApplicationDbContext _context;

    public CharacterService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Character?> GetCharacterByIdAsync(Guid id)
    {
        return await _context.Characters
            .Include(c => c.Cult)
            .Include(c => c.Culture)
            .Include(c => c.Concept)
            .Include(c => c.CharacterArtifacts)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<IEnumerable<Character>> GetAllCharactersAsync()
    {
        return await _context.Characters.ToListAsync();
    }

    public async Task<Character> CreateCharacterAsync(Character character)
    {
        _context.Characters.Add(character);
        await _context.SaveChangesAsync();
        return character;
    }

    public async Task<bool> UpdateCharacterAsync(Guid id, Character character)
    {
        var existingCharacter = await _context.Characters.FindAsync(id);
        if (existingCharacter == null)
            return false;

        _context.Entry(existingCharacter).CurrentValues.SetValues(character);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteCharacterAsync(Guid id)
    {
        var character = await _context.Characters.FindAsync(id);
        if (character == null)
            return false;

        _context.Characters.Remove(character);
        await _context.SaveChangesAsync();
        return true;
    }
}
