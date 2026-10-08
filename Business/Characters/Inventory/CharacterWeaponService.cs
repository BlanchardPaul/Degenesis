using AutoMapper;
using DataAccessLayer;
using Degenesis.Shared.DTOs;
using Degenesis.Shared.DTOs.Characters.CRUD.Inventory;
using Domain.Characters.Inventory;
using Microsoft.EntityFrameworkCore;

namespace Business.Characters.Inventory;

public interface ICharacterWeaponService
{
    Task<Result<List<CharacterWeaponDto>>> GetByCharacterIdAsync(Guid characterId);
    Task<Result<object>> CreateAsync(CharacterWeaponCreateDto characterWeapon);
    Task<Result<object>> UpdateAsync(CharacterWeaponDto characterWeapon);
    Task<Result<object>> DeleteAsync(Guid id);
}


public class CharacterWeaponService(ApplicationDbContext context, IMapper mapper) : ICharacterWeaponService
{
    private readonly ApplicationDbContext _context = context;
    private readonly IMapper _mapper = mapper;

    public async Task<Result<List<CharacterWeaponDto>>> GetByCharacterIdAsync(Guid characterId)
    {
        try
        {
            var characterWeapons =
                await _context.CharacterWeapons
                .Where(cv => cv.CharacterId == characterId)
                .Include(cv => cv.Weapon)
                    .ThenInclude(v => v.WeaponType)
                .Include(cv => cv.Weapon)
                    .ThenInclude(v => v.Cults)
                .ToListAsync();
            return new Result<List<CharacterWeaponDto>> { Value = _mapper.Map<List<CharacterWeaponDto>>(characterWeapons) };
        }
        catch (Exception)
        {
            return new Result<List<CharacterWeaponDto>> { IsError = true, Error = "A server error occurred" };
        }
    }

    public async Task<Result<object>> CreateAsync(CharacterWeaponCreateDto characterWeaponCreate)
    {
        try
        {
            var characterWeapon = _mapper.Map<CharacterWeapon>(characterWeaponCreate);

            var existingCharacter = await _context.Characters.FindAsync(characterWeapon.CharacterId);
            if (existingCharacter is null)
                return new Result<object> { IsError = true, Error = "Character not found" };

            var existingWeapon = await _context.Weapons.FindAsync(characterWeapon.WeaponId);
            if (existingWeapon is null)
                return new Result<object> { IsError = true, Error = "Weapon not found" };

            characterWeapon.Character = existingCharacter;
            characterWeapon.Weapon = existingWeapon;

            // Set the BulletsInMagazine, Slots, Encumbrance, Qualities based on the Weapon entity
            characterWeapon.BulletsInMagazine = characterWeapon.Weapon.Magazine;
            characterWeapon.UsedSlots = 0;
            characterWeapon.Slots = characterWeapon.Weapon.Slots;
            characterWeapon.Encumbrance = characterWeapon.Weapon.Encumbrance;
            characterWeapon.Qualities = characterWeapon.Weapon.Qualities;

            _context.CharacterWeapons.Add(characterWeapon);
            await _context.SaveChangesAsync();

            return new Result<object> { Value = null };
        }
        catch (Exception)
        {
            return new Result<object> { IsError = true, Error = "A server error occurred" };
        }
    }

    public async Task<Result<object>> UpdateAsync(CharacterWeaponDto characterWeaponDto)
    {
        try
        {
            var characterWeapon = await _context.CharacterWeapons.FindAsync(characterWeaponDto.Id);
            if (characterWeapon is null) 
                return new Result<object> { IsError = true, Error = "CharacterWeapon not found" };

            _mapper.Map(characterWeaponDto, characterWeapon);
            await _context.SaveChangesAsync();
            return new Result<object> { Value = null };
        }
        catch (Exception)
        {
            return new Result<object> { IsError = true, Error = "A server error occurred" };
        }
    }

    public async Task<Result<object>> DeleteAsync(Guid id)
    {
        try
        {
            var characterWeapon = await _context.CharacterWeapons.FindAsync(id);
            if (characterWeapon  is null) 
                return new Result<object> { IsError = true, Error = "CharacterWeapon not found" };
            _context.CharacterWeapons.Remove(characterWeapon);
            await _context.SaveChangesAsync();
            return new Result<object> { Value = null };
        }
        catch (Exception)
        {
            return new Result<object> { IsError = true, Error = "A server error occurred" };
        }
    }
}
