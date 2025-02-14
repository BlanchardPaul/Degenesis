using DataAccessLayer;
using Domain.Weapons;
using Microsoft.EntityFrameworkCore;

namespace Business.Weapons;
public interface ICharacterWeaponService
{
    Task<CharacterWeapon?> GetCharacterWeaponByIdAsync(Guid id);
    Task<CharacterWeapon?> GetCharacterWeaponByCharacterIdAsync(Guid characterId);
    Task CreateCharacterWeaponAsync(CharacterWeapon characterWeapon);
    Task UpdateCharacterWeaponAsync(Guid id, CharacterWeapon characterWeapon);
    Task DeleteCharacterWeaponAsync(Guid id);
}

public class CharacterWeaponService : ICharacterWeaponService
{
    private readonly ApplicationDbContext _context;

    public CharacterWeaponService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<CharacterWeapon?> GetCharacterWeaponByIdAsync(Guid id)
    {
        return await _context.CharacterWeapons
                             .Include(cw => cw.Weapon)
                             .FirstOrDefaultAsync(cw => cw.Id == id);
    }

    public async Task<CharacterWeapon?> GetCharacterWeaponByCharacterIdAsync(Guid characterId)
    {
        return await _context.CharacterWeapons
                             .Include(cw => cw.Weapon)
                             .FirstOrDefaultAsync(cw => cw.CharacterId == characterId);
    }

    public async Task CreateCharacterWeaponAsync(CharacterWeapon characterWeapon)
    {
        _context.CharacterWeapons.Add(characterWeapon);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateCharacterWeaponAsync(Guid id, CharacterWeapon characterWeapon)
    {
        var existingCharacterWeapon = await _context.CharacterWeapons.FindAsync(id);
        if (existingCharacterWeapon != null)
        {
            existingCharacterWeapon = characterWeapon;
            await _context.SaveChangesAsync();
        }
    }

    public async Task DeleteCharacterWeaponAsync(Guid id)
    {
        var characterWeapon = await _context.CharacterWeapons.FindAsync(id);
        if (characterWeapon != null)
        {
            _context.CharacterWeapons.Remove(characterWeapon);
            await _context.SaveChangesAsync();
        }
    }
}
