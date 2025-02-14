using DataAccessLayer;
using Domain.Protections;
using Microsoft.EntityFrameworkCore;

namespace Business.Protections;
public interface IProtectionService
{
    Task<List<Protection>> GetAllProtectionsAsync();
    Task<Protection?> GetProtectionByIdAsync(Guid id);
    Task<Protection> CreateProtectionAsync(Protection protection);
    Task<Protection?> UpdateProtectionAsync(Guid id, Protection protection);
    Task<bool> DeleteProtectionAsync(Guid id);
}
public class ProtectionService : IProtectionService
{
    private readonly ApplicationDbContext _context;

    public ProtectionService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Protection>> GetAllProtectionsAsync()
    {
        return await _context.Protections.ToListAsync();
    }

    public async Task<Protection?> GetProtectionByIdAsync(Guid id)
    {
        return await _context.Protections.FindAsync(id);
    }

    public async Task<Protection> CreateProtectionAsync(Protection protection)
    {
        _context.Protections.Add(protection);
        await _context.SaveChangesAsync();
        return protection;
    }

    public async Task<Protection?> UpdateProtectionAsync(Guid id, Protection protection)
    {
        var existingProtection = await _context.Protections.FindAsync(id);

        if (existingProtection == null)
        {
            return null;
        }

        existingProtection = protection;

        await _context.SaveChangesAsync();
        return existingProtection;
    }

    public async Task<bool> DeleteProtectionAsync(Guid id)
    {
        var existingProtection = await _context.Protections.FindAsync(id);

        if (existingProtection == null)
        {
            return false;
        }

        _context.Protections.Remove(existingProtection);
        await _context.SaveChangesAsync();
        return true;
    }
}
