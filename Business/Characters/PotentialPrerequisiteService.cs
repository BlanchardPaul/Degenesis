using AutoMapper;
using DataAccessLayer;
using Degenesis.Shared.DTOs;
using Degenesis.Shared.DTOs.Characters.CRUD;
using Domain.Characters;
using Microsoft.EntityFrameworkCore;

namespace Business.Characters;

public interface IPotentialPrerequisiteService
{
    Task<Result<List<PotentialPrerequisiteDto>>> GetAllPotentialPrerequisitesAsync();
    Task<Result<PotentialPrerequisiteDto>> GetPotentialPrerequisiteByIdAsync(Guid id);
    Task<Result<object>> CreatePotentialPrerequisiteAsync(PotentialPrerequisiteCreateDto createDto);
    Task<Result<object>> UpdatePotentialPrerequisiteAsync(PotentialPrerequisiteDto prerequisiteDto);
    Task<Result<object>> DeletePotentialPrerequisiteAsync(Guid id);
}

public class PotentialPrerequisiteService(ApplicationDbContext context, IMapper mapper) : IPotentialPrerequisiteService
{
    private readonly ApplicationDbContext _context = context;
    private readonly IMapper _mapper = mapper;

    public async Task<Result<List<PotentialPrerequisiteDto>>> GetAllPotentialPrerequisitesAsync()
    {
        try
        {
            var prerequisites = await _context.PotentialPrerequisites
                .Include(pp => pp.AttributeRequired)
                .Include(pp => pp.SkillRequired)
                .Include(pp => pp.BackgroundRequired)
            .Include(pp => pp.RankRequired)
            .ToListAsync();

            return new Result<List<PotentialPrerequisiteDto>> { Value = _mapper.Map<List<PotentialPrerequisiteDto>>(prerequisites) };
        }
        catch (Exception)
        {
            return new Result<List<PotentialPrerequisiteDto>> { IsError = true, Error = "A server error occurred" };
        }
    }

    public async Task<Result<PotentialPrerequisiteDto>> GetPotentialPrerequisiteByIdAsync(Guid id)
    {
        try
        {
            var prerequisite = await _context.PotentialPrerequisites
            .Include(pp => pp.AttributeRequired)
            .Include(pp => pp.SkillRequired)
            .Include(pp => pp.BackgroundRequired)
            .Include(pp => pp.RankRequired)
            .FirstOrDefaultAsync(pp => pp.Id == id);
            if (prerequisite is null)
                return new Result<PotentialPrerequisiteDto> { IsError = true, Error = "PotentialPrerequisite not found" };

            return new Result<PotentialPrerequisiteDto> { Value = _mapper.Map<PotentialPrerequisiteDto>(prerequisite) };
        }
        catch (Exception)
        {
            return new Result<PotentialPrerequisiteDto> { IsError = true, Error = "A server error occurred" };
        }
    }

    public async Task<Result<object>> CreatePotentialPrerequisiteAsync(PotentialPrerequisiteCreateDto createDto)
    {
        try
        {
            var prerequisite = _mapper.Map<PotentialPrerequisite>(createDto);
            prerequisite.Id = Guid.NewGuid();

            if (createDto.AttributeRequiredId is not null)
            {
                prerequisite.AttributeRequired = await _context.Attributes
                    .FindAsync(createDto.AttributeRequiredId);
                if (prerequisite.AttributeRequired is null)
                    return new Result<object> { IsError = true, Error = "Attribute not found" };
            }

            if (createDto.SkillRequiredId is not null)
            {
                prerequisite.SkillRequired = await _context.Skills
                    .FindAsync(createDto.SkillRequiredId);
                if (prerequisite.SkillRequired is null)
                    return new Result<object> { IsError = true, Error = "Skill not found" };
            }

            if (createDto.BackgroundRequiredId is not null)
            {
                prerequisite.BackgroundRequired = await _context.Backgrounds
                    .FindAsync(createDto.BackgroundRequiredId);
                if (prerequisite.BackgroundRequired is null)
                    return new Result<object> { IsError = true, Error = "Background not found" };
            }

            if (createDto.RankRequiredId is not null)
            {
                prerequisite.RankRequired = await _context.Ranks
                    .FindAsync(createDto.RankRequiredId);
                if (prerequisite.RankRequired is null)
                    return new Result<object> { IsError = true, Error = "Rank not found" };
            }

            _context.PotentialPrerequisites.Add(prerequisite);
            await _context.SaveChangesAsync();

            return new Result<object> { Value = null };
        }
        catch (Exception)
        {
            return new Result<object> { IsError = true, Error = "A server error occurred" };
        }
    }

    public async Task<Result<object>> UpdatePotentialPrerequisiteAsync(PotentialPrerequisiteDto prerequisiteDto)
    {
        try
        {
            var existing = await _context.PotentialPrerequisites
                .Include(pp => pp.AttributeRequired)
                .Include(pp => pp.SkillRequired)
                .Include(pp => pp.BackgroundRequired)
                .Include(pp => pp.RankRequired)
                .FirstOrDefaultAsync(pp => pp.Id == prerequisiteDto.Id);

            if (existing is null)
                return new Result<object> { IsError = true, Error = "PotentialPrerequisite not found" };

            _mapper.Map(prerequisiteDto, existing);

            if (prerequisiteDto.IsBackgroundPrerequisite)
            {
                existing.AttributeRequired = null;
                existing.AttributeRequiredId = null;
                existing.SkillRequired = null;
                existing.SkillRequiredId = null;
                existing.SumRequired = null;
                existing.RankRequired = null;
                existing.RankRequiredId = null;

                if (prerequisiteDto.BackgroundRequired is not null)
                {
                    existing.BackgroundRequired = await _context.Backgrounds
                        .FindAsync(prerequisiteDto.BackgroundRequired.Id);
                    if (existing.BackgroundRequired is null)
                        return new Result<object> { IsError = true, Error = "Background not found" };
                }
                else
                    existing.BackgroundRequired = null;
            }
            else if (prerequisiteDto.IsRankPrerequisite)
            {
                existing.AttributeRequired = null;
                existing.AttributeRequiredId = null;
                existing.SkillRequired = null;
                existing.SkillRequiredId = null;
                existing.SumRequired = null;
                existing.BackgroundRequired = null;
                existing.BackgroundRequiredId = null;
                existing.BackgroundLevelRequired = null;

                if (prerequisiteDto.RankRequired is not null)
                {
                    existing.RankRequired = await _context.Ranks
                        .FindAsync(prerequisiteDto.RankRequired.Id);
                    if (existing.RankRequired is null)
                        return new Result<object> { IsError = true, Error = "Rank not found" };
                }
                else
                    existing.RankRequired = null;
            }
            else
            {
                existing.BackgroundRequired = null;
                existing.BackgroundRequiredId = null;
                existing.BackgroundLevelRequired = null;
                existing.RankRequired = null;
                existing.RankRequiredId = null;

                if (prerequisiteDto.AttributeRequired is not null)
                {
                    existing.AttributeRequired = await _context.Attributes
                        .FindAsync(prerequisiteDto.AttributeRequired.Id);
                    if (existing.AttributeRequired is null)
                        return new Result<object> { IsError = true, Error = "Attribute not found" };
                }
                else
                    existing.AttributeRequired = null;

                if (prerequisiteDto.SkillRequired is not null)
                {
                    existing.SkillRequired = await _context.Skills
                        .FindAsync(prerequisiteDto.SkillRequired.Id);
                    if (existing.SkillRequired is null)
                        return new Result<object> { IsError = true, Error = "Skill not found" };
                }
                else
                    existing.SkillRequired = null;
            }

            await _context.SaveChangesAsync();
            return new Result<object> { Value = null };
        }
        catch (Exception)
        {
            return new Result<object> { IsError = true, Error = "An error occurred" };
        }
    }

    public async Task<Result<object>> DeletePotentialPrerequisiteAsync(Guid id)
    {
        try
        {
            var prerequisite = await _context.PotentialPrerequisites
                .FindAsync(id);
            if (prerequisite is null)
                return new Result<object> { IsError = true, Error = "PotentialPrerequisite not found" };

            _context.PotentialPrerequisites.Remove(prerequisite);
            await _context.SaveChangesAsync();
            return new Result<object> { Value = null };
        }
        catch (Exception)
        {
            return new Result<object> { IsError = true, Error = "An error occurred" };
        }
    }
}
