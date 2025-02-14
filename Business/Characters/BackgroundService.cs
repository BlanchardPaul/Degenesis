using DataAccessLayer;
using Domain.Characters;
using Microsoft.EntityFrameworkCore;

namespace Business.Characters;

public interface IBackgroundService
{
    Task<Background?> GetBackgroundByIdAsync(Guid id);
    Task<IEnumerable<Background>> GetAllBackgroundsAsync();
    Task<Background> CreateBackgroundAsync(Background background);
    Task<bool> UpdateBackgroundAsync(Guid id, Background background);
    Task<bool> DeleteBackgroundAsync(Guid id);
}
public class BackgroundService : IBackgroundService
{
    private readonly ApplicationDbContext _context;

    public BackgroundService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Background?> GetBackgroundByIdAsync(Guid id)
    {
        return await _context.Backgrounds
            .FirstOrDefaultAsync(b => b.Id == id);
    }

    public async Task<IEnumerable<Background>> GetAllBackgroundsAsync()
    {
        return await _context.Backgrounds.ToListAsync();
    }

    public async Task<Background> CreateBackgroundAsync(Background background)
    {
        _context.Backgrounds.Add(background);
        await _context.SaveChangesAsync();
        return background;
    }

    public async Task<bool> UpdateBackgroundAsync(Guid id, Background background)
    {
        var existingBackground = await _context.Backgrounds.FindAsync(id);
        if (existingBackground == null)
            return false;

        _context.Entry(existingBackground).CurrentValues.SetValues(background);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteBackgroundAsync(Guid id)
    {
        var background = await _context.Backgrounds.FindAsync(id);
        if (background == null)
            return false;

        _context.Backgrounds.Remove(background);
        await _context.SaveChangesAsync();
        return true;
    }
}
