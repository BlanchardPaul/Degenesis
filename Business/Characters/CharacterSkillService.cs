using DataAccessLayer;
using Degenesis.Shared.DTOs;
using Degenesis.Shared.DTOs.Characters.CRUD;
using Microsoft.EntityFrameworkCore;

namespace Business.Characters;
public interface ICharacterSkillService
{
    Task<Result<object>> UpdateCharacterSkillAsync(CharacterSkillDto characterSkill);
}

public class CharacterSkillService(ApplicationDbContext context) : ICharacterSkillService
{
    private readonly ApplicationDbContext _context = context;

    public async Task<Result<object>> UpdateCharacterSkillAsync(CharacterSkillDto characterSkill)
    {
        try
        {
            var existingCharacterSkill = await _context.CharacterSkills
                .FirstOrDefaultAsync(cs => cs.CharacterId == characterSkill.CharacterId && cs.SkillId == characterSkill.SkillId);
            if (existingCharacterSkill is null)
                return new Result<object> { IsError = true, Error = "CharacterSkill not found" };

            existingCharacterSkill.Level = characterSkill.Level;

            // We have to modify the maximum variables depending on specific attributes
            var skill = await _context.Skills
                .FirstOrDefaultAsync(s => s.Id == existingCharacterSkill.SkillId);
            if (skill is null)
                return new Result<object> { IsError = true, Error = "Skill not found" };
            var character = await _context.Characters
                .FirstOrDefaultAsync(c => c.Id == existingCharacterSkill.CharacterId);
            if (character is null)
                return new Result<object> { IsError = true, Error = "Character not found" };

            if (skill.Name == "FOCUS" && character.IsFocusOriented)
            {
                var intellectAttribute = await _context.Attributes
                   .FirstOrDefaultAsync(a => a.Name == "INTELLECT");
                if (intellectAttribute is null)
                    return new Result<object> { IsError = true, Error = "INTELLECT attribute not found" };
                var characterIntellectAttribute = await _context.CharacterAttributes
                    .FirstOrDefaultAsync(ca => ca.CharacterId == character.Id && ca.AttributeId == intellectAttribute.Id);
                if (characterIntellectAttribute is null)
                    return new Result<object> { IsError = true, Error = "CharacterAttribute for INTELLECT not found" };

                character.MaxEgo = (characterIntellectAttribute.Level + existingCharacterSkill.Level) * 2;
            }

            if(skill.Name == "PRIMAL" && !character.IsFocusOriented)
            {
                var instinctAttribute = await _context.Attributes
                   .FirstOrDefaultAsync(a => a.Name == "INSTINCT");
                if (instinctAttribute is null)
                    return new Result<object> { IsError = true, Error = "INSTINCT attribute not found" };
                var characterInstinctAttribute = await _context.CharacterAttributes
                    .FirstOrDefaultAsync(ca => ca.CharacterId == character.Id && ca.AttributeId == instinctAttribute.Id);
                if (characterInstinctAttribute is null)
                    return new Result<object> { IsError = true, Error = "CharacterAttribute for INSTINCT not found" };

                character.MaxEgo = (characterInstinctAttribute.Level + existingCharacterSkill.Level) * 2;
            }

            if(skill.Name == "FAITH" || skill.Name == "WILLPOWER")
            {
                var psycheAttribute = await _context.Attributes
                   .FirstOrDefaultAsync(a => a.Name == "PSYCHE");
                if (psycheAttribute is null)
                    return new Result<object> { IsError = true, Error = "PSYCHE attribute not found" };
                var characterPsycheAttribute = await _context.CharacterAttributes
                    .FirstOrDefaultAsync(ca => ca.CharacterId == character.Id && ca.AttributeId == psycheAttribute.Id);
                if (characterPsycheAttribute is null)
                    return new Result<object> { IsError = true, Error = "CharacterAttribute for PSYCHE not found" };

                var willpowerSkill = await _context.Skills
                    .FirstOrDefaultAsync(s => s.Name == "WILLPOWER");
                if (willpowerSkill is null)
                    return new Result<object> { IsError = true, Error = "Willpower skill not found" };
                var characterWillpowerSkill = await _context.CharacterSkills
                    .FirstOrDefaultAsync(cs => cs.CharacterId == character.Id && cs.SkillId == willpowerSkill.Id);
                if (characterWillpowerSkill is null)
                    return new Result<object> { IsError = true, Error = "CharacterSkill for Willpower skill not found" };

                var faithSkill = await _context.Skills
                    .FirstOrDefaultAsync(s => s.Name == "FAITH");
                if (faithSkill is null)
                    return new Result<object> { IsError = true, Error = "Faith skill not found" };
                var characterFaithSkill = await _context.CharacterSkills
                    .FirstOrDefaultAsync(cs => cs.CharacterId == character.Id && cs.SkillId == faithSkill.Id);
                if (characterFaithSkill is null)
                    return new Result<object> { IsError = true, Error = "CharacterSkill for Faith skill not found" };

                character.MaxSporeInfestation = (characterPsycheAttribute.Level + Math.Max(characterFaithSkill.Level, characterWillpowerSkill.Level)) * 2;
            }
            
            if(skill.Name == "TOUGHNESS")
            {
                var bodyAttribute = await _context.Attributes
                   .FirstOrDefaultAsync(a => a.Name == "BODY");
                if (bodyAttribute is null)
                    return new Result<object> { IsError = true, Error = "BODY attribute not found" };
                var characterBodyAttribute = await _context.CharacterAttributes
                    .FirstOrDefaultAsync(ca => ca.CharacterId == character.Id && ca.AttributeId == bodyAttribute.Id);
                if (characterBodyAttribute is null)
                    return new Result<object> { IsError = true, Error = "CharacterAttribute for BODY not found" };

                character.MaxFleshWounds = (characterBodyAttribute.Level + existingCharacterSkill.Level) * 2;
            }
            
            await _context.SaveChangesAsync();
            return new Result<object> { Value = null };
        }
        catch (Exception)
        {
            return new Result<object> { IsError = true, Error = "A server error occurred" };
        }
    }

}
