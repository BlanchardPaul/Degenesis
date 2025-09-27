using AutoMapper;
using DataAccessLayer;
using Degenesis.Shared.DTOs.Characters;
using Domain.Characters;
using Microsoft.EntityFrameworkCore;

namespace Business.Characters;

public interface IPotentialPrerequisiteService
{
    Task<IEnumerable<PotentialPrerequisiteDto>> GetAllPotentialPrerequisitesAsync();
    Task<PotentialPrerequisiteDto?> GetPotentialPrerequisiteByIdAsync(Guid id);
    Task<PotentialPrerequisiteDto?> CreatePotentialPrerequisiteAsync(PotentialPrerequisiteCreateDto createDto);
    Task<bool> UpdatePotentialPrerequisiteAsync(PotentialPrerequisiteDto prerequisiteDto);
    Task<bool> DeletePotentialPrerequisiteAsync(Guid id);
}

public class PotentialPrerequisiteService : IPotentialPrerequisiteService
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public PotentialPrerequisiteService(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<PotentialPrerequisiteDto>> GetAllPotentialPrerequisitesAsync()
    {
        var prerequisites = await _context.PotentialPrerequisites
            .Include(pp => pp.AttributeRequired)
            .Include(pp => pp.SkillRequired)
            .Include(pp => pp.BackgroundRequired)
            .Include(pp => pp.RankRequired)
            .ToListAsync();

        return _mapper.Map<IEnumerable<PotentialPrerequisiteDto>>(prerequisites);
    }

    public async Task<PotentialPrerequisiteDto?> GetPotentialPrerequisiteByIdAsync(Guid id)
    {
        var prerequisite = await _context.PotentialPrerequisites
            .Include(pp => pp.AttributeRequired)
            .Include(pp => pp.SkillRequired)
            .Include(pp => pp.BackgroundRequired)
            .Include(pp => pp.RankRequired)
            .FirstOrDefaultAsync(pp => pp.Id == id);

        return prerequisite is null ? null : _mapper.Map<PotentialPrerequisiteDto>(prerequisite);
    }

    public async Task<PotentialPrerequisiteDto?> CreatePotentialPrerequisiteAsync(PotentialPrerequisiteCreateDto createDto)
    {
        try
        {
            var prerequisite = _mapper.Map<PotentialPrerequisite>(createDto);
            prerequisite.Id = Guid.NewGuid();

            if (createDto.AttributeRequiredId is not null)
                prerequisite.AttributeRequired = await _context.Attributes.FindAsync(createDto.AttributeRequiredId);

            if (createDto.SkillRequiredId is not null)
                prerequisite.SkillRequired = await _context.Skills.FindAsync(createDto.SkillRequiredId);

            if (createDto.BackgroundRequiredId is not null)
                prerequisite.BackgroundRequired = await _context.Backgrounds.FindAsync(createDto.BackgroundRequiredId);

            if (createDto.RankRequiredId is not null)
                prerequisite.RankRequired = await _context.Ranks.FindAsync(createDto.RankRequiredId);

            _context.PotentialPrerequisites.Add(prerequisite);
            await _context.SaveChangesAsync();

            return _mapper.Map<PotentialPrerequisiteDto>(prerequisite);
        }
        catch (Exception)
        {
            return null;
        }
    }

    public async Task<bool> UpdatePotentialPrerequisiteAsync(PotentialPrerequisiteDto prerequisiteDto)
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
                return false;

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
                    existing.BackgroundRequired = await _context.Backgrounds.FindAsync(prerequisiteDto.BackgroundRequired.Id);
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
                    existing.RankRequired = await _context.Ranks.FindAsync(prerequisiteDto.RankRequired.Id);
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
                    existing.AttributeRequired = await _context.Attributes.FindAsync(prerequisiteDto.AttributeRequired.Id);
                else
                    existing.AttributeRequired = null;

                if (prerequisiteDto.SkillRequired is not null)
                    existing.SkillRequired = await _context.Skills.FindAsync(prerequisiteDto.SkillRequired.Id);
                else
                    existing.SkillRequired = null;
            }

            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public async Task<bool> DeletePotentialPrerequisiteAsync(Guid id)
    {
        try
        {
            var prerequisite = await _context.PotentialPrerequisites.FindAsync(id);
            if (prerequisite is null)
                return false;

            _context.PotentialPrerequisites.Remove(prerequisite);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
}
