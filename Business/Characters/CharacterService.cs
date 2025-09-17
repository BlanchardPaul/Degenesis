using AutoMapper;
using DataAccessLayer;
using Degenesis.Shared.DTOs.Characters;
using Domain.Characters;
using Domain.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Business.Characters;
public interface ICharacterService
{
    Task<Character?> GetCharacterByIdAsync(Guid id);
    Task<IEnumerable<Character>> GetAllCharactersAsync();
    Task<CharacterDto?> CreateCharacterAsync(CharacterCreateDto character, string userName);
    Task<bool> UpdateCharacterAsync(Guid id, Character character);
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

    public async Task<CharacterDto?> CreateCharacterAsync(CharacterCreateDto characterCreate, string userName)
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

            await _context.SaveChangesAsync();

            return _mapper.Map<CharacterDto>(character);
        }
        catch (Exception)
        {
            return null;
        }
    }

    public async Task<bool> UpdateCharacterAsync(Guid id, Character character)
    {
        var existingCharacter = await _context.Characters.FindAsync(id);
        if (existingCharacter is null)
            return false;

        _context.Entry(existingCharacter).CurrentValues.SetValues(character);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteCharacterAsync(Guid id)
    {
        var character = await _context.Characters.FindAsync(id);
        if (character is null)
            return false;

        _context.Characters.Remove(character);
        await _context.SaveChangesAsync();
        return true;
    }
}
