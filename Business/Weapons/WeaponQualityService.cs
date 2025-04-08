using AutoMapper;
using DataAccessLayer;
using Degenesis.Shared.DTOs.Weapons;
using Domain.Weapons;
using Microsoft.EntityFrameworkCore;

namespace Business.Weapons;
public interface IWeaponQualityService
{
    Task<IEnumerable<WeaponQualityDto>> GetAllWeaponQualitiesAsync();
    Task<WeaponQualityDto?> GetWeaponQualityByIdAsync(Guid id);
    Task<WeaponQualityDto?> CreateWeaponQualityAsync(WeaponQualityCreateDto weaponQualityCreate);
    Task<bool> UpdateWeaponQualityAsync(WeaponQualityDto weaponQuality);
    Task<bool> DeleteWeaponQualityAsync(Guid id);
}

public class WeaponQualityService : IWeaponQualityService
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public WeaponQualityService(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<WeaponQualityDto>> GetAllWeaponQualitiesAsync()
    {
        var weaponQualities = await _context.WeaponQualities.ToListAsync();
        return _mapper.Map<IEnumerable<WeaponQualityDto>>(weaponQualities);
    }

    public async Task<WeaponQualityDto?> GetWeaponQualityByIdAsync(Guid id)
    {
        var weaponQuality = await _context.WeaponQualities.FindAsync(id);
        return weaponQuality is null ? null : _mapper.Map<WeaponQualityDto>(weaponQuality);
    }

    public async Task<WeaponQualityDto?> CreateWeaponQualityAsync(WeaponQualityCreateDto weaponQualityCreate)
    {
        try
        {
            var weaponQuality = _mapper.Map<WeaponQuality>(weaponQualityCreate);
            _context.WeaponQualities.Add(weaponQuality);
            await _context.SaveChangesAsync();
            return _mapper.Map<WeaponQualityDto>(weaponQuality);
        }
        catch (Exception)
        {
            return null;
        }
    }

    public async Task<bool> UpdateWeaponQualityAsync(WeaponQualityDto weaponQualityDto)
    {
        try
        {
            var existingWeaponQuality = await _context.WeaponQualities.FindAsync(weaponQualityDto.Id);
            if (existingWeaponQuality is null)
                return false;

            _mapper.Map(weaponQualityDto, existingWeaponQuality);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public async Task<bool> DeleteWeaponQualityAsync(Guid id)
    {
        try
        {
            var weaponQuality = await _context.WeaponQualities.FindAsync(id);
            if (weaponQuality is null)
                return false;

            _context.WeaponQualities.Remove(weaponQuality);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
}