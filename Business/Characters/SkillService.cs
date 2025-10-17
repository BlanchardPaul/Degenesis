using AutoMapper;
using DataAccessLayer;
using Degenesis.Shared.DTOs.Characters.CRUD;
using Domain.Characters;
using Microsoft.EntityFrameworkCore;
using System;

namespace Business.Characters;
public interface ISkillService
{
    Task<List<SkillDto>> GetAllSkillsAsync();
    Task<SkillDto?> GetSkillByIdAsync(Guid id);
    Task<SkillDto?> CreateSkillAsync(SkillCreateDto skillCreate);
    Task<bool> UpdateSkillAsync(SkillDto skill);
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
        var skills = await _context.Skills
            .Include(s => s.CAttribute)
            .ToListAsync();
        return _mapper.Map<List<SkillDto>>(skills);
    }

    public async Task<SkillDto?> GetSkillByIdAsync(Guid id)
    {
        try
        {
            var skill = await _context.Skills
            .Include(s => s.CAttribute)
            .FirstOrDefaultAsync(a => a.Id == id)
            ?? throw new Exception("Skill not found");

            return _mapper.Map<SkillDto>(skill);
        }
        catch (Exception)
        {
            return null;
        }
    }

    public async Task<SkillDto?> CreateSkillAsync(SkillCreateDto skillCreate)
    {
        try
        {
            var skill = _mapper.Map<Skill>(skillCreate);

            // Ensure IsFocusOriented comes from parent Attribute
            var parentAttribute = await _context.Attributes
                .FirstOrDefaultAsync(a => a.Id == skillCreate.CAttributeId)
                ?? throw new Exception("Attribute not found");

            skill.CAttribute = parentAttribute;
            skill.IsFocusOriented = parentAttribute.IsFocusOriented;

            _context.Skills.Add(skill);
            await _context.SaveChangesAsync();
            return _mapper.Map<SkillDto>(skill);
        }
        catch (Exception)
        {
            return null;
        }
    }

    public async Task<bool> UpdateSkillAsync(SkillDto skill)
    {
        try
        {
            var existing = await _context.Skills
                .Include(s => s.CAttribute)
                .FirstOrDefaultAsync(a => a.Id == skill.Id)
                ?? throw new Exception("Skill not found");

            _mapper.Map(skill, existing);

            var parentAttribute = await _context.Attributes
                .FirstOrDefaultAsync(a => a.Id == skill.CAttributeId)
                ?? throw new Exception("Attribute not found");

            existing.CAttribute = parentAttribute;
            existing.IsFocusOriented = parentAttribute.IsFocusOriented;

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
            var existingSkill = await _context.Skills
                .Include(s => s.CAttribute)
                .FirstOrDefaultAsync(a => a.Id == id)
                ?? throw new Exception("Skill not found");

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
