using AutoMapper;
using DataAccessLayer;
using Degenesis.Shared.DTOs;
using Degenesis.Shared.DTOs.Characters.CRUD;
using Domain.Characters;
using Microsoft.EntityFrameworkCore;

namespace Business.Characters;

public interface ICultureService
{
    Task<Result<List<CultureDto>>> GetAllCulturesAsync();
    Task<Result<CultureDto>> GetCultureByIdAsync(Guid id);
    Task<Result<object>> CreateCultureAsync(CultureCreateDto cultureCreate);
    Task<Result<object>> UpdateCultureAsync(CultureDto culture);
    Task<Result<object>> DeleteCultureAsync(Guid id);
}

public class CultureService(ApplicationDbContext context, IMapper mapper) : ICultureService
{
    private readonly ApplicationDbContext _context = context;
    private readonly IMapper _mapper = mapper;

    public async Task<Result<List<CultureDto>>> GetAllCulturesAsync()
    {
        try
        {
            var cultures = await _context.Cultures
                .Include(c => c.BonusAttributes)
                .Include(c => c.BonusSkills)
                .Include(c => c.AvailableCults)
                    .ThenInclude(ac => ac.BonusSkills)
                .OrderBy(c => c.Name)
                .ToListAsync();
            return new Result<List<CultureDto>> { Value = _mapper.Map<List<CultureDto>>(cultures) };
        }
        catch (Exception)
        {
            return new Result<List<CultureDto>> { IsError = true, Error = "A server error occurred" };
        }
    }

    public async Task<Result<CultureDto>> GetCultureByIdAsync(Guid id)
    {
        try
        {
            var culture = await _context.Cultures
                .Include(c => c.BonusAttributes)
                .Include(c => c.BonusSkills)
                .Include(c => c.AvailableCults)
                    .ThenInclude(ac => ac.BonusSkills)
                .FirstOrDefaultAsync(c => c.Id == id);
            if (culture is null)
                return new Result<CultureDto> { IsError = true, Error = "Culture not found" };

            return new Result<CultureDto> { Value = _mapper.Map<CultureDto>(culture) };
        }
        catch (Exception)
        {
            return new Result<CultureDto> { IsError = true, Error = "A server error occurred" };
        }
    }

    public async Task<Result<object>> CreateCultureAsync(CultureCreateDto cultureCreate)
    {

        try
        {
            var culture = _mapper.Map<Culture>(cultureCreate);

            foreach (var cultDto in cultureCreate.AvailableCults)
            {
                var existingCult = await _context.Cults
                    .FirstOrDefaultAsync(s => s.Id == cultDto.Id);
                if (existingCult is null)
                    return new Result<object> { IsError = true, Error = "Cult not found" };

                culture.AvailableCults.Add(existingCult);
            }

            foreach (var attributeDto in cultureCreate.BonusAttributes)
            {
                var existingAttribute = await _context.Attributes
                    .FirstOrDefaultAsync(s => s.Id == attributeDto.Id);
                if (existingAttribute is null)
                    return new Result<object> { IsError = true, Error = "Attribute not found" };

                culture.BonusAttributes.Add(existingAttribute);
            }

            foreach (var skillDto in cultureCreate.BonusSkills)
            {
                var existingSkill = await _context.Skills
                    .FirstOrDefaultAsync(s => s.Id == skillDto.Id);
                if (existingSkill is null)
                    return new Result<object> { IsError = true, Error = "Skill not found" };

                culture.BonusSkills.Add(existingSkill);
            }

            _context.Cultures.Add(culture);
            await _context.SaveChangesAsync();
            return new Result<object> { Value = null };
        }
        catch (Exception)
        {
            return new Result<object> { IsError = true, Error = "A server error occurred" };
        }
    }

    public async Task<Result<object>> UpdateCultureAsync(CultureDto cultureDto)
    {

        try
        {
            var existingCulture = await _context.Cultures
                .Include(c => c.AvailableCults)
                .Include(c => c.BonusAttributes)
                .Include(c => c.BonusSkills)
                .FirstOrDefaultAsync(c => c.Id == cultureDto.Id);
            if (existingCulture is null)
                return new Result<object> { IsError = true, Error = "Culture not found" };

            _mapper.Map(cultureDto, existingCulture);

            existingCulture.AvailableCults.Clear();
            foreach (var cultDto in cultureDto.AvailableCults)
            {
                var existingCult = await _context.Cults
                    .FirstOrDefaultAsync(s => s.Id == cultDto.Id);
                if (existingCult is null)
                    return new Result<object> { IsError = true, Error = "Cult not found" };

                existingCulture.AvailableCults.Add(existingCult);
            }

            existingCulture.BonusAttributes.Clear();
            foreach (var attributeDto in cultureDto.BonusAttributes)
            {
                var existingAttribute = await _context.Attributes
                    .FirstOrDefaultAsync(s => s.Id == attributeDto.Id);
                if (existingAttribute is null)
                    return new Result<object> { IsError = true, Error = "Attribute not found" };

                existingCulture.BonusAttributes.Add(existingAttribute);
            }

            existingCulture.BonusSkills.Clear();
            foreach (var skillDto in cultureDto.BonusSkills)
            {
                var existingSkill = await _context.Skills
                    .FirstOrDefaultAsync(s => s.Id == skillDto.Id);
                if (existingSkill is null)
                    return new Result<object> { IsError = true, Error = "Skill not found" };

                existingCulture.BonusSkills.Add(existingSkill);
            }

            await _context.SaveChangesAsync();
            return new Result<object> { Value = null };
        }
        catch (Exception)
        {
            return new Result<object> { IsError = true, Error = "A server error occurred" };
        }
    }

    public async Task<Result<object>> DeleteCultureAsync(Guid id)
    {
        try
        {
            var culture = await _context.Cultures
                .Include(c => c.AvailableCults)
                .Include(c => c.BonusAttributes)
                .Include(c => c.BonusSkills)
                .FirstOrDefaultAsync(c => c.Id == id);
            if (culture is null)
                return new Result<object> { IsError = true, Error = "Culture not found" };

            _context.Cultures.Remove(culture);
            await _context.SaveChangesAsync();
            return new Result<object> { Value = null };
        }
        catch (Exception)
        {
            return new Result<object> { IsError = true, Error = "A server error occurred" };
        }
    }
}