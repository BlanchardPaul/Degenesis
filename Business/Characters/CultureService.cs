using AutoMapper;
using DataAccessLayer;
using Degenesis.Shared.DTOs.Characters;
using Domain.Characters;
using Microsoft.EntityFrameworkCore;
using System;
using System.Text.RegularExpressions;

namespace Business.Characters;

public interface ICultureService
{
    Task<IEnumerable<CultureDto>> GetAllCulturesAsync();
    Task<CultureDto?> GetCultureByIdAsync(Guid id);
    Task<CultureDto?> CreateCultureAsync(CultureCreateDto cultureCreate);
    Task<bool> UpdateCultureAsync(CultureDto culture);
    Task<bool> DeleteCultureAsync(Guid id);
}

public class CultureService : ICultureService
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public CultureService(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<CultureDto>> GetAllCulturesAsync()
    {
        var cultures = await _context.Cultures
            .Include(c => c.BonusAttributes)
            .Include(c => c.BonusSkills)
            .Include(c => c.AvailableCults)
            .ToListAsync();
        return _mapper.Map<IEnumerable<CultureDto>>(cultures);
    }

    public async Task<CultureDto?> GetCultureByIdAsync(Guid id)
    {
        try
        {
            var culture = await _context.Cultures
                .Include(c => c.AvailableCults)
                .Include(c => c.BonusAttributes)
                .Include(c => c.BonusSkills)
                .FirstOrDefaultAsync(c => c.Id == id) ?? throw new Exception("Culture not found");
            return _mapper.Map<CultureDto>(culture);
        }
        catch (Exception)
        {
            return null;
        }
    }

    public async Task<CultureDto?> CreateCultureAsync(CultureCreateDto cultureCreate)
    {

        try
        {
            var culture = _mapper.Map<Culture>(cultureCreate);

            foreach (var cultDto in cultureCreate.AvailableCults)
            {
                var existingCult = await _context.Cults
                    .FirstOrDefaultAsync(s => s.Id == cultDto.Id)
                    ?? throw new Exception("Cult not found");

                culture.AvailableCults.Add(existingCult);
            }

            foreach (var attributeDto in cultureCreate.BonusAttributes)
            {
                var existingAttribute = await _context.Attributes
                    .FirstOrDefaultAsync(s => s.Id == attributeDto.Id)
                    ?? throw new Exception("Attribute not found");

                culture.BonusAttributes.Add(existingAttribute);
            }

            foreach (var skillDto in cultureCreate.BonusSkills)
            {
                var existingSkill = await _context.Skills
                    .FirstOrDefaultAsync(s => s.Id == skillDto.Id)
                    ?? throw new Exception("Skill not found");

                culture.BonusSkills.Add(existingSkill);
            }

            _context.Cultures.Add(culture);
            await _context.SaveChangesAsync();
            return _mapper.Map<CultureDto>(culture);
        }
        catch (Exception)
        {
            return null;
        }
    }

    public async Task<bool> UpdateCultureAsync(CultureDto cultureDto)
    {

        try
        {
            var existingCulture = await _context.Cultures
                .Include(c => c.AvailableCults)
                .Include(c => c.BonusAttributes)
                .Include(c => c.BonusSkills)
                .FirstOrDefaultAsync(c => c.Id == cultureDto.Id) ?? throw new Exception("Culture not found");

            _mapper.Map(cultureDto, existingCulture);

            existingCulture.AvailableCults.Clear();
            foreach (var cultDto in cultureDto.AvailableCults)
            {
                var existingCult = await _context.Cults
                    .FirstOrDefaultAsync(s => s.Id == cultDto.Id)
                    ?? throw new Exception("Cult not found");

                existingCulture.AvailableCults.Add(existingCult);
            }

            existingCulture.BonusAttributes.Clear();
            foreach (var attributeDto in cultureDto.BonusAttributes)
            {
                var existingAttribute = await _context.Attributes
                    .FirstOrDefaultAsync(s => s.Id == attributeDto.Id)
                    ?? throw new Exception("Attribute not found");

                existingCulture.BonusAttributes.Add(existingAttribute);
            }

            existingCulture.BonusSkills.Clear();
            foreach (var skillDto in cultureDto.BonusSkills)
            {
                var existingSkill = await _context.Skills
                    .FirstOrDefaultAsync(s => s.Id == skillDto.Id)
                    ?? throw new Exception("Skill not found");

                existingCulture.BonusSkills.Add(existingSkill);
            }

            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public async Task<bool> DeleteCultureAsync(Guid id)
    {
        try
        {
            var culture = await _context.Cultures
                .Include(c => c.AvailableCults)
                .Include(c => c.BonusAttributes)
                .Include(c => c.BonusSkills)
                .FirstOrDefaultAsync(c => c.Id == id) ?? throw new Exception("Culture not found");

            _context.Cultures.Remove(culture);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
}