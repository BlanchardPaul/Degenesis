using AutoMapper;
using DataAccessLayer;
using Degenesis.Shared.DTOs.Characters.CRUD;
using Microsoft.EntityFrameworkCore;

namespace Business.Characters;
public interface ICharacterAttributeService
{
    Task<bool> UpdateCharacterAttributeAsync(CharacterAttributeDto characterAttribute);
}

public class CharacterAttributeService : ICharacterAttributeService
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public CharacterAttributeService(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<bool> UpdateCharacterAttributeAsync(CharacterAttributeDto characterAttribute)
    {
        try
        {
            var existingCharacterAttribute = await _context.CharacterAttributes
                .Include(a => a.Attribute)
                .Include(a => a.Character)
                .FirstOrDefaultAsync(ca => ca.CharacterId == characterAttribute.CharacterId && ca.AttributeId == characterAttribute.AttributeId) ?? throw new Exception("CharacterAttribute not found");

            // We ignore the Attribute and character in the mapper, it will never be changed from here
            _mapper.Map(characterAttribute, existingCharacterAttribute);

            existingCharacterAttribute.Attribute = await _context.Attributes.FirstOrDefaultAsync(a => a.Id == characterAttribute.AttributeId) ?? throw new Exception("Attribute not found");
            existingCharacterAttribute.Character = await _context.Characters.FirstOrDefaultAsync(c => c.Id == characterAttribute.CharacterId) ?? throw new Exception("Character not found");
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
}
