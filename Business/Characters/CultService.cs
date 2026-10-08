using AutoMapper;
using DataAccessLayer;
using Degenesis.Shared.DTOs;
using Degenesis.Shared.DTOs.Characters.CRUD;
using Domain.Characters;
using Microsoft.EntityFrameworkCore;

namespace Business.Characters;
public interface ICultService
{
    Task<Result<CultDto>> GetCultByIdAsync(Guid id);
    Task<Result<List<CultDto>>> GetAllCultsAsync();
    Task<Result<object>> CreateCultAsync(CultCreateDto cultCreate);
    Task<Result<object>> UpdateCultAsync(CultDto cult);
    Task<Result<object>> DeleteCultAsync(Guid id);
}

public class CultService(ApplicationDbContext context, IMapper mapper) : ICultService
{
    private readonly ApplicationDbContext _context = context;
    private readonly IMapper _mapper = mapper;

    public async Task<Result<CultDto>> GetCultByIdAsync(Guid id)
    {
        try
        {
            var cult = await _context.Cults
                .Include(c => c.BonusSkills)
                .OrderBy(c => c.Name)
                .FirstOrDefaultAsync(c => c.Id == id);
            if (cult is null)
                return new Result<CultDto> { IsError = true, Error = "Cult not found" };

            return new Result<CultDto> { Value = _mapper.Map<CultDto>(cult) };
        }
        catch (Exception)
        {
            return new Result<CultDto> { IsError = true, Error = "A server error occurred" };
        }
    }

    public async Task<Result<List<CultDto>>> GetAllCultsAsync()
    {
        try
        {
            var cults = await _context.Cults
                .Include(c => c.BonusSkills)
                .ToListAsync();
            return new Result<List<CultDto>> { Value = _mapper.Map<List<CultDto>>(cults) };
        }
        catch (Exception)
        {
            return new Result<List<CultDto>> { IsError = true, Error = "A server error occurred" };
        }
    }

    public async Task<Result<object>> CreateCultAsync(CultCreateDto cultCreate)
    {
        try
        {
            var cult = _mapper.Map<Cult>(cultCreate);

            foreach (var skillDto in cultCreate.BonusSkills)
            {
                var existingSkill = await _context.Skills.FindAsync(skillDto.Id);
                if (existingSkill is null)
                    return new Result<object> { IsError = true, Error = "Skill not found" };
                cult.BonusSkills.Add(existingSkill);
            }

            _context.Cults.Add(cult);
            await _context.SaveChangesAsync();

            return new Result<object> { Value = null };
        }
        catch (Exception)
        {
            return new Result<object> { IsError = true, Error = "A server error occurred" };
        }

    }

    public async Task<Result<object>> UpdateCultAsync(CultDto cultDto)
    {
        try
        {
            var existingCult = await _context.Cults
                .Include(c => c.BonusSkills)
                .FirstOrDefaultAsync(c => c.Id == cultDto.Id);
            if (existingCult is null)
                return new Result<object> { IsError = true, Error = "Cult not found" };

            _mapper.Map(cultDto, existingCult);

            existingCult.BonusSkills.Clear();
            foreach (var skillDto in cultDto.BonusSkills)
            {
                var skill = await _context.Skills
                    .FirstOrDefaultAsync(s => s.Id == skillDto.Id);
                if (skill is null)
                    return new Result<object> { IsError = true, Error = "Skill not found" };

                existingCult.BonusSkills.Add(skill);
            }

            await _context.SaveChangesAsync();
            return new Result<object> { Value = null };
        }
        catch (Exception)
        {
            return new Result<object> { IsError = true, Error = "A server error occurred" };
        }
        
    }

    public async Task<Result<object>> DeleteCultAsync(Guid id)
    {
        try
        {
            var cult = await _context.Cults
                .Include(c => c.BonusSkills)
                .FirstOrDefaultAsync(c => c.Id == id);
            if (cult is null)
                return new Result<object> { IsError = true, Error = "Cult not found" };

            _context.Cults.Remove(cult);
            await _context.SaveChangesAsync();
            return new Result<object> { Value = null };
        }
        catch (Exception)
        {
            return new Result<object> { IsError = true, Error = "A server error occurred" };
        }
    }
}
