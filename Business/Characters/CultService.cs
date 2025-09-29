using AutoMapper;
using DataAccessLayer;
using Degenesis.Shared.DTOs.Characters;
using Domain.Characters;
using Microsoft.EntityFrameworkCore;

namespace Business.Characters;
public interface ICultService
{
    Task<CultDto?> GetCultByIdAsync(Guid id);
    Task<IEnumerable<CultDto>> GetAllCultsAsync();
    Task<CultDto?> CreateCultAsync(CultCreateDto cultCreate);
    Task<bool> UpdateCultAsync(CultDto cult);
    Task<bool> DeleteCultAsync(Guid id);
}

public class CultService : ICultService
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public CultService(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<CultDto?> GetCultByIdAsync(Guid id)
    {
        return await _context.Cults
            .Include(c => c.BonusSkills)
            .Where(c => c.Id == id)
            .Select(c => new CultDto
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                BonusSkills = c.BonusSkills.Select(s => new SkillDto
                {
                    Id = s.Id,
                    Name = s.Name,
                    Description = s.Description,
                    CAttribute = new AttributeDto
                    {
                        Id = s.CAttribute.Id,
                        Name = s.CAttribute.Name,
                        Abbreviation = s.CAttribute.Abbreviation,
                        Description = s.CAttribute.Description
                    }
                }).ToList()
            })
            .FirstOrDefaultAsync();
    }
    public async Task<IEnumerable<CultDto>> GetAllCultsAsync()
    {
        return await _context.Cults
            .Include(c => c.BonusSkills)
            .Select(c => new CultDto
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                BonusSkills = c.BonusSkills.Select(s => new SkillDto
                {
                    Id = s.Id,
                    Name = s.Name,
                    Description = s.Description,
                    CAttribute = new AttributeDto
                    {
                        Id = s.CAttribute.Id,
                        Name = s.CAttribute.Name,
                        Abbreviation = s.CAttribute.Abbreviation,
                        Description = s.CAttribute.Description
                    }
                }).ToList()
            })
            .ToListAsync();
    }

    public async Task<CultDto?> CreateCultAsync(CultCreateDto cultCreate)
    {
        try
        {
            var cult = _mapper.Map<Cult>(cultCreate);
            cult.BonusSkills = new List<Skill>();

            foreach (var skillDto in cultCreate.BonusSkills)
            {
                var existingSkill = await _context.Skills.FindAsync(skillDto.Id);

                if (existingSkill != null)
                {
                    cult.BonusSkills.Add(existingSkill);
                }
            }

            _context.Cults.Add(cult);
            await _context.SaveChangesAsync();

            return _mapper.Map<CultDto>(cult);
        }
        catch (Exception)
        {
            return null;
        }

    }

    public async Task<bool> UpdateCultAsync(CultDto cultDto)
    {
        try
        {
            var existingCult = await _context.Cults
            .Include(c => c.BonusSkills)
            .FirstOrDefaultAsync(c => c.Id == cultDto.Id);

            if(existingCult is null)
                throw new Exception("Cult not found");

            _context.Entry(existingCult).CurrentValues.SetValues(cultDto);

            existingCult.BonusSkills.Clear();

            foreach (var skillDto in cultDto.BonusSkills)
            {
                var skill = await _context.Skills.FindAsync(skillDto.Id);
                if (skill != null)
                {
                    existingCult.BonusSkills.Add(skill);
                }
            }

            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
        
    }

    public async Task<bool> DeleteCultAsync(Guid id)
    {
        try
        {
            var cult = await _context.Cults.FindAsync(id);
            if (cult is null)
                return false;

            _context.Cults.Remove(cult);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
}
