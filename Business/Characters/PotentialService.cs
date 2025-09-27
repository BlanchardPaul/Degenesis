using AutoMapper;
using DataAccessLayer;
using Degenesis.Shared.DTOs.Characters;
using Domain.Characters;
using Microsoft.EntityFrameworkCore;

namespace Business.Characters;

public interface IPotentialService
{
    Task<List<PotentialDto>> GetAllPotentialsAsync();
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

    public async Task<List<PotentialDto>> GetAllPotentialsAsync()
    {
        var potentials = await _context.Potentials
            .OrderBy(p => p.Name)
            .Include(p => p.Prerequisites)
                .ThenInclude(pr => pr.AttributeRequired)
            .Include(p => p.Prerequisites)
                .ThenInclude(pr => pr.SkillRequired)
            .Include(p => p.Prerequisites)
                .ThenInclude(pr => pr.BackgroundRequired)
            .Include(p => p.Prerequisites)
                .ThenInclude(pr => pr.RankRequired)
            .Include(p => p.Prerequisites)
            .Include(p => p.Cult)
            .ToListAsync();

        return potentials.Select(p => _mapper.Map<PotentialDto>(p)).ToList();
    }

    public async Task<PotentialDto?> GetPotentialByIdAsync(Guid id)
    {
        var potential = await _context.Potentials
            .Include(p => p.Prerequisites)
                .ThenInclude(pr => pr.AttributeRequired)
            .Include(p => p.Prerequisites)
                .ThenInclude(pr => pr.SkillRequired)
            .Include(p => p.Prerequisites)
                .ThenInclude(pr => pr.BackgroundRequired)
            .Include(p => p.Prerequisites)
                .ThenInclude(pr => pr.RankRequired)
            .Include(p => p.Prerequisites)
            .Include(p => p.Cult)
            .FirstOrDefaultAsync(p => p.Id == id);

        return potential is null ? null : _mapper.Map<PotentialDto>(potential);
    }

    public async Task<PotentialDto?> CreatePotentialAsync(PotentialCreateDto potentialCreate)
    {
        try
        {
            var potential = _mapper.Map<Potential>(potentialCreate);

            potential.Prerequisites = await _context.PotentialPrerequisites
                .Where(pp => potentialCreate.Prerequisites.Select(p => p.Id).Contains(pp.Id))
                .ToListAsync();

            if (potentialCreate.CultId is not null)
            {
                potential.Cult = await _context.Cults.FirstAsync(c => c.Id == potentialCreate.CultId);
            }
            else
            {
                potential.Cult = null;
            }

                _context.Potentials.Add(potential);
            await _context.SaveChangesAsync();
            return _mapper.Map<PotentialDto>(potential);
        }
        catch (Exception)
        {
            return null;
        }
    }

    public async Task<bool> UpdatePotentialAsync(PotentialDto potentialDto)
    {
        try
        {
            var existingPotential = await _context.Potentials
                .Include(p => p.Prerequisites)
                .Include(p => p.Cult)
                .FirstOrDefaultAsync(p => p.Id == potentialDto.Id);

            if (existingPotential is null)
                return false;

            _mapper.Map(potentialDto, existingPotential);

            existingPotential.Prerequisites = await _context.PotentialPrerequisites
                .Where(pp => potentialDto.Prerequisites.Select(p => p.Id).Contains(pp.Id))
                .ToListAsync();

            if(potentialDto.CultId is not null)
            {
                existingPotential.Cult = await _context.Cults.FirstAsync(c => c.Id == potentialDto.CultId);
            }
            else
            {
                existingPotential.Cult = null;
                existingPotential.CultId = null;
            }
                

            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public async Task<bool> DeletePotentialAsync(Guid id)
    {
        try
        {
            var potential = await _context.Potentials.FindAsync(id);
            if (potential is null)
                return false;

            _context.Potentials.Remove(potential);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
}
