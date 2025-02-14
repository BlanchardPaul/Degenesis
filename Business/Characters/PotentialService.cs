using DataAccessLayer;
using Domain.Characters;
using Microsoft.EntityFrameworkCore;

namespace Business.Characters;
public interface IPotentialService
{
    Task<IEnumerable<Potential>> GetAllPotentialsAsync();
    Task<Potential?> GetPotentialByIdAsync(Guid id);
    Task<Potential?> CreatePotentialAsync(Potential potential);
    Task<bool> UpdatePotentialAsync(Guid id, Potential potential);
    Task<bool> DeletePotentialAsync(Guid id);
}
public class PotentialService : IPotentialService
{
    private readonly ApplicationDbContext _context;

    public PotentialService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Potential>> GetAllPotentialsAsync()
    {
        return await _context.Potentials.ToListAsync();
    }

    public async Task<Potential?> GetPotentialByIdAsync(Guid id)
    {
        return await _context.Potentials.FindAsync(id);
    }

    public async Task<Potential?> CreatePotentialAsync(Potential potential)
    {
        _context.Potentials.Add(potential);
        await _context.SaveChangesAsync();
        return potential;
    }

    public async Task<bool> UpdatePotentialAsync(Guid id, Potential potential)
    {
        var existingPotential = await _context.Potentials.FindAsync(id);
        if (existingPotential == null)
        {
            return false;
        }

        existingPotential = potential;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeletePotentialAsync(Guid id)
    {
        var potential = await _context.Potentials.FindAsync(id);
        if (potential == null)
        {
            return false;
        }

        _context.Potentials.Remove(potential);
        await _context.SaveChangesAsync();
        return true;
    }
}
