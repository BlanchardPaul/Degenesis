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
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<IEnumerable<Character>> GetAllCharactersAsync()
    {
        return await _context.Characters
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

            character.Rank = await _context.Ranks
                .Include(r => r.Prerequisites)
                    .ThenInclude(rp => rp.AttributeRequired)
                .Include(r => r.Prerequisites)
                    .ThenInclude(rp => rp.SkillRequired)
                .Include(r => r.Prerequisites)
                    .ThenInclude(rp => rp.BackgroundRequired)
                .FirstOrDefaultAsync(r => r.Id == characterCreate.RankId)
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
                    .ThenInclude(r => r.Prerequisites)
                        .ThenInclude(rp => rp.AttributeRequired)
                .Include(c => c.Rank)
                    .ThenInclude(r => r.Prerequisites)
                        .ThenInclude(rp => rp.SkillRequired)
                .Include(c => c.Rank)
                    .ThenInclude(r => r.Prerequisites)
                        .ThenInclude(rp => rp.BackgroundRequired)
                .FirstOrDefaultAsync(c => c.Id == character.Id)
                ?? throw new Exception("Character not found after creation");

            return _mapper.Map<CharacterDto>(characterWithRank);
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
        var character = await _context.Characters
               .Include(c => c.CharacterAttributes)
               .Include(c => c.CharacterSkills)
               .Include(c => c.CharacterBackgrounds)
               .Include(c => c.CharacterPontentials)
               .FirstOrDefaultAsync(c => c.Id == id);

        if (character is null)
            return false;

        // Here we have to delete the CharacterAttributes manually because we can't put an OnDelete.Cascade in the configuration
        _context.CharacterAttributes.RemoveRange(character.CharacterAttributes);

        _context.Characters.Remove(character);
        await _context.SaveChangesAsync();
        return true;
    }
}
