using AutoMapper;
using Business.Hubs;
using DataAccessLayer;
using Degenesis.Shared.DTOs;
using Degenesis.Shared.DTOs.Characters.CRUD;
using Degenesis.Shared.DTOs.Characters.Display;
using Domain.Characters;
using Domain.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Business.Characters;
public interface ICharacterService
{
    Task<Result<CharacterDisplayDto>> GetCharacterByUserAndRoomAsync(Guid roomId, string userName);
    Task<Result<List<CharacterDisplayForGMListDto>>> GetAllCharactersInRoomAsync(Guid roomId, string userName);
    Task<Result<CharacterDisplayDto>> GetCharacterByIdForGMAsync(Guid characterId, string userName);
    Task<Result<object>> CreateCharacterAsync(CharacterCreateDto character, string userName);
    Task<Result<object>> UpdateCharacterBasicInfosAsync(CharacterBasicInfosEditDto characterBasicInfosEditDto);
    Task<Result<object>> UpdateCharacterChroniclerMoneyAsync(IdIntEditDto characterChroniclerMoney); 
    Task<Result<object>> UpdateCharacterCurrentSporeInfestationAsync(IdIntEditDto characterCurrentSporeInfestation);
    Task<Result<object>> UpdateCharacterEgoAsync(IdIntEditDto characterEgo); 
    Task<Result<object>> UpdateCharacterFleshWoundsAsync(IdIntEditDto characterFleshWounds);
    Task<Result<object>> UpdateCharacterDinarAsync(IdIntEditDto characterDinar);
    Task<Result<object>> UpdateCharacterNotesAsync(IdStringEditDto characterNotes);
    Task<Result<object>> UpdateCharacterInventoryNotesAsync(IdStringEditDto characterInventoryNotes);
    Task<Result<object>> UpdateCharacterPermanentSporeInfestationAsync(IdIntEditDto characterPermanentSporeInfestation);
    Task<Result<object>> UpdateCharacterRankAsync(IdGuidEditDto characterRank);
    Task<Result<object>> UpdateCharacterTraumaAsync(IdIntEditDto characterTrauma);
    Task<Result<object>> UpdateCharacterXpAsync(IdIntEditDto characterXp);
    Task<Result<object>> DeleteCharacterAsync(Guid roomId, string userName);
}

public class CharacterService(ApplicationDbContext context, IMapper mapper, UserManager<ApplicationUser> userManager, IRoomBroadcastService broadcast) : ICharacterService
{
    private readonly ApplicationDbContext _context = context;
    private readonly IMapper _mapper = mapper;
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly IRoomBroadcastService _broadcast = broadcast;

    public async Task<Result<CharacterDisplayDto>> GetCharacterByUserAndRoomAsync(Guid roomId, string userName)
    {
        try
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user is null)
                return new Result<CharacterDisplayDto> { IsError = true, Error = "User not found" };
            var character = await _context.Characters
                .AsSplitQuery()
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
                .Include(c => c.CharacterArtifacts)
                    .ThenInclude(ca => ca.Artifact)
                .Include(c => c.CharacterBurns)
                    .ThenInclude(cb => cb.Burn)
                .Include(c => c.CharacterEquipments)
                    .ThenInclude(ce => ce.Equipment)
                        .ThenInclude(e => e.EquipmentType)
                .Include(c => c.CharacterEquipments)
                    .ThenInclude(ce => ce.Equipment)
                        .ThenInclude(e => e.Cults)
                .Include(c => c.CharacterProtections)
                    .ThenInclude(cp => cp.Protection)
                        .ThenInclude(p => p.Cults)
                .Include(c => c.CharacterVehicles)
                    .ThenInclude(cv => cv.Vehicle)
                        .ThenInclude(v => v.VehicleType)
                .Include(c => c.CharacterVehicles)
                    .ThenInclude(cv => cv.Vehicle)
                        .ThenInclude(v => v.Cult)
                .Include(c => c.CharacterWeapons)
                    .ThenInclude(cw => cw.Weapon)
                        .ThenInclude(w => w.WeaponType)
                .Include(c => c.CharacterWeapons)
                    .ThenInclude(cw => cw.Weapon)
                        .ThenInclude(w => w.Cults)
                .FirstOrDefaultAsync(c => c.IdRoom == roomId && c.IdApplicationUser == user.Id);
            if (character is null)
                return new Result<CharacterDisplayDto> { IsError = true, Error = "Character not found" };

            return new Result<CharacterDisplayDto> { Value = _mapper.Map<CharacterDisplayDto>(character) };
        }
        catch(Exception)
        {
            return new Result<CharacterDisplayDto> { IsError = true, Error = "A server error occurred" };
        }
    }

    public async Task<Result<List<CharacterDisplayForGMListDto>>> GetAllCharactersInRoomAsync(Guid roomId, string userName)
    {
        try
        {
            List<CharacterDisplayForGMListDto> characterList = [];
            var user = await _userManager.FindByNameAsync(userName);
            if (user is null)
                return new Result<List<CharacterDisplayForGMListDto>> { IsError = true, Error = "User not found" };
            var userRoom = _context.UserRooms.FirstOrDefault(ur => ur.IdApplicationUser == user.Id && ur.IdRoom == roomId);
            if(userRoom is null)
                return new Result<List<CharacterDisplayForGMListDto>> { IsError = true, Error = "User Room not found" };
            if (!userRoom.IsGM)
                return new Result<List<CharacterDisplayForGMListDto>> { IsError = true, Error = "Complete characters list available for the GM only" };
            var characters = _context.Characters.Where(c => c.IdRoom == roomId).ToList();
            foreach (var character in characters)
            {
                var characterOwner = _context.Users.Find(character.IdApplicationUser);
                if (characterOwner is null)
                    return new Result<List<CharacterDisplayForGMListDto>> { IsError = true, Error = "A character's owner was not found" };
                characterList.Add(new CharacterDisplayForGMListDto
                {
                    Id = character.Id,
                    Name = character.Name,
                    PlayerName = characterOwner.UserName!
                });
            }
            return new Result<List<CharacterDisplayForGMListDto>> { Value = characterList };
        }
        catch (Exception)
        {
            return new Result<List<CharacterDisplayForGMListDto>> { IsError = true, Error = "A server error occurred" };
        }
    }

    public async Task<Result<CharacterDisplayDto>> GetCharacterByIdForGMAsync(Guid characterId, string userName)
    {
        try
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user is null)
                return new Result<CharacterDisplayDto> { IsError = true, Error = "User not found" };

            var characterRoomId = await _context.Characters
                .Where(c => c.Id == characterId)
                .Select(c => c.IdRoom)
                .FirstOrDefaultAsync();

            var userRoom = await _context.UserRooms
                .FirstOrDefaultAsync(ur => ur.IdApplicationUser == user.Id && ur.IdRoom == characterRoomId);
            if (userRoom is null || !userRoom.IsGM)
                return new Result<CharacterDisplayDto> { IsError = true, Error = "Character available for the GM of the room only" };

            var character = await _context.Characters
                            .AsSplitQuery()
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
                            .Include(c => c.CharacterArtifacts)
                                .ThenInclude(ca => ca.Artifact)
                            .Include(c => c.CharacterBurns)
                                .ThenInclude(cb => cb.Burn)
                            .Include(c => c.CharacterEquipments)
                                .ThenInclude(ce => ce.Equipment)
                                    .ThenInclude(e => e.EquipmentType)
                            .Include(c => c.CharacterEquipments)
                                .ThenInclude(ce => ce.Equipment)
                                    .ThenInclude(e => e.Cults)
                            .Include(c => c.CharacterProtections)
                                .ThenInclude(cp => cp.Protection)
                                    .ThenInclude(p => p.Cults)
                            .Include(c => c.CharacterVehicles)
                                .ThenInclude(cv => cv.Vehicle)
                                    .ThenInclude(v => v.VehicleType)
                            .Include(c => c.CharacterVehicles)
                                .ThenInclude(cv => cv.Vehicle)
                                    .ThenInclude(v => v.Cult)
                            .Include(c => c.CharacterWeapons)
                                .ThenInclude(cw => cw.Weapon)
                                    .ThenInclude(w => w.WeaponType)
                            .Include(c => c.CharacterWeapons)
                                .ThenInclude(cw => cw.Weapon)
                                    .ThenInclude(w => w.Cults)
                            .FirstOrDefaultAsync(c => c.Id == characterId);

            if (character is null)
                return new Result<CharacterDisplayDto> { IsError = true, Error = "Character not found" };

            return new Result<CharacterDisplayDto> { Value = _mapper.Map<CharacterDisplayDto>(character) };
        }
        catch (Exception)
        {
            return new Result<CharacterDisplayDto> { IsError = true, Error = "A server error occurred" };
        }
    }

    public async Task<Result<object>> CreateCharacterAsync(CharacterCreateDto characterCreate, string userName)
    {
        try
        {
            var character = _mapper.Map<Character>(characterCreate);
            character.Id = Guid.NewGuid();

            var user = await _userManager.FindByNameAsync(userName);
            if (user is null)
                return new Result<object> { IsError = true, Error = "User not found" };

            character.IdApplicationUser = user.Id;
            character.ApplicationUser = user;

            var room = await _context.Rooms.FindAsync(characterCreate.IdRoom);
            if (room is null)
                return new Result<object> { IsError = true, Error = "Room not found" };

            var cult = await _context.Cults.FindAsync(characterCreate.CultId);
            if (cult is null)
                return new Result<object> { IsError = true, Error = "Cult not found" };

            var culture = await _context.Cultures.FindAsync(characterCreate.CultureId);
            if (culture is null)
                return new Result<object> { IsError = true, Error = "Culture not found" };
                
            var concept = await _context.Concepts.FindAsync(characterCreate.ConceptId);
            if (concept is null)
                return new Result<object> { IsError = true, Error = "Concept not found" };

            var rank = await _context.Ranks.FindAsync(characterCreate.RankId);
            if (rank is null)
                return new Result<object> { IsError = true, Error = "Rank not found" };

            character.Room = room;
            character.Cult = cult;
            character.Culture = culture;
            character.Concept = concept;
            character.Rank = rank;

            _context.Characters.Add(character);

            foreach (var attrDto in characterCreate.Attributes)
            {
                var attribute = await _context.Attributes.FindAsync(attrDto.AttributeId);
                if (attribute is null)
                    return new Result<object> { IsError = true, Error = "Attribute not found" };

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
                var skill = await _context.Skills.FindAsync(skillDto.SkillId);
                if (skill is null)
                    return new Result<object> { IsError = true, Error = "Skill not found" };

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
                var background = await _context.Backgrounds.FindAsync(bgDto.BackgroundId);
                if (background is null)
                    return new Result<object> { IsError = true, Error = "Background not found" };

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
                var potential = await _context.Potentials.FindAsync(cpDto.PotentialId);
                if (potential is null)
                    return new Result<object> { IsError = true, Error = "Potential not found" };

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

            return new Result<object> { Value = null };
        }
        catch (Exception)
        {
            return new Result<object> { IsError = true, Error = "A server error occurred" };
        }
    }

    public async Task<Result<object>> UpdateCharacterBasicInfosAsync(CharacterBasicInfosEditDto characterBasicInfosEditDto)
    {
        try
        {
            var existingCharacter = await _context.Characters
                .FirstOrDefaultAsync(c => c.Id == characterBasicInfosEditDto.Id);
            if (existingCharacter is null)
                return new Result<object> { IsError = true, Error = "Character not found" };

            _mapper.Map(characterBasicInfosEditDto, existingCharacter);

            await _context.SaveChangesAsync();
            await _broadcast.NotifyCharacterUpdatedAsync(characterBasicInfosEditDto.Id);
            return new Result<object> { Value = null};
        }
        catch (Exception)
        {
            return new Result<object> { IsError = true, Error = "A server error occurred" };
        }

    }

    public async Task<Result<object>> UpdateCharacterChroniclerMoneyAsync(IdIntEditDto characterChroniclerMoney)
    {
        try
        {
            var existingCharacter = await _context.Characters
                .FirstOrDefaultAsync(c => c.Id == characterChroniclerMoney.Id);
            if (existingCharacter is null)
                return new Result<object> { IsError = true, Error = "Character not found" };

            existingCharacter.ChroniclerMoney = characterChroniclerMoney.Value;

            await _context.SaveChangesAsync();
            await _broadcast.NotifyCharacterUpdatedAsync(characterChroniclerMoney.Id);
            return new Result<object> { Value = null};
        }
        catch (Exception)
        {
            return new Result<object> { IsError = true , Error = "A server error occured"};
        }

    }

    public async Task<Result<object>> UpdateCharacterCurrentSporeInfestationAsync(IdIntEditDto characterCurrentSporeInfestation)
    {
        try
        {
            var existingCharacter = await _context.Characters
                .FirstOrDefaultAsync(c => c.Id == characterCurrentSporeInfestation.Id);
            if (existingCharacter is null)
                return new Result<object> { IsError = true, Error = "Character not found" };

            existingCharacter.CurrentSporeInfestation = characterCurrentSporeInfestation.Value;

            await _context.SaveChangesAsync();
            await _broadcast.NotifyCharacterUpdatedAsync(characterCurrentSporeInfestation.Id);
            return new Result<object> { Value = null };
        }
        catch (Exception)
        {
            return new Result<object> { IsError = true, Error = "A server error occurred" };
        }
    }

    public async Task<Result<object>> UpdateCharacterDinarAsync(IdIntEditDto characterDinar)
    {
        try
        {
            var existingCharacter = await _context.Characters
                .FirstOrDefaultAsync(c => c.Id == characterDinar.Id);
            if (existingCharacter is null)
                return new Result<object> { IsError = true, Error = "Character not found" };

            existingCharacter.DinarMoney = characterDinar.Value;

            await _context.SaveChangesAsync();
            await _broadcast.NotifyCharacterUpdatedAsync(characterDinar.Id);
            return new Result<object> { Value = null };
        }
        catch (Exception)
        {
            return new Result<object> { IsError = true, Error = "A server error occurred" };
        }

    }

    public async Task<Result<object>> UpdateCharacterEgoAsync(IdIntEditDto characterEgo)
    {
        try
        {
            var existingCharacter = await _context.Characters
                .FirstOrDefaultAsync(c => c.Id == characterEgo.Id);
            if (existingCharacter is null)
                return new Result<object> { IsError = true, Error = "Character not found" };

            existingCharacter.Ego = characterEgo.Value;

            await _context.SaveChangesAsync();
            await _broadcast.NotifyCharacterUpdatedAsync(characterEgo.Id);
            return new Result<object> { Value = null };
        }
        catch (Exception)
        {
            return new Result<object> { IsError = true, Error = "A server error occurred" };
        }
    }

    public async Task<Result<object>> UpdateCharacterFleshWoundsAsync(IdIntEditDto characterFleshWounds)
    {
        try
        {
            var existingCharacter = await _context.Characters
                .FirstOrDefaultAsync(c => c.Id == characterFleshWounds.Id);
            if (existingCharacter is null)
                return new Result<object> { IsError = true, Error = "Character not found" };

            existingCharacter.FleshWounds = characterFleshWounds.Value;

            await _context.SaveChangesAsync();
            await _broadcast.NotifyCharacterUpdatedAsync(characterFleshWounds.Id);
            return new Result<object> { Value = null };
        }
        catch (Exception)
        {
            return new Result<object> { IsError = true, Error = "A server error occurred" };
        }
    }

    public async Task<Result<object>> UpdateCharacterNotesAsync(IdStringEditDto characterNotes)
    {
        try
        {
            var existingCharacter = await _context.Characters
                .FirstOrDefaultAsync(c => c.Id == characterNotes.Id);
            if (existingCharacter is null)
                return new Result<object> { IsError = true, Error = "Character not found" };

            existingCharacter.Notes = characterNotes.Value;

            await _context.SaveChangesAsync();
            await _broadcast.NotifyCharacterUpdatedAsync(characterNotes.Id);
            return new Result<object> { Value = null };
        }
        catch (Exception)
        {
            return new Result<object> { IsError = true, Error = "A server error occurred" };
        }
    }

    public async Task<Result<object>> UpdateCharacterInventoryNotesAsync(IdStringEditDto characterInventoryNotes)
    {
        try
        {
            var existingCharacter = await _context.Characters
                .FirstOrDefaultAsync(c => c.Id == characterInventoryNotes.Id);
            if (existingCharacter is null)
                return new Result<object> { IsError = true, Error = "Character not found" };

            existingCharacter.InventoryNotes = characterInventoryNotes.Value;
            await _context.SaveChangesAsync();
            await _broadcast.NotifyCharacterUpdatedAsync(characterInventoryNotes.Id);
            return new Result<object> { Value = null };
        }
        catch (Exception)
        {
            return new Result<object> { IsError = true, Error = "A server error occurred" };
        }
    }

    public async Task<Result<object>> UpdateCharacterPermanentSporeInfestationAsync(IdIntEditDto characterPermanentSporeInfestation)
    {
        try
        {
            var existingCharacter = await _context.Characters
                .FirstOrDefaultAsync(c => c.Id == characterPermanentSporeInfestation.Id);
            if (existingCharacter is null)
                return new Result<object> { IsError = true, Error = "Character not found" };

            existingCharacter.PermanentSporeInfestation = characterPermanentSporeInfestation.Value;

            await _context.SaveChangesAsync();
            await _broadcast.NotifyCharacterUpdatedAsync(characterPermanentSporeInfestation.Id);
            return new Result<object> { Value = null };
        }
        catch (Exception)
        {
            return new Result<object> { IsError = true, Error = "A server error occurred" };
        }
    }

    public async Task<Result<object>> UpdateCharacterRankAsync(IdGuidEditDto characterRank)
    {
        try
        {
            var existingCharacter = await _context.Characters
                .Include(c => c.Rank)
                .FirstOrDefaultAsync(c => c.Id == characterRank.Id);
            if (existingCharacter is null)
                return new Result<object> { IsError = true, Error = "Character not found" };

            var newRank = await _context.Ranks
                .FirstOrDefaultAsync(r => r.Id == characterRank.Value);
            if (newRank is null)
                return new Result<object> { IsError = true, Error = "Rank not found" };

            existingCharacter.Rank = newRank;
            existingCharacter.RankId = characterRank.Value;

            await _context.SaveChangesAsync();
            await _broadcast.NotifyCharacterUpdatedAsync(characterRank.Id);
            return new Result<object> { Value = null };
        }
        catch (Exception)
        {
            return new Result<object> { IsError = true, Error = "A server error occurred" };
        }
    }

    public async Task<Result<object>> UpdateCharacterTraumaAsync(IdIntEditDto characterTrauma)
    {
        try
        {
            var existingCharacter = await _context.Characters
                .FirstOrDefaultAsync(c => c.Id == characterTrauma.Id);
            if (existingCharacter is null)
                return new Result<object> { IsError = true, Error = "Character not found" };

            existingCharacter.Trauma = characterTrauma.Value;

            await _context.SaveChangesAsync();
            await _broadcast.NotifyCharacterUpdatedAsync(characterTrauma.Id);
            return new Result<object> { Value = null };
        }
        catch (Exception)
        {
            return new Result<object> { IsError = true, Error = "A server error occurred" };
        }
    }

    public async Task<Result<object>> UpdateCharacterXpAsync(IdIntEditDto characterXp)
    {
        try
        {
            var existingCharacter = await _context.Characters
                .FirstOrDefaultAsync(c => c.Id == characterXp.Id);
            if (existingCharacter is null)
                return new Result<object> { IsError = true, Error = "Character not found" };

            existingCharacter.Experience = characterXp.Value;

            await _context.SaveChangesAsync();
            await _broadcast.NotifyCharacterUpdatedAsync(characterXp.Id);
            return new Result<object> { Value = null };
        }
        catch (Exception)
        {
            return new Result<object> { IsError = true, Error = "A server error occurred" };
        }

    }

    public async Task<Result<object>> DeleteCharacterAsync(Guid roomId, string userName)
    {
        var user = await _userManager.FindByNameAsync(userName);
        if (user is null)
            return new Result<object> { IsError = true, Error = "User not found" };

        var character = await _context.Characters
               .Include(c => c.CharacterAttributes)
               .Include(c => c.CharacterSkills)
               .Include(c => c.CharacterBackgrounds)
               .Include(c => c.CharacterPontentials)
               .FirstOrDefaultAsync(c => c.IdRoom == roomId && c.IdApplicationUser == user.Id);
        if (character is null)
            return new Result<object> { IsError = true, Error = "Character not found" };

        // Here we have to delete the CharacterAttributes manually because we can't put an OnDelete.Cascade in the configuration
        _context.CharacterAttributes.RemoveRange(character.CharacterAttributes);

        _context.Characters.Remove(character);
        await _context.SaveChangesAsync();
        return new Result<object> { Value = null };
    }
}
