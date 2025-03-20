using AutoMapper;
using DataAccessLayer;
using Degenesis.Shared.DTOs.Weapons;
using Domain.Weapons;
using Microsoft.EntityFrameworkCore;

namespace Business.Weapons;

public interface IWeaponService
{
    Task<IEnumerable<WeaponDto>> GetAllWeaponsAsync();
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

    public async Task<IEnumerable<WeaponDto>> GetAllWeaponsAsync()
    {
        var weapons = await _context.Weapons
            .Include(w => w.WeaponType)
            .Include(w => w.Attribute)
            .Include(w => w.Qualities)
            .ToListAsync();
        return _mapper.Map<IEnumerable<WeaponDto>>(weapons);
    }

    public async Task<WeaponDto?> GetWeaponByIdAsync(Guid id)
    {
        var weapon = await _context.Weapons
            .Include(w => w.WeaponType)
            .Include(w => w.Attribute)
            .Include(w => w.Qualities)
            .FirstOrDefaultAsync(w => w.Id == id);

        return weapon is null ? null : _mapper.Map<WeaponDto>(weapon);
    }

    public async Task<WeaponDto?> CreateWeaponAsync(WeaponCreateDto weaponCreate)
    {
        var weapon = _mapper.Map<Weapon>(weaponCreate);
        weapon.WeaponType = await _context.WeaponTypes.FirstAsync(wt => wt.Id == weaponCreate.WeaponTypeId);
        weapon.Attribute = weaponCreate.AttributeId.HasValue
            ? await _context.Attributes.FindAsync(weaponCreate.AttributeId.Value)
            : null;

        weapon.Qualities = _context.WeaponQualities
            .Where(q => weaponCreate.Qualities.Select(qd => qd.Id).Contains(q.Id))
            .ToList();

        _context.Weapons.Add(weapon);
        await _context.SaveChangesAsync();
        return _mapper.Map<WeaponDto>(weapon);
    }

    public async Task<bool> UpdateWeaponAsync(WeaponDto weaponDto)
    {
        var existingWeapon = await _context.Weapons
            .Include(w => w.WeaponType)
            .Include(w => w.Attribute)
            .Include(w => w.Qualities)
            .FirstOrDefaultAsync(w => w.Id == weaponDto.Id);

        if (existingWeapon is null || weaponDto.WeaponType is null)
            return false;

        _mapper.Map(weaponDto, existingWeapon);
        existingWeapon.WeaponType = await _context.WeaponTypes.FirstAsync(wt => wt.Id == weaponDto.WeaponType.Id);
        existingWeapon.Attribute = weaponDto.Attribute?.Id != null
            ? await _context.Attributes.FindAsync(weaponDto.Attribute.Id)
            : null;

        existingWeapon.Qualities.Clear();
        existingWeapon.Qualities.AddRange(_context.WeaponQualities
            .Where(q => weaponDto.Qualities.Select(qd => qd.Id).Contains(q.Id)));

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteWeaponAsync(Guid id)
    {
        var weapon = await _context.Weapons.FindAsync(id);
        if (weapon == null)
            return false;

        _context.Weapons.Remove(weapon);
        await _context.SaveChangesAsync();
        return true;
    }
}