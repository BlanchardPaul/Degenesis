using AutoMapper;
using DataAccessLayer;
using Degenesis.Shared.DTOs.Characters;
using Domain.Characters;
using Microsoft.EntityFrameworkCore;

namespace Business.Characters;
public interface IPotentialService
{
    Task<IEnumerable<PotentialDto>> GetAllPotentialsAsync();
    Task<PotentialDto?> GetPotentialByIdAsync(Guid id);
    Task<PotentialDto?> CreatePotentialAsync(PotentialCreateDto potentialCreate);
    Task<bool> UpdatePotentialAsync(PotentialDto potential);
    Task<bool> DeletePotentialAsync(Guid id);
}

public class PotentialService : IPotentialService
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public PotentialService(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<PotentialDto>> GetAllPotentialsAsync()
    {
        var potentials = await _context.Potentials
            .Include(p => p.Cult)
            .ToListAsync();
        return _mapper.Map<IEnumerable<PotentialDto>>(potentials);
    }

    public async Task<PotentialDto?> GetPotentialByIdAsync(Guid id)
    {
        var potential = await _context.Potentials
            .Include(p => p.Cult)
            .FirstOrDefaultAsync(p => p.Id == id);

        return potential is null ? null : _mapper.Map<PotentialDto>(potential);
    }

    public async Task<PotentialDto?> CreatePotentialAsync(PotentialCreateDto potentialCreate)
    {
        var potential = _mapper.Map<Potential>(potentialCreate);

        if (potentialCreate.CultId.HasValue)
        {
            potential.Cult = await _context.Cults.FindAsync(potentialCreate.CultId);
        }

        _context.Potentials.Add(potential);
        await _context.SaveChangesAsync();
        return _mapper.Map<PotentialDto>(potential);
    }

    public async Task<bool> UpdatePotentialAsync(PotentialDto potentialDto)
    {
        var existingPotential = await _context.Potentials
            .Include(p => p.Cult)
            .FirstOrDefaultAsync(p => p.Id == potentialDto.Id);

        if (existingPotential == null)
            return false;

        _mapper.Map(potentialDto, existingPotential);

        if (potentialDto.Cult?.Id != null)
        {
            existingPotential.Cult = await _context.Cults.FindAsync(potentialDto.Cult.Id);
        }

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeletePotentialAsync(Guid id)
    {
        var potential = await _context.Potentials.FindAsync(id);
        if (potential == null)
            return false;

        _context.Potentials.Remove(potential);
        await _context.SaveChangesAsync();
        return true;
    }
}