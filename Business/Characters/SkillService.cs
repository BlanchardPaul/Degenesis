using AutoMapper;
using DataAccessLayer;
using Degenesis.Shared.DTOs;
using Degenesis.Shared.DTOs.Characters.CRUD;
using Domain.Characters;
using Microsoft.EntityFrameworkCore;

namespace Business.Characters;
public interface ISkillService
{
    Task<Result<List<SkillDto>>> GetAllSkillsAsync();
    Task<Result<SkillDto>> GetSkillByIdAsync(Guid id);
    Task<Result<object>> CreateSkillAsync(SkillCreateDto skillCreate);
    Task<Result<object>> UpdateSkillAsync(SkillDto skill);
    Task<Result<object>> DeleteSkillAsync(Guid id);
}
public class SkillService(ApplicationDbContext context, IMapper mapper) : ISkillService
{
    private readonly ApplicationDbContext _context = context;
    private readonly IMapper _mapper = mapper;

    public async Task<Result<List<SkillDto>>> GetAllSkillsAsync()
    {
        try
        {
            var skills = await _context.Skills
                .Include(s => s.CAttribute)
                .OrderBy(s => s.Name)
                .ToListAsync();
            return new Result<List<SkillDto>> { Value = _mapper.Map<List<SkillDto>>(skills) };
        }
        catch (Exception)
        {
            return new Result<List<SkillDto>> { IsError = true, Error = "A server error occurred" };
        }

    }

    public async Task<Result<SkillDto>> GetSkillByIdAsync(Guid id)
    {
        try
        {
            var skill = await _context.Skills
                .Include(s => s.CAttribute)
                .FirstOrDefaultAsync(a => a.Id == id);
            if (skill is null)
                return new Result<SkillDto> { IsError = true, Error = "Skill not found" };

            return new Result<SkillDto> { Value = _mapper.Map<SkillDto>(skill) };
        }
        catch (Exception)
        {
            return new Result<SkillDto> { IsError = true, Error = "A server error occurred" };
        }
    }

    public async Task<Result<object>> CreateSkillAsync(SkillCreateDto skillCreate)
    {
        try
        {
            var skill = _mapper.Map<Skill>(skillCreate);

            // Ensure IsFocusOriented comes from parent Attribute
            var parentAttribute = await _context.Attributes
                .FirstOrDefaultAsync(a => a.Id == skillCreate.CAttributeId);
            if (parentAttribute is null)
                return new Result<object> { IsError = true, Error = "Parent attribute not found" };

            skill.CAttribute = parentAttribute;
            skill.IsFocusOriented = parentAttribute.IsFocusOriented;

            _context.Skills.Add(skill);
            await _context.SaveChangesAsync();
            return new Result<object> { Value = null };
        }
        catch (Exception)
        {
            return new Result<object> { IsError = true, Error = "A server error occurred" };
        }
    }

    public async Task<Result<object>> UpdateSkillAsync(SkillDto skill)
    {
        try
        {
            var existing = await _context.Skills
                .Include(s => s.CAttribute)
                .FirstOrDefaultAsync(a => a.Id == skill.Id);
            if (existing is null)
                return new Result<object> { IsError = true, Error = "Skill not found" };

            _mapper.Map(skill, existing);

            var parentAttribute = await _context.Attributes
                .FirstOrDefaultAsync(a => a.Id == skill.CAttributeId);
            if (parentAttribute is null)
                return new Result<object> { IsError = true, Error = "Parent attribute not found" };

            existing.CAttribute = parentAttribute;
            existing.IsFocusOriented = parentAttribute.IsFocusOriented;

            await _context.SaveChangesAsync();
            return new Result<object> { Value = null };
        }
        catch (Exception)
        {
            return new Result<object> { IsError = true, Error = "A server error occurred" };
        }
    }

    public async Task<Result<object>> DeleteSkillAsync(Guid id)
    {
        try
        {
            var existingSkill = await _context.Skills
                .Include(s => s.CAttribute)
                .FirstOrDefaultAsync(a => a.Id == id);
            if (existingSkill is null)
                return new Result<object> { IsError = true, Error = "Skill not found" };

            _context.Skills.Remove(existingSkill);
            await _context.SaveChangesAsync();
            return new Result<object> { Value = null };
        }
        catch (Exception)
        {
            return new Result<object> { IsError = true, Error = "A server error occurred" };
        }
    }
}
