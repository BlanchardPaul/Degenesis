using AutoMapper;
using DataAccessLayer;
using Degenesis.Shared.DTOs.Characters;
using Domain.Characters;
using Microsoft.EntityFrameworkCore;

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
            .Include(c => c.AvailableCults)
            .Include(c => c.BonusAttributes)
            .Include(c => c.BonusSkills)
            .ToListAsync();
        return _mapper.Map<IEnumerable<CultureDto>>(cultures);
    }

    public async Task<CultureDto?> GetCultureByIdAsync(Guid id)
    {
        var culture = await _context.Cultures
            .Include(c => c.AvailableCults)
            .Include(c => c.BonusAttributes)
            .Include(c => c.BonusSkills)
            .FirstOrDefaultAsync(c => c.Id == id);

        return culture is null ? null : _mapper.Map<CultureDto>(culture);
    }

    public async Task<CultureDto?> CreateCultureAsync(CultureCreateDto cultureCreate)
    {

        try
        {
            var culture = _mapper.Map<Culture>(cultureCreate);

            // Attacher les Cults existants
            culture.AvailableCults = await _context.Cults.Where(c => cultureCreate.AvailableCults.Select(x => x.Id).Contains(c.Id)).ToListAsync();
            culture.BonusAttributes = await _context.Attributes.Where(a => cultureCreate.BonusAttributes.Select(x => x.Id).Contains(a.Id)).ToListAsync();
            culture.BonusSkills = await _context.Skills.Where(s => cultureCreate.BonusSkills.Select(x => x.Id).Contains(s.Id)).ToListAsync();

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
            .FirstOrDefaultAsync(c => c.Id == cultureDto.Id);

            if (existingCulture is null)
                return false;

            _mapper.Map(cultureDto, existingCulture);

            existingCulture.AvailableCults = await _context.Cults.Where(c => cultureDto.AvailableCults.Select(x => x.Id).Contains(c.Id)).ToListAsync();
            existingCulture.BonusAttributes = await _context.Attributes.Where(a => cultureDto.BonusAttributes.Select(x => x.Id).Contains(a.Id)).ToListAsync();
            existingCulture.BonusSkills = await _context.Skills.Where(s => cultureDto.BonusSkills.Select(x => x.Id).Contains(s.Id)).ToListAsync();

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
            var culture = await _context.Cultures.FindAsync(id);
            if (culture is null)
                return false;

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