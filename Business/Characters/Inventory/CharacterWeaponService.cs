using AutoMapper;
using DataAccessLayer;
using Degenesis.Shared.DTOs.Characters.CRUD.Inventory;
using Domain.Characters.Inventory;
using Microsoft.EntityFrameworkCore;

namespace Business.Characters.Inventory;

public interface ICharacterWeaponService
{
    Task<List<CharacterWeaponDto>> GetByCharacterIdAsync(Guid characterId);
    Task<CharacterWeaponDto?> CreateAsync(CharacterWeaponCreateDto characterWeapon);
    Task<bool> UpdateAsync(CharacterWeaponDto characterWeapon);
    Task<bool> DeleteAsync(Guid id);
}


public class CharacterWeaponService : ICharacterWeaponService
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public CharacterWeaponService(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<CharacterWeaponDto>> GetByCharacterIdAsync(Guid characterId)
    {
        var characterWeapons =
            await _context.CharacterWeapons
            .Where(cv => cv.CharacterId == characterId)
            .Include(cv => cv.Weapon)
                .ThenInclude(v => v.WeaponType)
            .Include(cv => cv.Weapon)
                .ThenInclude(v => v.Cults)
            .ToListAsync();
        return _mapper.Map<List<CharacterWeaponDto>>(characterWeapons);
    }

    public async Task<CharacterWeaponDto?> CreateAsync(CharacterWeaponCreateDto characterWeaponCreate)
    {
        try
        {
            var characterWeapon = _mapper.Map<CharacterWeapon>(characterWeaponCreate);
            characterWeapon.Character = await _context.Characters.FindAsync(characterWeaponCreate.CharacterId)
                ?? throw new Exception("Character not found");
            characterWeapon.Weapon = await _context.Weapons.FindAsync(characterWeaponCreate.WeaponId)
                ?? throw new Exception("Weapon not found");
            // Set the BulletsInMagazine, Slots, Encumbrance, Qualities based on the Weapon entity
            characterWeapon.BulletsInMagazine = characterWeapon.Weapon.Magazine;
            characterWeapon.UsedSlots = 0;
            characterWeapon.Slots = characterWeapon.Weapon.Slots;
            characterWeapon.Encumbrance = characterWeapon.Weapon.Encumbrance;
            characterWeapon.Qualities = characterWeapon.Weapon.Qualities;
            _context.CharacterWeapons.Add(characterWeapon);
            await _context.SaveChangesAsync();
            return _mapper.Map<CharacterWeaponDto>(characterWeapon);
        }
        catch (Exception)
        {
            return null;
        }
    }

    public async Task<bool> UpdateAsync(CharacterWeaponDto characterWeaponDto)
    {
        try
        {
            var characterWeapon = await _context.CharacterWeapons.FindAsync(characterWeaponDto.Id);
            if (characterWeapon == null) return false;
            _mapper.Map(characterWeaponDto, characterWeapon);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        try
        {
            var characterWeapon = await _context.CharacterWeapons.FindAsync(id);
            if (characterWeapon  == null) return false;
            _context.CharacterWeapons.Remove(characterWeapon);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
}
