using AutoMapper;
using DataAccessLayer;
using Degenesis.Shared.DTOs.Weapons;
using Domain.Weapons;
using Microsoft.EntityFrameworkCore;

namespace Business.Weapons;

public interface IWeaponService
{
    Task<List<WeaponDto>> GetAllWeaponsAsync();
    Task<WeaponDto?> GetWeaponByIdAsync(Guid id);
    Task<WeaponDto?> CreateWeaponAsync(WeaponCreateDto weaponCreate);
    Task<bool> UpdateWeaponAsync(WeaponDto weapon);
    Task<bool> DeleteWeaponAsync(Guid id);
}

public class WeaponService : IWeaponService
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public WeaponService(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<WeaponDto>> GetAllWeaponsAsync()
    {
        var weapons = await _context.Weapons
            .Include(w => w.WeaponType)
            .Include(w => w.Attribute)
            .Include(w => w.Qualities)
            .ToListAsync();
        return _mapper.Map<List<WeaponDto>>(weapons);
    }

    public async Task<WeaponDto?> GetWeaponByIdAsync(Guid id)
    {
        try
        {
            var weapon = await _context.Weapons
                .Include(w => w.WeaponType)
                .Include(w => w.Attribute)
                .Include(w => w.Qualities)
                .FirstOrDefaultAsync(w => w.Id == id)
                ?? throw new Exception("Weapon not found");
            return _mapper.Map<WeaponDto>(weapon);
        }
        catch (Exception)
        {
            return null;
        }
    }

    public async Task<WeaponDto?> CreateWeaponAsync(WeaponCreateDto weaponCreate)
    {
        try
        {
            var weapon = _mapper.Map<Weapon>(weaponCreate);

            weapon.WeaponType = await _context.WeaponTypes
                .FirstOrDefaultAsync(wt => wt.Id == weaponCreate.WeaponTypeId)
                ?? throw new Exception("WeaponType not found");

            if(weaponCreate.AttributeId is not null)
                weapon.Attribute = await _context.Attributes.FindAsync(weaponCreate.AttributeId.Value)
                    ?? throw new Exception("WeaponAttribute not found");

            foreach(var quality in weaponCreate.Qualities)
            {
                var existingQuality = _context.WeaponQualities.Find(quality.Id)
                    ?? throw new Exception("WeaponQuality not found");
                weapon.Qualities.Add(existingQuality);
            }

            _context.Weapons.Add(weapon);
            await _context.SaveChangesAsync();
            return _mapper.Map<WeaponDto>(weapon);
        }
        catch (Exception)
        {
            return null;
        }
    }

    public async Task<bool> UpdateWeaponAsync(WeaponDto weaponDto)
    {
        try
        {
            var existingWeapon = await _context.Weapons
                .Include(w => w.WeaponType)
                .Include(w => w.Attribute)
                .Include(w => w.Qualities)
                .FirstOrDefaultAsync(w => w.Id == weaponDto.Id)
                ?? throw new Exception("Weapon not found");

            _mapper.Map(weaponDto, existingWeapon);

            existingWeapon.WeaponType = await _context.WeaponTypes
                .FirstOrDefaultAsync(wt => wt.Id == weaponDto.WeaponTypeId)
                ?? throw new Exception("WeaponType not found");


            if (weaponDto.AttributeId is not null)
            {
                existingWeapon.Attribute = await _context.Attributes.FindAsync(weaponDto.AttributeId)
                    ?? throw new Exception("WeaponAttribute not found");
            }
            else
            {
                existingWeapon.Attribute = null;
            }

            existingWeapon.Qualities.Clear();
            foreach (var quality in weaponDto.Qualities)
            {
                var existingQuality = _context.WeaponQualities.Find(quality.Id)
                    ?? throw new Exception("WeaponQuality not found");
                existingWeapon.Qualities.Add(existingQuality);
            }

            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public async Task<bool> DeleteWeaponAsync(Guid id)
    {
        try
        {
            var weapon = await _context.Weapons
                .Include(w => w.WeaponType)
                .Include(w => w.Attribute)
                .Include(w => w.Qualities)
                .FirstOrDefaultAsync(w => w.Id == id)
                ?? throw new Exception("Weapon not found");

            _context.Weapons.Remove(weapon);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
}