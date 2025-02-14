using DataAccessLayer;
using Domain.Characters;
using Microsoft.EntityFrameworkCore;

namespace Business.Characters;
public interface IAttributeService
{
    Task<CAttribute?> GetAttributeByIdAsync(Guid id);
    Task<IEnumerable<CAttribute>> GetAllAttributesAsync();
    Task<CAttribute> CreateAttributeAsync(CAttribute attribute);
    Task<bool> UpdateAttributeAsync(Guid id, CAttribute attribute);
    Task<bool> DeleteAttributeAsync(Guid id);
}

public class AttributeService : IAttributeService
{
    private readonly ApplicationDbContext _context;

    public AttributeService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<CAttribute?> GetAttributeByIdAsync(Guid id)
    {
        return await _context.Attributes
            .Include(a => a.Skills) // Inclure les compétences associées
            .FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task<IEnumerable<CAttribute>> GetAllAttributesAsync()
    {
        return await _context.Attributes.ToListAsync();
    }

    public async Task<CAttribute> CreateAttributeAsync(CAttribute attribute)
    {
        _context.Attributes.Add(attribute);
        await _context.SaveChangesAsync();
        return attribute;
    }

    public async Task<bool> UpdateAttributeAsync(Guid id, CAttribute attribute)
    {
        var existingAttribute = await _context.Attributes.FindAsync(id);
        if (existingAttribute == null)
            return false;

        _context.Entry(existingAttribute).CurrentValues.SetValues(attribute);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAttributeAsync(Guid id)
    {
        var attribute = await _context.Attributes.FindAsync(id);
        if (attribute == null)
            return false;

        _context.Attributes.Remove(attribute);
        await _context.SaveChangesAsync();
        return true;
    }
}