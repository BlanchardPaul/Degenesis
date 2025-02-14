using DataAccessLayer;
using Domain.Characters;
using Microsoft.EntityFrameworkCore;

namespace Business.Characters;
public interface ICultService
{
    Task<Cult?> GetCultByIdAsync(Guid id);
    Task<IEnumerable<Cult>> GetAllCultsAsync();
    Task<Cult> CreateCultAsync(Cult cult);
    Task<bool> UpdateCultAsync(Guid id, Cult cult);
    Task<bool> DeleteCultAsync(Guid id);
}

public class CultService : ICultService
{
    private readonly ApplicationDbContext _context;

    public CultService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Cult?> GetCultByIdAsync(Guid id)
    {
        return await _context.Cults
            .Include(c => c.BonusSkills)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<IEnumerable<Cult>> GetAllCultsAsync()
    {
        return await _context.Cults
            .Include(c => c.BonusSkills)
            .ToListAsync();
    }

    public async Task<Cult> CreateCultAsync(Cult cult)
    {
        _context.Cults.Add(cult);
        await _context.SaveChangesAsync();
        return cult;
    }

    public async Task<bool> UpdateCultAsync(Guid id, Cult cult)
    {
        var existingCult = await _context.Cults.FindAsync(id);
        if (existingCult == null)
            return false;

        _context.Entry(existingCult).CurrentValues.SetValues(cult);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteCultAsync(Guid id)
    {
        var cult = await _context.Cults.FindAsync(id);
        if (cult == null)
            return false;

        _context.Cults.Remove(cult);
        await _context.SaveChangesAsync();
        return true;
    }
}
