using AutoMapper;
using DataAccessLayer;
using Degenesis.Shared.DTOs.Characters;
using Domain.Burns;
using Domain.Characters;
using Microsoft.EntityFrameworkCore;

namespace Business.Characters;
public interface IAttributeService
{
    Task<AttributeDto?> GetAttributeByIdAsync(Guid id);
    Task<IEnumerable<AttributeDto>> GetAllAttributesAsync();
    Task<AttributeDto?> CreateAttributeAsync(AttributeCreateDto attributeCreate);
    Task<bool> UpdateAttributeAsync(AttributeDto attribute);
    Task<bool> DeleteAttributeAsync(Guid id);
}

public class AttributeService : IAttributeService
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public AttributeService(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<AttributeDto?> GetAttributeByIdAsync(Guid id)
    {
        var attribute = await _context.Attributes
            .Include(a => a.Skills)
            .FirstOrDefaultAsync(a => a.Id == id);
        return _mapper.Map<AttributeDto>(attribute);
    }

    public async Task<IEnumerable<AttributeDto>> GetAllAttributesAsync()
    {
        var attributes = await _context.Attributes.ToListAsync();
        return _mapper.Map<IEnumerable<AttributeDto>>(attributes);
    }

    public async Task<AttributeDto?> CreateAttributeAsync(AttributeCreateDto attributeCreate)
    {
        try
        {
            var attribute = _mapper.Map<CAttribute>(attributeCreate);
            _context.Attributes.Add(attribute);
            await _context.SaveChangesAsync();
            return _mapper.Map<AttributeDto>(attribute);
        }
        catch (Exception) { 
            return null;
        }

    }

    public async Task<bool> UpdateAttributeAsync(AttributeDto attribute)
    {
        try
        {
            var existing = await _context.Attributes
                .Include(a => a.Skills)
                .FirstOrDefaultAsync(a => a.Id == attribute.Id);
            if (existing is null) return false;

            _mapper.Map(attribute, existing);


            // Cascade IsFocusOriented to all linked Skills
            foreach (var skill in existing.Skills)
            {
                skill.IsFocusOriented = existing.IsFocusOriented;
            }

            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception) {
            return false;
        }

    }

    public async Task<bool> DeleteAttributeAsync(Guid id)
    {
        try
        {
            var attribute = await _context.Attributes.FindAsync(id);
            if (attribute is null)
                return false;

            _context.Attributes.Remove(attribute);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception)
        {
            return false;
        }

    }
}