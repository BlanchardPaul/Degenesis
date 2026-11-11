using AutoMapper;
using Business.Hubs;
using DataAccessLayer;
using Degenesis.Shared.DTOs;
using Degenesis.Shared.DTOs.Characters.CRUD;
using Degenesis.Shared.DTOs.Characters.Display;
using Domain.Characters;
using Microsoft.EntityFrameworkCore;

namespace Business.Characters;
public interface ICharacterSavedDiceRollService
{
    Task<Result<List<CharacterSavedDiceRollDisplayDto>>> GetCharacterSavedDiceRollByIdCharacterAsync(Guid characterId);
    Task<Result<object>> CreateCharacterSavedDiceRollAsync(CharacterSavedDiceRollDto characterSavedDiceRoll);
    Task<Result<object>> UpdateCharacterSavedDiceRollAsync(CharacterSavedDiceRollDto characterSavedDiceRoll);
    Task<Result<object>> DeleteCharacterSavedDiceRollAsync(Guid characterSavedDiceRollId);
}

public class CharacterSavedDiceRollService(ApplicationDbContext context, IMapper mapper, IRoomBroadcastService broadcast) : ICharacterSavedDiceRollService
{
    private readonly ApplicationDbContext _context = context;
    private readonly IMapper _mapper = mapper;
    private readonly IRoomBroadcastService _broadcast = broadcast;

    public async Task<Result<List<CharacterSavedDiceRollDisplayDto>>> GetCharacterSavedDiceRollByIdCharacterAsync(Guid characterId)
    {
        try
        {
            var savedRolls = await _context.CharacterSavedDiceRolls
                .Include(sr => sr.Attribute)
                .Include(sr => sr.Skill)
                .Where(sr => sr.CharacterId == characterId)
                .OrderBy(sr => sr.Name)
                .ToListAsync();

            return new Result<List<CharacterSavedDiceRollDisplayDto>> { Value = _mapper.Map<List<CharacterSavedDiceRollDisplayDto>>(savedRolls) };
        }
        catch
        {
            return new Result<List<CharacterSavedDiceRollDisplayDto>> { IsError = true, Error = "A server error occurred" };
        }
    }

    public async Task<Result<object>> CreateCharacterSavedDiceRollAsync(CharacterSavedDiceRollDto characterSavedDiceRoll)
    {
        try
        {
            var character = await _context.Characters.FindAsync(characterSavedDiceRoll.CharacterId);
            if (character is null)
                return new Result<object> { IsError = true, Error = "Character not found" };

            var attribute = await _context.Attributes.FindAsync(characterSavedDiceRoll.AttributeId);
            if (attribute is null)
                return new Result<object> { IsError = true, Error = "Attribute not found" };

            var skill = await _context.Skills.FindAsync(characterSavedDiceRoll.SkillId);
            if (skill is null)
                return new Result<object> { IsError = true, Error = "Skill not found" };

            var savedRoll = _mapper.Map<CharacterSavedDiceRoll>(characterSavedDiceRoll);
            savedRoll.Id = Guid.NewGuid();
            savedRoll.Character = character;
            savedRoll.Attribute = attribute;
            savedRoll.Skill = skill;

            _context.CharacterSavedDiceRolls.Add(savedRoll);
            await _context.SaveChangesAsync();
            await _broadcast.NotifyCharacterUpdatedAsync(character.Id);
            return new Result<object> { Value = null };
        }
        catch
        {
            return new Result<object> { IsError = true, Error = "A server error occurred" };
        }
    }

    public async Task<Result<object>> UpdateCharacterSavedDiceRollAsync(CharacterSavedDiceRollDto characterSavedDiceRoll)
    {
        try
        {
            var existing = await _context.CharacterSavedDiceRolls
                .FirstOrDefaultAsync(sr => sr.Id == characterSavedDiceRoll.Id);
            if (existing is null)
                return new Result<object> { IsError = true, Error = "Saved dice roll not found" };

            existing.Name = characterSavedDiceRoll.Name;
            existing.AttributeId = characterSavedDiceRoll.AttributeId;
            existing.SkillId = characterSavedDiceRoll.SkillId;

            await _context.SaveChangesAsync();
            await _broadcast.NotifyCharacterUpdatedAsync(existing.CharacterId);
            return new Result<object> { Value = null };
        }
        catch
        {
            return new Result<object> { IsError = true, Error = "A server error occurred" };
        }
    }

    public async Task<Result<object>> DeleteCharacterSavedDiceRollAsync(Guid characterSavedDiceRollId)
    {
        try
        {
            var existing = await _context.CharacterSavedDiceRolls
                .FirstOrDefaultAsync(sr => sr.Id == characterSavedDiceRollId);
            if (existing is null)
                return new Result<object> { IsError = true, Error = "Saved dice roll not found" };

            _context.CharacterSavedDiceRolls.Remove(existing);
            await _context.SaveChangesAsync();
            await _broadcast.NotifyCharacterUpdatedAsync(existing.CharacterId);
            return new Result<object> { Value = null };
        }
        catch
        {
            return new Result<object> { IsError = true, Error = "A server error occurred" };
        }
    }
}