using DataAccessLayer;
using Domain.Weapons;
using Microsoft.EntityFrameworkCore;

namespace Business.Weapons;
public interface IWeaponTypeService
{
    Task<WeaponType?> GetWeaponTypeByIdAsync(Guid id);
    Task<IEnumerable<WeaponType>> GetAllWeaponTypesAsync();
    Task CreateWeaponTypeAsync(WeaponType weaponType);
    Task UpdateWeaponTypeAsync(Guid id, WeaponType weaponType);
    Task DeleteWeaponTypeAsync(Guid id);
}

public class WeaponTypeService : IWeaponTypeService
{
    private readonly ApplicationDbContext _context;

    public WeaponTypeService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<WeaponType?> GetWeaponTypeByIdAsync(Guid id)
    {
        return await _context.WeaponTypes
            .FirstOrDefaultAsync(wt => wt.Id == id);
    }

    public async Task<IEnumerable<WeaponType>> GetAllWeaponTypesAsync()
    {
        return await _context.WeaponTypes.ToListAsync();
    }

    public async Task CreateWeaponTypeAsync(WeaponType weaponType)
    {
        _context.WeaponTypes.Add(weaponType);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateWeaponTypeAsync(Guid id, WeaponType weaponType)
    {
        var existingWeaponType = await _context.WeaponTypes.FirstOrDefaultAsync(wt => wt.Id == id);
        if (existingWeaponType is not null)
        {
            existingWeaponType = weaponType; // Direct assignment as per your instructions
            await _context.SaveChangesAsync();
        }
    }

    public async Task DeleteWeaponTypeAsync(Guid id)
    {
        var weaponType = await _context.WeaponTypes.FindAsync(id);
        if (weaponType is not null)
        {
            _context.WeaponTypes.Remove(weaponType);
            await _context.SaveChangesAsync();
        }
    }
}