using DataAccessLayer;
using Domain.Protections;
using Microsoft.EntityFrameworkCore;

namespace Business.Protections;
public interface IProtectionQualityService
{
    Task<List<ProtectionQuality>> GetAllProtectionQualitiesAsync();
    Task<ProtectionQuality?> GetProtectionQualityByIdAsync(Guid id);
    Task<ProtectionQuality> CreateProtectionQualityAsync(ProtectionQuality protectionQuality);
    Task<ProtectionQuality?> UpdateProtectionQualityAsync(Guid id, ProtectionQuality protectionQuality);
    Task<bool> DeleteProtectionQualityAsync(Guid id);
}
public class ProtectionQualityService : IProtectionQualityService
{
    private readonly ApplicationDbContext _context;

    public ProtectionQualityService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<ProtectionQuality>> GetAllProtectionQualitiesAsync()
    {
        return await _context.ProtectionQualities.ToListAsync();
    }

    public async Task<ProtectionQuality?> GetProtectionQualityByIdAsync(Guid id)
    {
        return await _context.ProtectionQualities.FindAsync(id);
    }

    public async Task<ProtectionQuality> CreateProtectionQualityAsync(ProtectionQuality protectionQuality)
    {
        _context.ProtectionQualities.Add(protectionQuality);
        await _context.SaveChangesAsync();
        return protectionQuality;
    }

    public async Task<ProtectionQuality?> UpdateProtectionQualityAsync(Guid id, ProtectionQuality protectionQuality)
    {
        var existingProtectionQuality = await _context.ProtectionQualities.FindAsync(id);

        if (existingProtectionQuality == null)
        {
            return null;
        }

        existingProtectionQuality = protectionQuality;

        await _context.SaveChangesAsync();
        return existingProtectionQuality;
    }

    public async Task<bool> DeleteProtectionQualityAsync(Guid id)
    {
        var existingProtectionQuality = await _context.ProtectionQualities.FindAsync(id);

        if (existingProtectionQuality == null)
        {
            return false;
        }

        _context.ProtectionQualities.Remove(existingProtectionQuality);
        await _context.SaveChangesAsync();
        return true;
    }
}
