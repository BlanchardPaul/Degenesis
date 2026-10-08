using AutoMapper;
using DataAccessLayer;
using Degenesis.Shared.DTOs;
using Degenesis.Shared.DTOs.Characters.CRUD;
using Domain.Characters;
using Microsoft.EntityFrameworkCore;

namespace Business.Characters;

public interface IRankPrerequisiteService
{
    Task<Result<List<RankPrerequisiteDto>>> GetAllRankPrerequisitesAsync();
    Task<Result<RankPrerequisiteDto>> GetRankPrerequisiteByIdAsync(Guid id);
    Task<Result<object>> CreateRankPrerequisiteAsync(RankPrerequisiteCreateDto rankPrerequisiteCreate);
    Task<Result<object>> UpdateRankPrerequisiteAsync(RankPrerequisiteDto rankPrerequisite);
    Task<Result<object>> DeleteRankPrerequisiteAsync(Guid id);
}

public class RankPrerequisiteService(ApplicationDbContext context, IMapper mapper) : IRankPrerequisiteService
{
    private readonly ApplicationDbContext _context = context;
    private readonly IMapper _mapper = mapper;

    public async Task<Result<List<RankPrerequisiteDto>>> GetAllRankPrerequisitesAsync()
    {
        try
        {
            var rankPrerequisites = await _context.RankPrerequisites
                .Include(rp => rp.AttributeRequired)
                .Include(rp => rp.BackgroundRequired)
                .Include(rp => rp.SkillRequired)
                .ToListAsync();
            return new Result<List<RankPrerequisiteDto>> { Value = _mapper.Map<List<RankPrerequisiteDto>>(rankPrerequisites) };
        }
        catch (Exception)
        {
            return new Result<List<RankPrerequisiteDto>> { IsError = true, Error = "A server error occurred" };
        }
    }

    public async Task<Result<RankPrerequisiteDto>> GetRankPrerequisiteByIdAsync(Guid id)
    {
        try
        {
            var rankPrerequisite = await _context.RankPrerequisites
                .Include(rp => rp.AttributeRequired)
                .Include(r => r.BackgroundRequired)
                .Include(rp => rp.SkillRequired)
                .FirstOrDefaultAsync(rp => rp.Id == id);
            if (rankPrerequisite is null)
                return new Result<RankPrerequisiteDto> { IsError = true, Error = "RankPrerequisite not found" };

            return new Result<RankPrerequisiteDto> { Value = _mapper.Map<RankPrerequisiteDto>(rankPrerequisite) };
        }
        catch (Exception)
        {
            return new Result<RankPrerequisiteDto> { IsError = true, Error = "A server error occurred" };
        }
    }

    public async Task<Result<object>> CreateRankPrerequisiteAsync(RankPrerequisiteCreateDto createDto)
    {
        try
        {
            var rankPrerequisite = _mapper.Map<RankPrerequisite>(createDto);
            rankPrerequisite.Id = Guid.NewGuid();

            if (createDto.AttributeRequiredId is not null)
            {
                rankPrerequisite.AttributeRequired = await _context.Attributes.FindAsync(createDto.AttributeRequiredId);
                if (rankPrerequisite.AttributeRequired is null)
                    throw new Exception("Attribute not found");
            }

            if (createDto.SkillRequiredId is not null)
            {
                rankPrerequisite.SkillRequired = await _context.Skills.FindAsync(createDto.SkillRequiredId);
                if (rankPrerequisite.SkillRequired is null)
                    throw new Exception("Skill not found");
            }

            if (createDto.BackgroundRequiredId is not null)
            {
                rankPrerequisite.BackgroundRequired = await _context.Backgrounds.FindAsync(createDto.BackgroundRequiredId);
                if (rankPrerequisite.BackgroundRequired is null)
                    throw new Exception("Background not found");
            }

            _context.RankPrerequisites.Add(rankPrerequisite);
            await _context.SaveChangesAsync();

            return new Result<object> { Value = null };
        }
        catch (Exception)
        {
            return new Result<object> { IsError = true, Error = "A server error occurred" };
        }
    }

    public async Task<Result<object>> UpdateRankPrerequisiteAsync(RankPrerequisiteDto rankPrerequisiteDto)
    {
        try
        {
            var existingRankPrerequisite = await _context.RankPrerequisites
                .Include(rp => rp.AttributeRequired)
                .Include(rp => rp.SkillRequired)
                .Include(rp => rp.BackgroundRequired)
                .FirstOrDefaultAsync(rp => rp.Id == rankPrerequisiteDto.Id);
            if (existingRankPrerequisite is null)
                return new Result<object> { IsError = true, Error = "RankPrerequisite not found" };

            _mapper.Map(rankPrerequisiteDto, existingRankPrerequisite);

            if (rankPrerequisiteDto.IsBackgroundPrerequisite)
            {
                existingRankPrerequisite.AttributeRequiredId = null;
                existingRankPrerequisite.AttributeRequired = null;
                existingRankPrerequisite.SkillRequiredId = null;
                existingRankPrerequisite.SkillRequired = null;
                existingRankPrerequisite.SumRequired = null;

                if (rankPrerequisiteDto.BackgroundRequired is not null)
                {
                    existingRankPrerequisite.BackgroundRequired = await _context.Backgrounds.FindAsync(rankPrerequisiteDto.BackgroundRequired.Id);
                    if (existingRankPrerequisite.BackgroundRequired is null)
                        return new Result<object> { IsError = true, Error = "Background not found" };
                }

                else
                    existingRankPrerequisite.BackgroundRequired = null;
            }
            else
            {
                existingRankPrerequisite.BackgroundRequiredId = null;
                existingRankPrerequisite.BackgroundRequired = null;
                existingRankPrerequisite.BackgroundLevelRequired = null;

                if (rankPrerequisiteDto.AttributeRequired is not null) 
                {
                    existingRankPrerequisite.AttributeRequired = await _context.Attributes.FindAsync(rankPrerequisiteDto.AttributeRequired.Id);
                    if (existingRankPrerequisite.AttributeRequired is null)
                        return new Result<object> { IsError = true, Error = "Attribute not found" };
                }
                else
                    existingRankPrerequisite.AttributeRequired = null;

                if (rankPrerequisiteDto.SkillRequired is not null)
                {
                    existingRankPrerequisite.SkillRequired = await _context.Skills.FindAsync(rankPrerequisiteDto.SkillRequired.Id);
                    if (existingRankPrerequisite.SkillRequired is null)
                        return new Result<object> { IsError = true, Error = "Skill not found" };
                }
                else
                    existingRankPrerequisite.SkillRequired = null;
            }

            await _context.SaveChangesAsync();
            return new Result<object> { Value = null };
        }
        catch (Exception)
        {
            return new Result<object> { IsError = true, Error = "A server error occurred" };
        }
    }

    public async Task<Result<object>> DeleteRankPrerequisiteAsync(Guid id)
    {
        try
        {
            var rankPrerequisite = await _context.RankPrerequisites
                .Include(rp => rp.AttributeRequired)
                .Include(rp => rp.SkillRequired)
                .Include(rp => rp.BackgroundRequired)
                .FirstOrDefaultAsync(rp => rp.Id == id);
            if (rankPrerequisite is null)
                return new Result<object> { IsError = true, Error = "RankPrerequisite not found" };

            _context.RankPrerequisites.Remove(rankPrerequisite);
            await _context.SaveChangesAsync();
            return new Result<object> { Value = null };
        }
        catch (Exception)
        {
            return new Result<object> { IsError = true, Error = "A server error occurred" };
        }
    }
}
