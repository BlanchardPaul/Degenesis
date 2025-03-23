using AutoMapper;
using DataAccessLayer;
using Degenesis.Shared.DTOs.Characters;
using Domain.Burns;
using Domain.Characters;
using Microsoft.EntityFrameworkCore;

namespace Business.Characters;
public interface IAttributeService
{
    Task<CAttribute?> GetAttributeByIdAsync(Guid id);
    Task<IEnumerable<CAttribute>> GetAllAttributesAsync();
    Task<CAttribute> CreateAttributeAsync(AttributeCreateDto attributeCreate);
    Task<bool> UpdateAttributeAsync(CAttribute attribute);
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

    public async Task<CAttribute?> GetAttributeByIdAsync(Guid id)
    {
        return await _context.Attributes
            .Include(a => a.Skills) // Inclure les compétences associées
            .FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task<IEnumerable<CAttribute>> GetAllAttributesAsync()
    {
        return await _context.Attributes.ToListAsync();
    }

    public async Task<CAttribute> CreateAttributeAsync(AttributeCreateDto attributeCreate)
    {
        var attribute = _mapper.Map<CAttribute>(attributeCreate);
        _context.Attributes.Add(attribute);
        await _context.SaveChangesAsync();
        return attribute;
    }

    public async Task<bool> UpdateAttributeAsync(CAttribute attribute)
    {
        var existing = await _context.Attributes.FindAsync(attribute.Id);
        if (existing is null) return false;

        _mapper.Map(attribute, existing);

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAttributeAsync(Guid id)
    {
        var attribute = await _context.Attributes.FindAsync(id);
        if (attribute is null)
            return false;

        _context.Attributes.Remove(attribute);
        await _context.SaveChangesAsync();
        return true;
    }
}