using DataAccessLayer;
using Domain.Characters;
using Microsoft.EntityFrameworkCore;

namespace Business.Characters;
public interface ISkillService
{
    Task<List<Skill>> GetAllSkillsAsync();
    Task<Skill?> GetSkillByIdAsync(Guid id);
    Task<Skill> CreateSkillAsync(Skill skill);
    Task<Skill?> UpdateSkillAsync(Guid id, Skill skill);
    Task<bool> DeleteSkillAsync(Guid id);
}
public class SkillService : ISkillService
{
    private readonly ApplicationDbContext _context;

    public SkillService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Skill>> GetAllSkillsAsync()
    {
        return await _context.Skills
            .ToListAsync();
    }

    public async Task<Skill?> GetSkillByIdAsync(Guid id)
    {
        return await _context.Skills
            .FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<Skill> CreateSkillAsync(Skill skill)
    {
        _context.Skills.Add(skill);
        await _context.SaveChangesAsync();
        return skill;
    }

    public async Task<Skill?> UpdateSkillAsync(Guid id, Skill skill)
    {
        var existingSkill = await _context.Skills.FindAsync(id);
        if (existingSkill == null)
        {
            return null;
        }

        existingSkill = skill;

        await _context.SaveChangesAsync();
        return existingSkill;
    }

    public async Task<bool> DeleteSkillAsync(Guid id)
    {
        var existingSkill = await _context.Skills.FindAsync(id);
        if (existingSkill == null)
        {
            return false;
        }

        _context.Skills.Remove(existingSkill);
        await _context.SaveChangesAsync();
        return true;
    }
}
