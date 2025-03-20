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
            .Include(rp => rp.SkillRequired)
            .ToListAsync();
        return _mapper.Map<IEnumerable<RankPrerequisiteDto>>(rankPrerequisites);
    }

    public async Task<RankPrerequisiteDto?> GetRankPrerequisiteByIdAsync(Guid id)
    {
        var rankPrerequisite = await _context.RankPrerequisites
            .Include(rp => rp.AttributeRequired)
            .Include(rp => rp.SkillRequired)
            .FirstOrDefaultAsync(rp => rp.Id == id);

        return rankPrerequisite is null ? null : _mapper.Map<RankPrerequisiteDto>(rankPrerequisite);
    }

    public async Task<RankPrerequisiteDto?> CreateRankPrerequisiteAsync(RankPrerequisiteCreateDto rankPrerequisiteCreate)
    {
        var rankPrerequisite = _mapper.Map<RankPrerequisite>(rankPrerequisiteCreate);

        if (rankPrerequisiteCreate.AttributeRequiredId != Guid.Empty)
        {
            var attribute = await _context.Attributes.FindAsync(rankPrerequisiteCreate.AttributeRequiredId);
            if (attribute != null)
                rankPrerequisite.AttributeRequired = attribute;
        }

        if (rankPrerequisiteCreate.SkillRequiredId != Guid.Empty)
        {
            var skill = await _context.Skills.FindAsync(rankPrerequisiteCreate.SkillRequiredId);
            if (skill != null)
                rankPrerequisite.SkillRequired = skill;
        }

        _context.RankPrerequisites.Add(rankPrerequisite);
        await _context.SaveChangesAsync();
        return _mapper.Map<RankPrerequisiteDto>(rankPrerequisite);
    }

    public async Task<bool> UpdateRankPrerequisiteAsync(RankPrerequisiteDto rankPrerequisiteDto)
    {
        var existingRankPrerequisite = await _context.RankPrerequisites
            .Include(rp => rp.AttributeRequired)
            .Include(rp => rp.SkillRequired)
            .FirstOrDefaultAsync(rp => rp.Id == rankPrerequisiteDto.Id);

        if (existingRankPrerequisite is null || rankPrerequisiteDto.AttributeRequired is null)
            return false;

        _mapper.Map(rankPrerequisiteDto, existingRankPrerequisite);

        existingRankPrerequisite.AttributeRequired = await _context.Attributes.FirstAsync(a => a.Id == rankPrerequisiteDto.AttributeRequired.Id);

        if(rankPrerequisiteDto.SkillRequired is not null)
            existingRankPrerequisite.SkillRequired = await _context.Skills.FindAsync(rankPrerequisiteDto.SkillRequired.Id);

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteRankPrerequisiteAsync(Guid id)
    {
        var rankPrerequisite = await _context.RankPrerequisites.FindAsync(id);
        if (rankPrerequisite is null)
            return false;

        _context.RankPrerequisites.Remove(rankPrerequisite);
        await _context.SaveChangesAsync();
        return true;
    }
}
