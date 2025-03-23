using DataAccessLayer;
using Domain.Characters;
using Microsoft.EntityFrameworkCore;

namespace Business.Characters;
public interface ICharacterSkillService
{
    Task<CharacterSkill?> GetCharacterSkillByIdAsync(Guid characterId, Guid skillId);
    Task<IEnumerable<CharacterSkill>> GetCharacterSkillsByCharacterIdAsync(Guid characterId);
    Task<IEnumerable<CharacterSkill>> GetCharacterSkillsBySkillIdAsync(Guid skillId);
    Task<CharacterSkill> CreateCharacterSkillAsync(CharacterSkill characterSkill);
    Task<bool> UpdateCharacterSkillAsync(Guid characterId, Guid skillId, CharacterSkill characterSkill);
    Task<bool> DeleteCharacterSkillAsync(Guid characterId, Guid skillId);
}
public class CharacterSkillService : ICharacterSkillService
{
    private readonly ApplicationDbContext _context;

    public CharacterSkillService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<CharacterSkill?> GetCharacterSkillByIdAsync(Guid characterId, Guid skillId)
    {
        return await _context.CharacterSkills
            .FirstOrDefaultAsync(cs => cs.CharacterId == characterId && cs.SkillId == skillId);
    }

    public async Task<IEnumerable<CharacterSkill>> GetCharacterSkillsByCharacterIdAsync(Guid characterId)
    {
        return await _context.CharacterSkills
            .Where(cs => cs.CharacterId == characterId)
            .ToListAsync();
    }

    public async Task<IEnumerable<CharacterSkill>> GetCharacterSkillsBySkillIdAsync(Guid skillId)
    {
        return await _context.CharacterSkills
            .Where(cs => cs.SkillId == skillId)
            .ToListAsync();
    }

    public async Task<CharacterSkill> CreateCharacterSkillAsync(CharacterSkill characterSkill)
    {
        _context.CharacterSkills.Add(characterSkill);
        await _context.SaveChangesAsync();
        return characterSkill;
    }

    public async Task<bool> UpdateCharacterSkillAsync(Guid characterId, Guid skillId, CharacterSkill characterSkill)
    {
        var existingCharacterSkill = await _context.CharacterSkills
            .FirstOrDefaultAsync(cs => cs.CharacterId == characterId && cs.SkillId == skillId);

        if (existingCharacterSkill is null)
            return false;

        _context.Entry(existingCharacterSkill).CurrentValues.SetValues(characterSkill);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteCharacterSkillAsync(Guid characterId, Guid skillId)
    {
        var characterSkill = await _context.CharacterSkills
            .FirstOrDefaultAsync(cs => cs.CharacterId == characterId && cs.SkillId == skillId);

        if (characterSkill is null)
            return false;

        _context.CharacterSkills.Remove(characterSkill);
        await _context.SaveChangesAsync();
        return true;
    }
}
