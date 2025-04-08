using AutoMapper;
using DataAccessLayer;
using Degenesis.Shared.DTOs.Characters;
using Domain.Characters;
using Microsoft.EntityFrameworkCore;
using System;

namespace Business.Characters;
public interface ISkillService
{
    Task<List<SkillDto>> GetAllSkillsAsync();
    Task<Skill?> GetSkillByIdAsync(Guid id);
    Task<Skill?> CreateSkillAsync(SkillCreateDto skillCreate);
    Task<bool> UpdateSkillAsync(Skill skill);
    Task<bool> DeleteSkillAsync(Guid id);
}
public class SkillService : ISkillService
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    public SkillService(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<SkillDto>> GetAllSkillsAsync()
    {
        return await _context.Skills
            .Include(s => s.CAttribute)
            .Select(s => new SkillDto
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
            })
            .ToListAsync();
    }

    public async Task<Skill?> GetSkillByIdAsync(Guid id)
    {
        return await _context.Skills
            .FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<Skill?> CreateSkillAsync(SkillCreateDto skillCreate)
    {
        try
        {
            var skill = _mapper.Map<Skill>(skillCreate);
            _context.Skills.Add(skill);
            await _context.SaveChangesAsync();
            return skill;
        }
        catch (Exception)
        {
            return null;
        }
    }

    public async Task<bool> UpdateSkillAsync(Skill skill)
    {
        try
        {
            var existing = await _context.Skills.FindAsync(skill.Id);
            if (existing is null) return false;

            _mapper.Map(skill, existing);

            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public async Task<bool> DeleteSkillAsync(Guid id)
    {
        try
        {
            var existingSkill = await _context.Skills.FindAsync(id);
            if (existingSkill is null)
            {
                return false;
            }

            _context.Skills.Remove(existingSkill);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
}
