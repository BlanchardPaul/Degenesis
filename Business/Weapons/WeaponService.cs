namespace Business.Weapons;

using DataAccessLayer;
using Domain.Weapons;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface IWeaponService
{
    Task<Weapon?> GetWeaponByIdAsync(Guid id);
    Task<IEnumerable<Weapon>> GetAllWeaponsAsync();
    Task CreateWeaponAsync(Weapon weapon);
    Task UpdateWeaponAsync(Guid id, Weapon weapon);
    Task DeleteWeaponAsync(Guid id);
}
public class WeaponService : IWeaponService
{
    private readonly ApplicationDbContext _context;

    public WeaponService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Weapon?> GetWeaponByIdAsync(Guid id)
    {
        return await _context.Weapons
            .Include(w => w.Attribute)
            .Include(w => w.WeaponType)
            .Include(w => w.Qualities)
            .FirstOrDefaultAsync(w => w.Id == id);
    }

    public async Task<IEnumerable<Weapon>> GetAllWeaponsAsync()
    {
        return await _context.Weapons
            .Include(w => w.Attribute)
            .Include(w => w.WeaponType)
            .Include(w => w.Qualities)
            .ToListAsync();
    }

    public async Task CreateWeaponAsync(Weapon weapon)
    {
        _context.Weapons.Add(weapon);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateWeaponAsync(Guid id, Weapon weapon)
    {
        var existingWeapon = await _context.Weapons.FirstOrDefaultAsync(w => w.Id == id);
        if (existingWeapon is not null)
        {
            existingWeapon = weapon;
            await _context.SaveChangesAsync();
        }
    }

    public async Task DeleteWeaponAsync(Guid id)
    {
        var weapon = await _context.Weapons.FindAsync(id);
        if (weapon is not null)
        {
            _context.Weapons.Remove(weapon);
            await _context.SaveChangesAsync();
        }
    }
}