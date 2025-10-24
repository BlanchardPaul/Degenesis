using DataAccessLayer;
using Degenesis.Shared.DTOs.Characters.CRUD;
using Domain.Characters;
using Microsoft.EntityFrameworkCore;

namespace Business.Characters;
public interface ICharacterAttributeService
{
    Task<bool> UpdateCharacterAttributeAsync(CharacterAttributeDto characterAttribute);
}

public class CharacterAttributeService : ICharacterAttributeService
{
    private readonly ApplicationDbContext _context;

    public CharacterAttributeService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> UpdateCharacterAttributeAsync(CharacterAttributeDto characterAttribute)
    {
        try
        {
            var existingCharacterAttribute = await _context.CharacterAttributes
                .FirstOrDefaultAsync(ca => ca.CharacterId == characterAttribute.CharacterId && ca.AttributeId == characterAttribute.AttributeId) 
                ?? throw new Exception("CharacterAttribute not found");

            existingCharacterAttribute.Level = characterAttribute.Level;

            // We have to modify the maximum variables depending on specific attributes
            var attribute = await _context.Attributes
                .FirstOrDefaultAsync(a => a.Id == existingCharacterAttribute.AttributeId) 
                ?? throw new Exception("Attribute not found");
            var character = await _context.Characters
                .FirstOrDefaultAsync(a => a.Id == existingCharacterAttribute.CharacterId) 
                ?? throw new Exception("Character not found");

            if (attribute.Name == "INTELLECT" && character.IsFocusOriented)
            {
                var focusSkill = await _context.Skills
                    .FirstOrDefaultAsync(s => s.Name == "FOCUS") ?? throw new Exception("Focus skill not found");
                var characterFocusSkill = await _context.CharacterSkills
                    .FirstOrDefaultAsync(cs => cs.CharacterId == character.Id && cs.SkillId == focusSkill.Id) 
                    ?? throw new Exception("CharacterSkill for Focus skill not found");

                character.MaxEgo = (existingCharacterAttribute.Level + characterFocusSkill.Level) * 2;
            }

            if (attribute.Name == "INSTINCT" && !character.IsFocusOriented)
            {
                var primalSkill = await _context.Skills
                    .FirstOrDefaultAsync(s => s.Name == "PRIMAL") ?? throw new Exception("Primal skill not found");
                var characterPrimalSkill = await _context.CharacterSkills
                    .FirstOrDefaultAsync(cs => cs.CharacterId == character.Id && cs.SkillId == primalSkill.Id)
                    ?? throw new Exception("CharacterSkill for Focus skill not found");

                character.MaxEgo = (existingCharacterAttribute.Level + characterPrimalSkill.Level) * 2;
            }

            if(attribute.Name == "PSYCHE")
            {
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

                character.MaxSporeInfestation = (existingCharacterAttribute.Level + Math.Max(characterFaithSkill.Level, characterWillpowerSkill.Level)) * 2;

                var bodyAttribute = await _context.Attributes
                    .FirstOrDefaultAsync(a => a.Name == "BODY")
                    ?? throw new Exception("Body attribute not found");
                var characterBodyAttribute = await _context.CharacterAttributes
                    .FirstOrDefaultAsync(ca => ca.CharacterId == character.Id && ca.AttributeId == bodyAttribute.Id)
                    ?? throw new Exception("CharacterAttribute for Body not found");

                character.MaxTrauma = characterBodyAttribute.Level + existingCharacterAttribute.Level;
            }

            if(attribute.Name == "BODY")
            {
                var toughnessSkill = await _context.Skills
                    .FirstOrDefaultAsync(s => s.Name == "TOUGHNESS") ?? throw new Exception("TOUGHNESS skill not found");
                var characterToughnessSkill = await _context.CharacterSkills
                    .FirstOrDefaultAsync(cs => cs.CharacterId == character.Id && cs.SkillId == toughnessSkill.Id)
                    ?? throw new Exception("CharacterSkill for TOUGHNESS skill not found");

                character.MaxFleshWounds = (existingCharacterAttribute.Level + characterToughnessSkill.Level) * 2;

                var psycheAttribute = await _context.Attributes
                    .FirstOrDefaultAsync(a => a.Name == "PSYCHE")
                    ?? throw new Exception("PSYCHE attribute not found");
                var characterPsycheAttribute = await _context.CharacterAttributes
                    .FirstOrDefaultAsync(ca => ca.CharacterId == character.Id && ca.AttributeId == psycheAttribute.Id)
                    ?? throw new Exception("CharacterAttribute for PSYCHE not found"); ;

                character.MaxTrauma = characterPsycheAttribute.Level + existingCharacterAttribute.Level;
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
