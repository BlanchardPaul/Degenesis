using AutoMapper;
using DataAccessLayer;
using Degenesis.Shared.DTOs.Characters.CRUD;
using Degenesis.Shared.DTOs.Characters.Display;
using Domain.Characters;
using Domain.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Business.Characters;
public interface ICharacterService
{
    Task<CharacterDisplayDto?> GetCharacterByUserAndRoomAsync(Guid roomId, string userName);
    Task<List<CharacterDisplayDto>> GetAllCharactersAsync();
    Task<CharacterDisplayDto?> CreateCharacterAsync(CharacterCreateDto character, string userName);
    Task<bool> UpdateCharacterBasicInfosAsync(CharacterBasicInfosEditDto characterBasicInfosEditDto);
    Task<bool> UpdateCharacterChroniclerMoneyAsync(CharacterIntValueEditDto characterChroniclerMoney); 
    Task<bool> UpdateCharacterCurrentSporeInfestationAsync(CharacterIntValueEditDto characterCurrentSporeInfestation);
    Task<bool> UpdateCharacterEgoAsync(CharacterIntValueEditDto characterEgo); 
    Task<bool> UpdateCharacterFleshWoundsAsync(CharacterIntValueEditDto characterFleshWounds);
    Task<bool> UpdateCharacterDinarAsync(CharacterIntValueEditDto characterDinar);
    Task<bool> UpdateCharacterPermanentSporeInfestationAsync(CharacterIntValueEditDto characterPermanentSporeInfestation);
    Task<bool> UpdateCharacterRankAsync(CharacterGuidValueEditDto characterRank);
    Task<bool> UpdateCharacterTraumaAsync(CharacterIntValueEditDto characterTrauma);
    Task<bool> UpdateCharacterXpAsync(CharacterIntValueEditDto characterXp);
    Task<bool> DeleteCharacterAsync(Guid id);
}

public class CharacterService : ICharacterService
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly UserManager<ApplicationUser> _userManager;

    public CharacterService(ApplicationDbContext context, IMapper mapper, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _mapper = mapper;
        _userManager = userManager;
    }

    public async Task<CharacterDisplayDto?> GetCharacterByUserAndRoomAsync(Guid roomId, string userName)
    {
        var user = await _userManager.FindByNameAsync(userName) ?? throw new Exception("User not found");
        var character = await _context.Characters
            .Include(c => c.Cult)
            .Include(c => c.Culture)
            .Include(c => c.Concept)
            .Include(c => c.Room)
            .Include(c => c.Rank)
            .Include(c => c.CharacterAttributes)
                .ThenInclude(ca => ca.Attribute)
            .Include(c => c.CharacterSkills)
                .ThenInclude(cs => cs.Skill)
            .Include(c => c.CharacterBackgrounds)
                .ThenInclude(cb => cb.Background)
            .Include(c => c.CharacterPontentials)
                .ThenInclude(cp => cp.Potential)
            .FirstOrDefaultAsync(c => c.IdRoom == roomId && c.IdApplicationUser == user.Id);
        return _mapper.Map<CharacterDisplayDto>(character);
    }

    public async Task<List<CharacterDisplayDto>> GetAllCharactersAsync()
    {
        var characters = await _context.Characters
            .Include(c => c.Cult)
            .Include(c => c.Culture)
            .Include(c => c.Concept)
            .Include(c => c.Room)
            .Include(c => c.Rank)
            .Include(c => c.CharacterAttributes)
                .ThenInclude(ca => ca.Attribute)
            .Include(c => c.CharacterSkills)
                .ThenInclude(cs => cs.Skill)
            .Include(c => c.CharacterBackgrounds)
                .ThenInclude(cb => cb.Background)
            .Include(c => c.CharacterPontentials)
                .ThenInclude(cp => cp.Potential)
            .ToListAsync();
        return _mapper.Map<List<CharacterDisplayDto>>(characters);
    }

    public async Task<CharacterDisplayDto?> CreateCharacterAsync(CharacterCreateDto characterCreate, string userName)
    {
        try
        {
            var character = _mapper.Map<Character>(characterCreate);
            character.Id = Guid.NewGuid();

            var user = await _userManager.FindByNameAsync(userName) ?? throw new Exception("User not found");
            character.IdApplicationUser = user.Id;
            character.ApplicationUser = user;
            character.Room = await _context.Rooms.FindAsync(characterCreate.IdRoom) ?? throw new Exception("Room not found");
            character.Cult = await _context.Cults.FindAsync(characterCreate.CultId) ?? throw new Exception("Cult not found");
            character.Culture = await _context.Cultures.FindAsync(characterCreate.CultureId) ?? throw new Exception("Culture not found");
            character.Concept = await _context.Concepts.FindAsync(characterCreate.ConceptId) ?? throw new Exception("Concept not found");

            character.Rank = await _context.Ranks
                .FindAsync(characterCreate.RankId)
                ?? throw new Exception("Rank not found");

            _context.Characters.Add(character);

            foreach (var attrDto in characterCreate.Attributes)
            {
                var attribute = await _context.Attributes.FindAsync(attrDto.AttributeId)
                                ?? throw new Exception("Attribute not found");

                _context.CharacterAttributes.Add(new CharacterAttribute
                {
                    CharacterId = character.Id,
                    Character = character,
                    AttributeId = attrDto.AttributeId,
                    Attribute = attribute,
                    Level = attrDto.Level
                });
            }

            foreach (var skillDto in characterCreate.Skills)
            {
                var skill = await _context.Skills.FindAsync(skillDto.SkillId)
                            ?? throw new Exception("Skill not found");

                _context.CharacterSkills.Add(new CharacterSkill
                {
                    CharacterId = character.Id,
                    Character = character,
                    SkillId = skillDto.SkillId,
                    Skill = skill,
                    Level = skillDto.Level
                });
            }

            foreach (var bgDto in characterCreate.Backgrounds)
            {
                var background = await _context.Backgrounds.FindAsync(bgDto.BackgroundId)
                                 ?? throw new Exception("Background not found");

                _context.CharacterBackgrounds.Add(new CharacterBackground
                {
                    CharacterId = character.Id,
                    Character = character,
                    BackgroundId = bgDto.BackgroundId,
                    Background = background,
                    Level = bgDto.Level
                });
            }

            foreach (var cpDto in characterCreate.Potentials)
            {
                var potential = await _context.Potentials.FindAsync(cpDto.PotentialId)
                                ?? throw new Exception("Potential not found");

                _context.CharacterPotentials.Add(new CharacterPotential
                {
                    CharacterId = character.Id,
                    Character = character,
                    PotentialId = cpDto.PotentialId,
                    Potential = potential,
                    Level = cpDto.Level
                });
            }

            await _context.SaveChangesAsync();

            var characterWithRank = await _context.Characters
                .Include(c => c.Cult)
                .Include(c => c.Culture)
                .Include(c => c.Concept)
                .Include(c => c.Room)
                .Include(c => c.Rank)
                .FirstOrDefaultAsync(c => c.Id == character.Id)
                ?? throw new Exception("Character not found after creation");

            return _mapper.Map<CharacterDisplayDto>(characterWithRank);
        }
        catch (Exception)
        {
            return null;
        }
    }

    public async Task<bool> UpdateCharacterBasicInfosAsync(CharacterBasicInfosEditDto characterBasicInfosEditDto)
    {
        try
        {
            var existingCharacter = await _context.Characters
                .FirstOrDefaultAsync(c => c.Id == characterBasicInfosEditDto.Id) ?? throw new Exception("Character not found");

            _mapper.Map(characterBasicInfosEditDto, existingCharacter);

            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception)
        {
            return false;
        }

    }

    public async Task<bool> UpdateCharacterChroniclerMoneyAsync(CharacterIntValueEditDto characterChroniclerMoney)
    {
        try
        {
            var existingCharacter = await _context.Characters
                .FirstOrDefaultAsync(c => c.Id == characterChroniclerMoney.Id) ?? throw new Exception("Character not found");

            existingCharacter.ChroniclerMoney = characterChroniclerMoney.Value;

            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception)
        {
            return false;
        }

    }

    public async Task<bool> UpdateCharacterCurrentSporeInfestationAsync(CharacterIntValueEditDto characterCurrentSporeInfestation)
    {
        try
        {
            var existingCharacter = await _context.Characters
                .FirstOrDefaultAsync(c => c.Id == characterCurrentSporeInfestation.Id) ?? throw new Exception("Character not found");

            existingCharacter.CurrentSporeInfestation = characterCurrentSporeInfestation.Value;

            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public async Task<bool> UpdateCharacterDinarAsync(CharacterIntValueEditDto characterDinar)
    {
        try
        {
            var existingCharacter = await _context.Characters
                .FirstOrDefaultAsync(c => c.Id == characterDinar.Id) ?? throw new Exception("Character not found");

            existingCharacter.DinarMoney = characterDinar.Value;

            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception)
        {
            return false;
        }

    }

    public async Task<bool> UpdateCharacterEgoAsync(CharacterIntValueEditDto characterEgo)
    {
        try
        {
            var existingCharacter = await _context.Characters
                .FirstOrDefaultAsync(c => c.Id == characterEgo.Id) ?? throw new Exception("Character not found");

            existingCharacter.Ego = characterEgo.Value;

            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public async Task<bool> UpdateCharacterFleshWoundsAsync(CharacterIntValueEditDto characterFleshWounds)
    {
        try
        {
            var existingCharacter = await _context.Characters
                .FirstOrDefaultAsync(c => c.Id == characterFleshWounds.Id) ?? throw new Exception("Character not found");

            existingCharacter.FleshWounds = characterFleshWounds.Value;

            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public async Task<bool> UpdateCharacterPermanentSporeInfestationAsync(CharacterIntValueEditDto characterPermanentSporeInfestation)
    {
        try
        {
            var existingCharacter = await _context.Characters
                .FirstOrDefaultAsync(c => c.Id == characterPermanentSporeInfestation.Id) ?? throw new Exception("Character not found");

            existingCharacter.PermanentSporeInfestation = characterPermanentSporeInfestation.Value;

            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public async Task<bool> UpdateCharacterRankAsync(CharacterGuidValueEditDto characterRank)
    {
        try
        {
            var existingCharacter = await _context.Characters
                .Include(c => c.Rank)
                .FirstOrDefaultAsync(c => c.Id == characterRank.Id) ?? throw new Exception("Character not found");

            var newRank = await _context.Ranks
                .FirstOrDefaultAsync(r => r.Id == characterRank.Value) ?? throw new Exception("Rank not found");

            existingCharacter.Rank = newRank;
            existingCharacter.RankId = characterRank.Value;

            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public async Task<bool> UpdateCharacterTraumaAsync(CharacterIntValueEditDto characterTrauma)
    {
        try
        {
            var existingCharacter = await _context.Characters
                .FirstOrDefaultAsync(c => c.Id == characterTrauma.Id) ?? throw new Exception("Character not found");

            existingCharacter.Trauma = characterTrauma.Value;

            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public async Task<bool> UpdateCharacterXpAsync(CharacterIntValueEditDto characterXp)
    {
        try
        {
            var existingCharacter = await _context.Characters
                .FirstOrDefaultAsync(c => c.Id == characterXp.Id) ?? throw new Exception("Character not found");

            existingCharacter.Experience = characterXp.Value;

            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception)
        {
            return false;
        }

    }

    public async Task<bool> DeleteCharacterAsync(Guid id)
    {
        var character = await _context.Characters
               .Include(c => c.CharacterAttributes)
               .Include(c => c.CharacterSkills)
               .Include(c => c.CharacterBackgrounds)
               .Include(c => c.CharacterPontentials)
               .FirstOrDefaultAsync(c => c.Id == id) ?? throw new Exception("Character not found");

        // Here we have to delete the CharacterAttributes manually because we can't put an OnDelete.Cascade in the configuration
        _context.CharacterAttributes.RemoveRange(character.CharacterAttributes);

        _context.Characters.Remove(character);
        await _context.SaveChangesAsync();
        return true;
    }
}
