using DataAccessLayer;
using Domain.Characters;
using Microsoft.EntityFrameworkCore;

namespace Business.Characters;
public interface ICultureService
{
    Task<IEnumerable<Culture>> GetAllCulturesAsync();
    Task<Culture?> GetCultureByIdAsync(Guid id);  // Retour nullable
    Task<Culture?> CreateCultureAsync(Culture culture); // Retour nullable
    Task<bool> UpdateCultureAsync(Guid id, Culture culture);
    Task<bool> DeleteCultureAsync(Guid id);
}

public class CultureService : ICultureService
{
    private readonly ApplicationDbContext _context;

    public CultureService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Culture>> GetAllCulturesAsync()
    {
        return await _context.Cultures.ToListAsync();
    }

    public async Task<Culture?> GetCultureByIdAsync(Guid id)
    {
        return await _context.Cultures.FindAsync(id);
    }

    public async Task<Culture?> CreateCultureAsync(Culture culture)
    {
        _context.Cultures.Add(culture);
        await _context.SaveChangesAsync();
        return culture;
    }

    public async Task<bool> UpdateCultureAsync(Guid id, Culture culture)
    {
        var existingCulture = await _context.Cultures.FindAsync(id);
        if (existingCulture == null)
        {
            return false;
        }

        existingCulture.Name = culture.Name;
        existingCulture.Description = culture.Description;

        _context.Cultures.Update(existingCulture);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteCultureAsync(Guid id)
    {
        var culture = await _context.Cultures.FindAsync(id);
        if (culture == null)
        {
            return false; 
        }

        _context.Cultures.Remove(culture);
        await _context.SaveChangesAsync();
        return true;
    }
}
