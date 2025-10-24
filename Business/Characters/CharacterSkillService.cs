using AutoMapper;
using DataAccessLayer;
using Degenesis.Shared.DTOs.Characters.CRUD;
using Domain.Characters;
using Microsoft.EntityFrameworkCore;

namespace Business.Characters;
public interface ICharacterSkillService
{
    Task<bool> UpdateCharacterSkillAsync(CharacterSkillDto characterSkill);
}
public class CharacterSkillService : ICharacterSkillService
{
    private readonly ApplicationDbContext _context;

    public CharacterSkillService(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
    }

    public async Task<bool> UpdateCharacterSkillAsync(CharacterSkillDto characterSkill)
    {
        try
        {
            var existingCharacterSkill = await _context.CharacterSkills
                .FirstOrDefaultAsync(cs => cs.CharacterId == characterSkill.CharacterId && cs.SkillId == characterSkill.SkillId)
                ?? throw new Exception("CharacterSkill not found");

            existingCharacterSkill.Level = characterSkill.Level;

            // We have to modify the maximum variables depending on specific attributes
            var skill = await _context.Skills
                .FirstOrDefaultAsync(s => s.Id == existingCharacterSkill.SkillId)
                ?? throw new Exception("Skill not found");
            var character = await _context.Characters
                .FirstOrDefaultAsync(c => c.Id == existingCharacterSkill.CharacterId)
                ?? throw new Exception("Character not found");

            if(skill.Name == "FOCUS" && character.IsFocusOriented)
            {
                var intellectAttribute = await _context.Attributes
                   .FirstOrDefaultAsync(a => a.Name == "INTELLECT")
                   ?? throw new Exception("INTELLECT attribute not found");
                var characterIntellectAttribute = await _context.CharacterAttributes
                    .FirstOrDefaultAsync(ca => ca.CharacterId == character.Id && ca.AttributeId == intellectAttribute.Id)
                    ?? throw new Exception("CharacterAttribute for INTELLECT not found");

                character.MaxEgo = (characterIntellectAttribute.Level + existingCharacterSkill.Level) * 2;
            }

            if(skill.Name == "PRIMAL" && !character.IsFocusOriented)
            {
                var instinctAttribute = await _context.Attributes
                   .FirstOrDefaultAsync(a => a.Name == "INSTINCT")
                   ?? throw new Exception("INSTINCT attribute not found");
                var characterInstinctAttribute = await _context.CharacterAttributes
                    .FirstOrDefaultAsync(ca => ca.CharacterId == character.Id && ca.AttributeId == instinctAttribute.Id)
                    ?? throw new Exception("CharacterAttribute for INSTINCT not found");

                character.MaxEgo = (characterInstinctAttribute.Level + existingCharacterSkill.Level) * 2;
            }

            if(skill.Name == "FAITH" || skill.Name == "WILLPOWER")
            {
                var psycheAttribute = await _context.Attributes
                   .FirstOrDefaultAsync(a => a.Name == "PSYCHE")
                   ?? throw new Exception("PSYCHE attribute not found");
                var characterPsycheAttribute = await _context.CharacterAttributes
                    .FirstOrDefaultAsync(ca => ca.CharacterId == character.Id && ca.AttributeId == psycheAttribute.Id)
                    ?? throw new Exception("CharacterAttribute for PSYCHE not found");

                var willpowerSkill = await _context.Skills
                    .FirstOrDefaultAsync(s => s.Name == "WILLPOWER") ?? throw new Exception("Willpower skill not found");
                var characterWillpowerSkill = await _context.CharacterSkills
                    .FirstOrDefaultAsync(cs => cs.CharacterId == character.Id && cs.SkillId == willpowerSkill.Id)
                    ?? throw new Exception("CharacterSkill for Willpower skill not found");

                var faithSkill = await _context.Skills
                    .FirstOrDefaultAsync(s => s.Name == "FAITH") ?? throw new Exception("Faith skill not found");
                var characterFaithSkill = await _context.CharacterSkills
                    .FirstOrDefaultAsync(cs => cs.CharacterId == character.Id && cs.SkillId == faithSkill.Id)
                    ?? throw new Exception("CharacterSkill for Faith skill not found");

                character.MaxSporeInfestation = (characterPsycheAttribute.Level + Math.Max(characterFaithSkill.Level, characterWillpowerSkill.Level)) * 2;
            }
            
            if(skill.Name == "TOUGHNESS")
            {
                var bodyAttribute = await _context.Attributes
                   .FirstOrDefaultAsync(a => a.Name == "BODY")
                   ?? throw new Exception("BODY attribute not found");
                var characterBodyAttribute = await _context.CharacterAttributes
                    .FirstOrDefaultAsync(ca => ca.CharacterId == character.Id && ca.AttributeId == bodyAttribute.Id)
                    ?? throw new Exception("CharacterAttribute for BODY not found");

                character.MaxFleshWounds = (characterBodyAttribute.Level + existingCharacterSkill.Level) * 2;
            }
            
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

}
