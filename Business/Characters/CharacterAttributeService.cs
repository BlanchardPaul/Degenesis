using DataAccessLayer;
using Domain.Characters;
using Microsoft.EntityFrameworkCore;

namespace Business.Characters;
public interface ICharacterAttributeService
{
    Task<CharacterAttribute?> GetCharacterAttributeByIdAsync(Guid characterId, Guid attributeId);
    Task<IEnumerable<CharacterAttribute>> GetCharacterAttributesByCharacterIdAsync(Guid characterId);
    Task<IEnumerable<CharacterAttribute>> GetCharacterAttributesByAttributeIdAsync(Guid attributeId);
    Task<CharacterAttribute> CreateCharacterAttributeAsync(CharacterAttribute characterAttribute);
    Task<bool> UpdateCharacterAttributeAsync(Guid characterId, Guid attributeId, CharacterAttribute characterAttribute);
    Task<bool> DeleteCharacterAttributeAsync(Guid characterId, Guid attributeId);
}

public class CharacterAttributeService : ICharacterAttributeService
{
    private readonly ApplicationDbContext _context;

    public CharacterAttributeService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<CharacterAttribute?> GetCharacterAttributeByIdAsync(Guid characterId, Guid attributeId)
    {
        return await _context.CharacterAttributes
            .FirstOrDefaultAsync(ca => ca.CharacterId == characterId && ca.AttributeId == attributeId);
    }

    public async Task<IEnumerable<CharacterAttribute>> GetCharacterAttributesByCharacterIdAsync(Guid characterId)
    {
        return await _context.CharacterAttributes
            .Where(ca => ca.CharacterId == characterId)
            .ToListAsync();
    }

    public async Task<IEnumerable<CharacterAttribute>> GetCharacterAttributesByAttributeIdAsync(Guid attributeId)
    {
        return await _context.CharacterAttributes
            .Where(ca => ca.AttributeId == attributeId)
            .ToListAsync();
    }

    public async Task<CharacterAttribute> CreateCharacterAttributeAsync(CharacterAttribute characterAttribute)
    {
        _context.CharacterAttributes.Add(characterAttribute);
        await _context.SaveChangesAsync();
        return characterAttribute;
    }

    public async Task<bool> UpdateCharacterAttributeAsync(Guid characterId, Guid attributeId, CharacterAttribute characterAttribute)
    {
        var existingCharacterAttribute = await _context.CharacterAttributes
            .FirstOrDefaultAsync(ca => ca.CharacterId == characterId && ca.AttributeId == attributeId);

        if (existingCharacterAttribute == null)
            return false;

        _context.Entry(existingCharacterAttribute).CurrentValues.SetValues(characterAttribute);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteCharacterAttributeAsync(Guid characterId, Guid attributeId)
    {
        var characterAttribute = await _context.CharacterAttributes
            .FirstOrDefaultAsync(ca => ca.CharacterId == characterId && ca.AttributeId == attributeId);

        if (characterAttribute == null)
            return false;

        _context.CharacterAttributes.Remove(characterAttribute);
        await _context.SaveChangesAsync();
        return true;
    }
}
