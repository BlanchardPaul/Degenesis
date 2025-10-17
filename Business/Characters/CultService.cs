using AutoMapper;
using DataAccessLayer;
using Degenesis.Shared.DTOs.Characters.CRUD;
using Domain.Characters;
using Microsoft.EntityFrameworkCore;

namespace Business.Characters;
public interface ICultService
{
    Task<CultDto?> GetCultByIdAsync(Guid id);
    Task<List<CultDto>> GetAllCultsAsync();
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
        try
        {
            var cult = await _context.Cults
                .Include(c => c.BonusSkills)
                .FirstOrDefaultAsync(c => c.Id == id) ?? throw new Exception("Cult not found");
            return _mapper.Map<CultDto>(cult);
        }
        catch (Exception)
        {
            return null;
        }
    }

    public async Task<List<CultDto>> GetAllCultsAsync()
    {
        var cults = await _context.Cults
            .Include(c => c.BonusSkills)
            .ToListAsync();
        return _mapper.Map<List<CultDto>>(cults);
    }

    public async Task<CultDto?> CreateCultAsync(CultCreateDto cultCreate)
    {
        try
        {
            var cult = _mapper.Map<Cult>(cultCreate);

            foreach (var skillDto in cultCreate.BonusSkills)
            {
                var existingSkill = await _context.Skills.FindAsync(skillDto.Id) ?? throw new Exception("Skill not found");
                cult.BonusSkills.Add(existingSkill);
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
                .FirstOrDefaultAsync(c => c.Id == cultDto.Id) ?? throw new Exception("Cult not found");

            _mapper.Map(cultDto, existingCult);

            existingCult.BonusSkills.Clear();
            foreach (var skillDto in cultDto.BonusSkills)
            {
                var skill = await _context.Skills
                    .FirstOrDefaultAsync(s => s.Id == skillDto.Id) ?? throw new Exception("Skill not found");
                existingCult.BonusSkills.Add(skill);
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
            var cult = await _context.Cults
                .Include(c => c.BonusSkills)
                .FirstOrDefaultAsync(c => c.Id == id) ?? throw new Exception("Cult not found");

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
