using AutoMapper;
using DataAccessLayer;
using Degenesis.Shared.DTOs;
using Degenesis.Shared.DTOs.Weapons;
using Domain.Weapons;
using Microsoft.EntityFrameworkCore;

namespace Business.Weapons;

public interface IWeaponService
{
    Task<Result<List<WeaponDto>>> GetAllWeaponsAsync();
    Task<Result<WeaponDto>> GetWeaponByIdAsync(Guid id);
    Task<Result<object>> CreateWeaponAsync(WeaponCreateDto weaponCreate);
    Task<Result<object>> UpdateWeaponAsync(WeaponDto weapon);
    Task<Result<object>> DeleteWeaponAsync(Guid id);
}

public class WeaponService(ApplicationDbContext context, IMapper mapper) : IWeaponService
{
    private readonly ApplicationDbContext _context = context;
    private readonly IMapper _mapper = mapper;

    public async Task<Result<List<WeaponDto>>> GetAllWeaponsAsync()
    {
        try
        {
            var weapons = await _context.Weapons
                .Include(w => w.WeaponType)
                .Include(w => w.Attribute)
                .Include(w => w.Skill)
                .Include(w => w.Cults)
                .OrderBy(w => w.Name)
                .ToListAsync();
            return new Result<List<WeaponDto>> { Value = _mapper.Map<List<WeaponDto>>(weapons) };
        }
        catch (Exception)
        {
            return new Result<List<WeaponDto>> { IsError = true, Error = "A server error occurred" };
        }

    }

    public async Task<Result<WeaponDto>> GetWeaponByIdAsync(Guid id)
    {
        try
        {
            var weapon = await _context.Weapons
                .Include(w => w.WeaponType)
                .Include(w => w.Attribute)
                .Include(w => w.Skill)
                .Include(e => e.Cults)
                .FirstOrDefaultAsync(w => w.Id == id);
            if (weapon is null)
                return new Result<WeaponDto> { IsError = true, Error = "Weapon not found" };

            return new Result<WeaponDto> { Value = _mapper.Map<WeaponDto>(weapon) };
        }
        catch (Exception)
        {
            return new Result<WeaponDto> { IsError = true, Error = "A server error occurred" };
        }
    }

    public async Task<Result<object>> CreateWeaponAsync(WeaponCreateDto weaponCreate)
    {
        try
        {
            var weapon = _mapper.Map<Weapon>(weaponCreate);

            var weaponType = await _context.WeaponTypes
                .FirstOrDefaultAsync(wt => wt.Id == weaponCreate.WeaponTypeId);
            if (weaponType is null)
                return new Result<object> { IsError = true, Error = "Weapon Type not found" };
            weapon.WeaponType = weaponType;

            if(weaponCreate.AttributeId is not null)
            {
                var attribute = await _context.Attributes.FindAsync(weaponCreate.AttributeId.Value);
                if (attribute is null)
                    return new Result<object> { IsError = true, Error = "Attribute not found" };
                weapon.Attribute = attribute;
            }

            if (weaponCreate.SkillId is not null)
            {
                var skill = await _context.Skills.FindAsync(weaponCreate.SkillId.Value);
                if (skill is null)
                    return new Result<object> { IsError = true, Error = "Attribute not found" };
                weapon.Skill = skill;
            }


            foreach (var cultDto in weaponCreate.Cults)
            {
                var cult = await _context.Cults
                    .FirstOrDefaultAsync(c => c.Id == cultDto.Id);
                if (cult is null)
                    return new Result<object> { IsError = true, Error = "Cult not found" };
                weapon.Cults.Add(cult);
            }

            _context.Weapons.Add(weapon);
            await _context.SaveChangesAsync();
            return new Result<object> { Value = null };
        }
        catch (Exception)
        {
            return new Result<object> { IsError = true, Error = "A server error occurred" };
        }
    }

    public async Task<Result<object>> UpdateWeaponAsync(WeaponDto weaponDto)
    {
        try
        {
            var existingWeapon = await _context.Weapons
                .Include(w => w.WeaponType)
                .Include(w => w.Attribute)
                .Include(w => w.Skill)
                .Include(e => e.Cults)
                .FirstOrDefaultAsync(w => w.Id == weaponDto.Id);
            if (existingWeapon is null)
                return new Result<object> { IsError = true, Error = "Weapon not found" };

            _mapper.Map(weaponDto, existingWeapon);

            var weaponType = await _context.WeaponTypes
                .FirstOrDefaultAsync(wt => wt.Id == weaponDto.WeaponTypeId);
            if (weaponType is null)
                return new Result<object> { IsError = true, Error = "Weapon Type not found" };
            existingWeapon.WeaponType = weaponType;

            if (weaponDto.AttributeId is not null)
            {
                var attribute = await _context.Attributes.FindAsync(weaponDto.AttributeId.Value);
                if (attribute is null)
                    return new Result<object> { IsError = true, Error = "Attribute not found" };
                existingWeapon.Attribute = attribute;
            }
            else
                existingWeapon.Attribute = null;

            if (weaponDto.SkillId is not null)
            {
                var skill = await _context.Skills.FindAsync(weaponDto.SkillId.Value);
                if (skill is null)
                    return new Result<object> { IsError = true, Error = "Attribute not found" };
                existingWeapon.Skill = skill;
            }
            else
                existingWeapon.Skill = null;


            existingWeapon.Cults.Clear();
            foreach (var cultDto in weaponDto.Cults)
            {
                var cult = await _context.Cults
                  .FirstOrDefaultAsync(c => c.Id == cultDto.Id);
                if (cult is null)
                    return new Result<object> { IsError = true, Error = "Cult not found" };
                existingWeapon.Cults.Add(cult);
            }

            await _context.SaveChangesAsync();
            return new Result<object> { Value = null };
        }
        catch (Exception)
        {
            return new Result<object> { IsError = true, Error = "A server error occurred" };
        }
    }

    public async Task<Result<object>> DeleteWeaponAsync(Guid id)
    {
        try
        {
            var weapon = await _context.Weapons
                .FirstOrDefaultAsync(w => w.Id == id);
            if (weapon is null)
                return new Result<object> { IsError = true, Error = "Weapon not found" };

            _context.Weapons.Remove(weapon);
            await _context.SaveChangesAsync();
            return new Result<object> { Value = null };
        }
        catch (Exception)
        {
            return new Result<object> { IsError = true, Error = "A server error occurred" };
        }
    }
}