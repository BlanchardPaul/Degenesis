using AutoMapper;
using DataAccessLayer;
using Degenesis.Shared.DTOs.Characters;
using Domain.Characters;
using Microsoft.EntityFrameworkCore;

namespace Business.Characters;

public interface IRankPrerequisiteService
{
    Task<IEnumerable<RankPrerequisiteDto>> GetAllRankPrerequisitesAsync();
    Task<RankPrerequisiteDto?> GetRankPrerequisiteByIdAsync(Guid id);
    Task<RankPrerequisiteDto?> CreateRankPrerequisiteAsync(RankPrerequisiteCreateDto rankPrerequisiteCreate);
    Task<bool> UpdateRankPrerequisiteAsync(RankPrerequisiteDto rankPrerequisite);
    Task<bool> DeleteRankPrerequisiteAsync(Guid id);
}

public class RankPrerequisiteService : IRankPrerequisiteService
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public RankPrerequisiteService(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<RankPrerequisiteDto>> GetAllRankPrerequisitesAsync()
    {
        var rankPrerequisites = await _context.RankPrerequisites
            .Include(rp => rp.AttributeRequired)
            .Include(rp => rp.BackgroundRequired)
            .Include(rp => rp.SkillRequired)
            .ToListAsync();
        return _mapper.Map<IEnumerable<RankPrerequisiteDto>>(rankPrerequisites);
    }

    public async Task<RankPrerequisiteDto?> GetRankPrerequisiteByIdAsync(Guid id)
    {
        var rankPrerequisite = await _context.RankPrerequisites
            .Include(rp => rp.AttributeRequired)
            .Include(r => r.BackgroundRequired)
            .Include(rp => rp.SkillRequired)
            .FirstOrDefaultAsync(rp => rp.Id == id);

        return rankPrerequisite is null ? null : _mapper.Map<RankPrerequisiteDto>(rankPrerequisite);
    }

    public async Task<RankPrerequisiteDto?> CreateRankPrerequisiteAsync(RankPrerequisiteCreateDto createDto)
    {
        try
        {
            var rankPrerequisite = _mapper.Map<RankPrerequisite>(createDto);
            rankPrerequisite.Id = Guid.NewGuid();

            if (createDto.AttributeRequiredId is not null)
                rankPrerequisite.AttributeRequired = await _context.Attributes.FindAsync(createDto.AttributeRequiredId);

            if (createDto.SkillRequiredId is not null)
                rankPrerequisite.SkillRequired = await _context.Skills.FindAsync(createDto.SkillRequiredId);

            if (createDto.BackgroundRequiredId is not null)
                rankPrerequisite.BackgroundRequired = await _context.Backgrounds.FindAsync(createDto.BackgroundRequiredId);

            _context.RankPrerequisites.Add(rankPrerequisite);
            await _context.SaveChangesAsync();

            return _mapper.Map<RankPrerequisiteDto>(rankPrerequisite);
        }
        catch (Exception)
        {
            return null;
        }
    }

    public async Task<bool> UpdateRankPrerequisiteAsync(RankPrerequisiteDto rankPrerequisiteDto)
    {
        try
        {
            var existingRankPrerequisite = await _context.RankPrerequisites
                .Include(rp => rp.AttributeRequired)
                .Include(rp => rp.SkillRequired)
                .Include(rp => rp.BackgroundRequired)
                .FirstOrDefaultAsync(rp => rp.Id == rankPrerequisiteDto.Id);

            if (existingRankPrerequisite is null || rankPrerequisiteDto.AttributeRequired is null)
            return false;

            _mapper.Map(rankPrerequisiteDto, existingRankPrerequisite);

            if (rankPrerequisiteDto.IsBackgroundPrerequisite)
            {
                existingRankPrerequisite.AttributeRequiredId = null;
                existingRankPrerequisite.AttributeRequired = null;
                existingRankPrerequisite.SkillRequiredId = null;
                existingRankPrerequisite.SkillRequired = null;
                existingRankPrerequisite.SumRequired = null;

                if (rankPrerequisiteDto.BackgroundRequired is not null)
                    existingRankPrerequisite.BackgroundRequired = await _context.Backgrounds.FindAsync(rankPrerequisiteDto.BackgroundRequired.Id);
                else
                    existingRankPrerequisite.BackgroundRequired = null;
            }
            else
            {
                existingRankPrerequisite.BackgroundRequiredId = null;
                existingRankPrerequisite.BackgroundRequired = null;
                existingRankPrerequisite.BackgroundLevelRequired = null;

                if (rankPrerequisiteDto.AttributeRequired is not null)
                    existingRankPrerequisite.AttributeRequired = await _context.Attributes.FindAsync(rankPrerequisiteDto.AttributeRequired.Id);
                else
                    existingRankPrerequisite.AttributeRequired = null;

                if (rankPrerequisiteDto.SkillRequired is not null)
                    existingRankPrerequisite.SkillRequired = await _context.Skills.FindAsync(rankPrerequisiteDto.SkillRequired.Id);
                else
                    existingRankPrerequisite.SkillRequired = null;
            }

            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public async Task<bool> DeleteRankPrerequisiteAsync(Guid id)
    {
        try
        {
            var rankPrerequisite = await _context.RankPrerequisites.FindAsync(id);
            if (rankPrerequisite is null)
                return false;

            _context.RankPrerequisites.Remove(rankPrerequisite);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
}
