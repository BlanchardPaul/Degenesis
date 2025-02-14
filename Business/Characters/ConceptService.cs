using DataAccessLayer;
using Domain.Characters;
using Microsoft.EntityFrameworkCore;

namespace Business.Characters;
public interface IConceptService
{
    Task<Concept?> GetConceptByIdAsync(Guid id);
    Task<IEnumerable<Concept>> GetAllConceptsAsync();
    Task<Concept> CreateConceptAsync(Concept concept);
    Task<bool> UpdateConceptAsync(Guid id, Concept concept);
    Task<bool> DeleteConceptAsync(Guid id);
}
public class ConceptService : IConceptService
{
    private readonly ApplicationDbContext _context;

    public ConceptService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Concept?> GetConceptByIdAsync(Guid id)
    {
        return await _context.Concepts
            .Include(c => c.BonusAttribute)
            .Include(c => c.BonusSkills)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<IEnumerable<Concept>> GetAllConceptsAsync()
    {
        return await _context.Concepts
            .Include(c => c.BonusAttribute)
            .Include(c => c.BonusSkills)
            .ToListAsync();
    }

    public async Task<Concept> CreateConceptAsync(Concept concept)
    {
        _context.Concepts.Add(concept);
        await _context.SaveChangesAsync();
        return concept;
    }

    public async Task<bool> UpdateConceptAsync(Guid id, Concept concept)
    {
        var existingConcept = await _context.Concepts
            .Include (c => c.BonusAttribute)
            .Include (c => c.BonusSkills)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (existingConcept == null)
            return false;

        _context.Entry(existingConcept).CurrentValues.SetValues(concept);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteConceptAsync(Guid id)
    {
        var concept = await _context.Concepts
            .FirstOrDefaultAsync(c => c.Id == id);

        if (concept == null)
            return false;

        _context.Concepts.Remove(concept);
        await _context.SaveChangesAsync();
        return true;
    }
}
