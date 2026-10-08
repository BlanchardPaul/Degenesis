using AutoMapper;
using DataAccessLayer;
using Degenesis.Shared.DTOs;
using Degenesis.Shared.DTOs.Characters.CRUD;
using Domain.Characters;
using Microsoft.EntityFrameworkCore;

namespace Business.Characters;

public interface IPotentialService
{
    Task<Result<List<PotentialDto>>> GetAllPotentialsAsync();
    Task<Result<PotentialDto>> GetPotentialByIdAsync(Guid id);
    Task<Result<object>> CreatePotentialAsync(PotentialCreateDto potentialCreate);
    Task<Result<object>> UpdatePotentialAsync(PotentialDto potential);
    Task<Result<object>> DeletePotentialAsync(Guid id);
}

public class PotentialService(ApplicationDbContext context, IMapper mapper) : IPotentialService
{
    private readonly ApplicationDbContext _context = context;
    private readonly IMapper _mapper = mapper;

    public async Task<Result<List<PotentialDto>>> GetAllPotentialsAsync()
    {
        try
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
                .OrderBy(p => p.Name)
                .ToListAsync();

            return new Result<List<PotentialDto>> { Value = _mapper.Map<List<PotentialDto>>(potentials) };
        }
        catch (Exception)
        {
            return new Result<List<PotentialDto>> { IsError = true, Error = "A server error occurred" };
        }
    }

    public async Task<Result<PotentialDto>> GetPotentialByIdAsync(Guid id)
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
                .FirstOrDefaultAsync(p => p.Id == id);
            if (potential is null)
                return new Result<PotentialDto> { IsError = true, Error = "Potential not found" };

            return new Result<PotentialDto> { Value = _mapper.Map<PotentialDto>(potential) };
        }
        catch (Exception)
        {
            return new Result<PotentialDto> { IsError = true, Error = "A server error occurred" };
        }
    }

    public async Task<Result<object>> CreatePotentialAsync(PotentialCreateDto potentialCreate)
    {
        try
        {
            var potential = _mapper.Map<Potential>(potentialCreate);

            foreach (var prerequisiteDto in potentialCreate.Prerequisites)
            {
                var existingPrerequisite = await _context.PotentialPrerequisites
                    .FirstOrDefaultAsync(s => s.Id == prerequisiteDto.Id);
                if (existingPrerequisite is null)
                    return new Result<object> { IsError = true, Error = "PotentialPrerequisite not found" };

                potential.Prerequisites.Add(existingPrerequisite);
            }

            if (potentialCreate.CultId is not null && potentialCreate.CultId != Guid.Empty)
            {
                potential.Cult = await _context.Cults
                    .FirstOrDefaultAsync(c => c.Id == potentialCreate.CultId);
                if (potential.Cult is null)
                    return new Result<object> { IsError = true, Error = "Cult not found" };
            }
            else
                potential.Cult = null;

            _context.Potentials.Add(potential);
            await _context.SaveChangesAsync();
            return new Result<object> { Value = null };
        }
        catch (Exception)
        {
            return new Result<object> { IsError = true, Error = "A server error occurred" };
        }
    }

    public async Task<Result<object>> UpdatePotentialAsync(PotentialDto potentialDto)
    {
        try
        {
            var existingPotential = await _context.Potentials
                .Include(p => p.Prerequisites)
                .Include(p => p.Cult)
                .FirstOrDefaultAsync(p => p.Id == potentialDto.Id);
            if (existingPotential is null)
                return new Result<object> { IsError = true, Error = "Potential not found" };

            _mapper.Map(potentialDto, existingPotential);

            existingPotential.Prerequisites.Clear();
            foreach (var prerequisiteDto in potentialDto.Prerequisites)
            {
                var prerequisite = await _context.PotentialPrerequisites
                    .FirstOrDefaultAsync(s => s.Id == prerequisiteDto.Id);
                if (prerequisite is null)
                    return new Result<object> { IsError = true, Error = "PotentialPrerequisite not found" };

                existingPotential.Prerequisites.Add(prerequisite);
            }

            if(potentialDto.CultId is not null)
            {
                existingPotential.Cult = await _context.Cults
                    .FirstOrDefaultAsync(c => c.Id == potentialDto.CultId);
                if (existingPotential.Cult is null)
                    return new Result<object> { IsError = true, Error = "Cult not found" };
            }
            else
            {
                existingPotential.Cult = null;
                existingPotential.CultId = null;
            }

            await _context.SaveChangesAsync();
            return new Result<object> { Value = null };
        }
        catch (Exception)
        {
            return new Result<object> { IsError = true, Error = "A server error occurred" };
        }
    }

    public async Task<Result<object>> DeletePotentialAsync(Guid id)
    {
        try
        {
            var potential = await _context.Potentials
                .Include(p => p.Prerequisites)
                .Include(p => p.Cult)
                .FirstOrDefaultAsync(p => p.Id == id);
            if (potential is null)
                return new Result<object> { IsError = true, Error = "Potential not found" };

            _context.Potentials.Remove(potential);
            await _context.SaveChangesAsync();
            return new Result<object> { Value = null };
        }
        catch (Exception)
        {
            return new Result<object> { IsError = true, Error = "A server error occurred" };
        }
    }
}
