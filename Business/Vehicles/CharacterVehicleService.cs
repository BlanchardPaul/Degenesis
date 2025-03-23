using DataAccessLayer;
using Domain.Vehicles;
using Microsoft.EntityFrameworkCore;

namespace Business.Vehicles;
public interface ICharacterVehicleService
{
    Task<List<CharacterVehicle>> GetAllCharacterVehiclesAsync();
    Task<CharacterVehicle?> GetCharacterVehicleByIdAsync(Guid id);
    Task<CharacterVehicle> CreateCharacterVehicleAsync(CharacterVehicle characterVehicle);
    Task<CharacterVehicle?> UpdateCharacterVehicleAsync(Guid id, CharacterVehicle characterVehicle);
    Task<bool> DeleteCharacterVehicleAsync(Guid id);
}
public class CharacterVehicleService : ICharacterVehicleService
{
    private readonly ApplicationDbContext _context;

    public CharacterVehicleService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<CharacterVehicle>> GetAllCharacterVehiclesAsync()
    {
        return await _context.CharacterVehicles.ToListAsync();
    }

    public async Task<CharacterVehicle?> GetCharacterVehicleByIdAsync(Guid id)
    {
        return await _context.CharacterVehicles.FindAsync(id);
    }

    public async Task<CharacterVehicle> CreateCharacterVehicleAsync(CharacterVehicle characterVehicle)
    {
        _context.CharacterVehicles.Add(characterVehicle);
        await _context.SaveChangesAsync();
        return characterVehicle;
    }

    public async Task<CharacterVehicle?> UpdateCharacterVehicleAsync(Guid id, CharacterVehicle characterVehicle)
    {
        var existingCharacterVehicle = await _context.CharacterVehicles.FindAsync(id);

        if (existingCharacterVehicle is null)
        {
            return null;
        }

        existingCharacterVehicle = characterVehicle;

        await _context.SaveChangesAsync();
        return existingCharacterVehicle;
    }

    public async Task<bool> DeleteCharacterVehicleAsync(Guid id)
    {
        var existingCharacterVehicle = await _context.CharacterVehicles.FindAsync(id);

        if (existingCharacterVehicle is null)
        {
            return false;
        }

        _context.CharacterVehicles.Remove(existingCharacterVehicle);
        await _context.SaveChangesAsync();
        return true;
    }
}
