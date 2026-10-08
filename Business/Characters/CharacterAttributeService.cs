using DataAccessLayer;
using Degenesis.Shared.DTOs;
using Degenesis.Shared.DTOs.Characters.CRUD;
using Microsoft.EntityFrameworkCore;

namespace Business.Characters;
public interface ICharacterAttributeService
{
    Task<Result<object>> UpdateCharacterAttributeAsync(CharacterAttributeDto characterAttribute);
}

public class CharacterAttributeService(ApplicationDbContext context) : ICharacterAttributeService
{
    private readonly ApplicationDbContext _context = context;

    public async Task<Result<object>> UpdateCharacterAttributeAsync(CharacterAttributeDto characterAttribute)
    {
        try
        {
            var existingCharacterAttribute = await _context.CharacterAttributes
                .FirstOrDefaultAsync(ca => ca.CharacterId == characterAttribute.CharacterId && ca.AttributeId == characterAttribute.AttributeId) ;
            
            if(existingCharacterAttribute is null)
                return new Result<object> { IsError = true, Error = "CharacterAttribute not found" };

            existingCharacterAttribute.Level = characterAttribute.Level;

            // We have to modify the maximum variables depending on specific attributes
            var attribute = await _context.Attributes
                .FirstOrDefaultAsync(a => a.Id == existingCharacterAttribute.AttributeId);
            if(attribute is null)
                return new Result<object> { IsError = true, Error = "Attribute not found" };
            var character = await _context.Characters
                .FirstOrDefaultAsync(a => a.Id == existingCharacterAttribute.CharacterId);
            if(character is null)
                return new Result<object> { IsError = true, Error = "Character not found" };

            if (attribute.Name == "INTELLECT" && character.IsFocusOriented)
            {
                var focusSkill = await _context.Skills
                    .FirstOrDefaultAsync(s => s.Name == "FOCUS");
                if (focusSkill is null)
                    return new Result<object> { IsError = true, Error = "Focus skill not found" };
                var characterFocusSkill = await _context.CharacterSkills
                    .FirstOrDefaultAsync(cs => cs.CharacterId == character.Id && cs.SkillId == focusSkill.Id);
                if (characterFocusSkill is null)
                    return new Result<object> { IsError = true, Error = "CharacterSkill for Focus skill not found" };

                character.MaxEgo = (existingCharacterAttribute.Level + characterFocusSkill.Level) * 2;
            }

            if (attribute.Name == "INSTINCT" && !character.IsFocusOriented)
            {
                var primalSkill = await _context.Skills
                    .FirstOrDefaultAsync(s => s.Name == "PRIMAL");
                if (primalSkill is null)
                    return new Result<object> { IsError = true, Error = "Primal skill not found" };
                var characterPrimalSkill = await _context.CharacterSkills
                    .FirstOrDefaultAsync(cs => cs.CharacterId == character.Id && cs.SkillId == primalSkill.Id);
                if (characterPrimalSkill is null)
                    return new Result<object> { IsError = true, Error = "CharacterSkill for Primal skill not found" };

                character.MaxEgo = (existingCharacterAttribute.Level + characterPrimalSkill.Level) * 2;
            }

            if(attribute.Name == "PSYCHE")
            {
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

                character.MaxSporeInfestation = (existingCharacterAttribute.Level + Math.Max(characterFaithSkill.Level, characterWillpowerSkill.Level)) * 2;

                var bodyAttribute = await _context.Attributes
                    .FirstOrDefaultAsync(a => a.Name == "BODY");
                if (bodyAttribute is null)
                    return new Result<object> { IsError = true, Error = "Body attribute not found" };
                var characterBodyAttribute = await _context.CharacterAttributes
                    .FirstOrDefaultAsync(ca => ca.CharacterId == character.Id && ca.AttributeId == bodyAttribute.Id);
                if (characterBodyAttribute is null)
                    return new Result<object> { IsError = true, Error = "CharacterAttribute for Body not found" };

                character.MaxTrauma = characterBodyAttribute.Level + existingCharacterAttribute.Level;
            }

            if(attribute.Name == "BODY")
            {
                var toughnessSkill = await _context.Skills
                    .FirstOrDefaultAsync(s => s.Name == "TOUGHNESS");
                if (toughnessSkill is null)
                    return new Result<object> { IsError = true, Error = "TOUGHNESS skill not found" };
                var characterToughnessSkill = await _context.CharacterSkills
                    .FirstOrDefaultAsync(cs => cs.CharacterId == character.Id && cs.SkillId == toughnessSkill.Id);
                if (characterToughnessSkill is null)
                    return new Result<object> { IsError = true, Error = "CharacterSkill for TOUGHNESS skill not found" };

                character.MaxFleshWounds = (existingCharacterAttribute.Level + characterToughnessSkill.Level) * 2;

                var psycheAttribute = await _context.Attributes
                    .FirstOrDefaultAsync(a => a.Name == "PSYCHE");
                if (psycheAttribute is null)
                    return new Result<object> { IsError = true, Error = "PSYCHE attribute not found" };
                var characterPsycheAttribute = await _context.CharacterAttributes
                    .FirstOrDefaultAsync(ca => ca.CharacterId == character.Id && ca.AttributeId == psycheAttribute.Id);
                if (characterPsycheAttribute is null)
                    return new Result<object> { IsError = true, Error = "CharacterAttribute for PSYCHE not found" };

                character.MaxTrauma = characterPsycheAttribute.Level + existingCharacterAttribute.Level;
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
