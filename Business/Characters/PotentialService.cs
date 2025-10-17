using AutoMapper;
using DataAccessLayer;
using Degenesis.Shared.DTOs.Characters.CRUD;
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

        return _mapper.Map<List<PotentialDto>>(potentials);
    }

    public async Task<PotentialDto?> GetPotentialByIdAsync(Guid id)
    {
        try
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
                .FirstOrDefaultAsync(p => p.Id == id) ?? throw new Exception("Potential not found");
            return _mapper.Map<PotentialDto>(potential);
        }
        catch (Exception)
        {
            return null;
        }
    }

    public async Task<PotentialDto?> CreatePotentialAsync(PotentialCreateDto potentialCreate)
    {
        try
        {
            var potential = _mapper.Map<Potential>(potentialCreate);

            foreach (var prerequisiteDto in potentialCreate.Prerequisites)
            {
                var existingPrerequisite = await _context.PotentialPrerequisites
                    .FirstOrDefaultAsync(s => s.Id == prerequisiteDto.Id)
                    ?? throw new Exception("PotentialPrerequisite not found");

                potential.Prerequisites.Add(existingPrerequisite);
            }

            if (potentialCreate.CultId is not null && potentialCreate.CultId != Guid.Empty)
            {
                potential.Cult = await _context.Cults
                    .FirstOrDefaultAsync(c => c.Id == potentialCreate.CultId)
                    ?? throw new Exception("Cult not found");
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
                .FirstOrDefaultAsync(p => p.Id == potentialDto.Id)
                ?? throw new Exception("Potential not found");

            if (existingPotential is null)
                return false;

            _mapper.Map(potentialDto, existingPotential);

            existingPotential.Prerequisites.Clear();
            foreach (var prerequisiteDto in potentialDto.Prerequisites)
            {
                var prerequisite = await _context.PotentialPrerequisites
                    .FirstOrDefaultAsync(s => s.Id == prerequisiteDto.Id)
                    ?? throw new Exception("PotentialPrerequisite not found");
                existingPotential.Prerequisites.Add(prerequisite);
            }

            if(potentialDto.CultId is not null)
            {
                existingPotential.Cult = await _context.Cults
                    .FirstOrDefaultAsync(c => c.Id == potentialDto.CultId)
                    ?? throw new Exception("Cult not found");
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
            var potential = await _context.Potentials
                .Include(p => p.Prerequisites)
                .Include(p => p.Cult)
                .FirstOrDefaultAsync(p => p.Id == id)
                ?? throw new Exception("Potential not found");

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
