using DataAccessLayer;
using Domain.Weapons;
using Microsoft.EntityFrameworkCore;

namespace Business.Weapons;
public interface IWeaponQualityService
{
    Task<WeaponQuality?> GetWeaponQualityByIdAsync(Guid id);
    Task<IEnumerable<WeaponQuality>> GetAllWeaponQualitiesAsync();
    Task CreateWeaponQualityAsync(WeaponQuality weaponQuality);
    Task UpdateWeaponQualityAsync(Guid id, WeaponQuality weaponQuality);
    Task DeleteWeaponQualityAsync(Guid id);
}

public class WeaponQualityService : IWeaponQualityService
{
    private readonly ApplicationDbContext _context;

    public WeaponQualityService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<WeaponQuality?> GetWeaponQualityByIdAsync(Guid id)
    {
        return await _context.WeaponQualities
            .FirstOrDefaultAsync(wq => wq.Id == id);
    }

    public async Task<IEnumerable<WeaponQuality>> GetAllWeaponQualitiesAsync()
    {
        return await _context.WeaponQualities.ToListAsync();
    }

    public async Task CreateWeaponQualityAsync(WeaponQuality weaponQuality)
    {
        _context.WeaponQualities.Add(weaponQuality);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateWeaponQualityAsync(Guid id, WeaponQuality weaponQuality)
    {
        var existingWeaponQuality = await _context.WeaponQualities.FirstOrDefaultAsync(wq => wq.Id == id);
        if (existingWeaponQuality is not null)
        {
            existingWeaponQuality = weaponQuality; // Direct assignment as per your instructions
            await _context.SaveChangesAsync();
        }
    }

    public async Task DeleteWeaponQualityAsync(Guid id)
    {
        var weaponQuality = await _context.WeaponQualities.FindAsync(id);
        if (weaponQuality is not null)
        {
            _context.WeaponQualities.Remove(weaponQuality);
            await _context.SaveChangesAsync();
        }
    }
}